using java.util;
using java.io;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.tagger.maxent;
using edu.stanford.nlp.time;
using edu.stanford.nlp.util;
using edu.stanford.nlp.ling;
using Console = System.Console;
using System;



namespace ConsoleApplication8
{
    class Program
    {
        string sentenceInput;
        string typeTime;
        string valueTime;

        public void ShowAll()
        {
            Console.WriteLine("\nSentence - " + sentenceInput + " ; Type - " + typeTime + " ; Value - " + valueTime); 
        
        }


        public void extractTime(string text)
        {
            sentenceInput = text;
            string presentDate = "2015-10-10";
            string curr = Environment.CurrentDirectory;
            var jarRoot = curr + @"\stanford-corenlp-3.5.2-models";
            var modelsDirectory = jarRoot + @"\edu\stanford\nlp\models";

            // Annotation pipeline configuration
            var pipeline = new AnnotationPipeline();
            pipeline.addAnnotator(new TokenizerAnnotator(false));
            pipeline.addAnnotator(new WordsToSentencesAnnotator(false));

            // SUTime configuration
            var sutimeRules = modelsDirectory + @"\sutime\defs.sutime.txt,"
                              + modelsDirectory + @"\sutime\english.holidays.sutime.txt,"
                              + modelsDirectory + @"\sutime\english.sutime.txt";

            var props = new Properties();

            props.setProperty("sutime.rules", sutimeRules);
            props.setProperty("sutime.binders", "0");
            props.setProperty("sutime.markTimeRanges", "true");
            props.setProperty("sutime.includeRange", "true");

            pipeline.addAnnotator(new TimeAnnotator("sutime", props));

            // Sample text for time expression extraction
            
            var annotation = new Annotation(text);
            annotation.set(new CoreAnnotations.DocDateAnnotation().getClass(), presentDate);
            pipeline.annotate(annotation);

            //  Console.WriteLine("{0}\n", annotation.get(new CoreAnnotations.TextAnnotation().getClass()));

            var timexAnnsAll = annotation.get(new TimeAnnotations.TimexAnnotations().getClass()) as ArrayList;
            foreach (CoreMap cm in timexAnnsAll)
            {

                var time = cm.get(new TimeExpression.Annotation().getClass()) as TimeExpression;

                string typeTimex = time.getTemporal().getTimexType().toString();
                if (typeTimex.ToLower() == "duration")
                {
                    typeTime = "tPeriod";
                    valueTime = time.getTemporal().toISOString();
                    Console.WriteLine(valueTime);
                }

                if (typeTimex.ToLower() == "time" || typeTimex.ToLower() == "date")
                {
                    string textOftime = time.getText().ToString();

                    char[] delimiterChars = { ' ' };
                    string[] words = textOftime.Split(delimiterChars);

                    string mainword = words[0];
                    var tagger = new MaxentTagger(modelsDirectory + @"\pos-tagger\english-bidirectional\english-bidirectional-distsim.tagger");

                    var sentences = MaxentTagger.tokenizeText(new StringReader(text));
                    var first = sentences.get(0) as ArrayList;
                    int size = first.size();

                    int i = 0;
                    int index = -3;
                    while (i < size)
                    {
                        if (first.get(i).ToString() == mainword)
                            index = i;

                        i++;
                    }
                    var taggedSentence = tagger.tagSentence(first);

                    string checker = taggedSentence.get(index - 1).ToString();
                    if (checker.ToLower() == "after/in" || checker.ToLower() == "since/in")
                    {
                        typeTime = "tTrigger";
                        valueTime = "Start : " + time.getTemporal().toISOString();


                        Console.WriteLine(valueTime);

                    }

                    else if (checker.ToLower() == "before/in")
                    {
                        if (typeTimex == "TIME")
                        {
                            typeTime = "tTrigger";
                            valueTime = "End : " + time.getTemporal().toISOString();

                            Console.WriteLine(valueTime);
                        }
                        else
                        {
                            DateTime result = new DateTime();
                            DateTime current = DateTime.ParseExact(presentDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                            string dt = time.getTemporal().toString();
                            char[] delimiter = { '-', '-', '-' };
                            string[] partsOfDate = time.getTemporal().toISOString().Split(delimiter);
                            int count = partsOfDate.Length;
                            if (count == 1)
                            {

                                result = Convert.ToDateTime("01-01-" + partsOfDate[0]);
                            }

                            if (count == 2)
                            {
                                result = Convert.ToDateTime("01-" + partsOfDate[1] + "-" + partsOfDate[0]);
                            }

                            //  result = DateTime.ParseExact(dt, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                            int comp = DateTime.Compare(current, result);
                            if (comp < 0)
                            {
                                typeTime = "tTrigger";
                                valueTime = "Start now (" + presentDate + ") End :" + time.getTemporal().toString();
                                Console.WriteLine(valueTime);
                            }

                            else
                            {
                                typeTime = "tTrigger";
                                valueTime = "End : " + time.getTemporal().toString();
                                Console.WriteLine(valueTime);
                            }

                        }

                    }

                    else
                    {

                        typeTime = "tStamp";
                        valueTime = time.getTemporal().toISOString();
                        Console.WriteLine(valueTime);
                        
                    }
                }
            }


        }


        
    }
}
