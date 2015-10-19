# Natural-Language-Processing
Natural-Language-Processing For internship

Here are a few examples which work fine

	All the dates are in the format yyyy-mm-dd

1. (stamp)I went there last year -->2014
2. (stamp)Make a reservation day after tomorrow at seven in the evening-->2015-10-12T19:00
3. (stamp)My exam is on the first thursday of next month-->2015-11-05
4. (trigger)I have been working in this company since june 2012 --> Start : 2012-06
5. (trigger)Anytime after 5pm is good--> Start: 2015-10-10T17:00
6. (trigger)Before 2pm I am free--> End:2015-10-10T14:00
7. (trigger)I will finish the task before 10 december 2015-->Start now(2015-10-10) End:2015-12-10
8. (trigger)She had ordered the bag before july-->End:2015-07
9. (period)I was working in LA from july 2012 to november 2014-->2012-07/2014-11


	Few examples for finding time period didn't work out. Eg: I worked in LA for last 2 years.
	It is detecting the time as a stamp and hence giving wrong results. 
	It can be worked out by filtering the results further. 
