using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication8
{
    class InteractClass
    {
        static void Main(string[] args)
        {
            int key;
            int count = -1;
            Program[] objList = new Program[30];
            do
            {
                
                Console.WriteLine("\n--------------------------------------------------\n");
                Console.WriteLine("\nChoose one of these \n1.Extract time value \n2.View all \n3.Exit");
                key = Int32.Parse(Console.ReadLine());
                switch (key)
                {
                    case 1:
                        count++;
                        objList[count] = new Program();
                        Console.WriteLine("\nEnter sentence : ");
                        string text = Console.ReadLine();
                        objList[count].extractTime(text);
                        break;

                    case 2:
                        
                        Console.WriteLine("\n--------------------------------------------------\n");
                        for (int i = 0; i <= count; i++)
                        {
                            objList[i].ShowAll();
                        }
                        Console.WriteLine("\n--------------------------------------------------\n");
                        break;

                    case 3: break;
                    default: Console.WriteLine("Invalid choice"); break;


                }
            } while (key != 3);
            

            
           


        }

    }
}
