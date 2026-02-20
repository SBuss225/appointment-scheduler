// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using app.Services;

// TODO: allow for retries 
Console.WriteLine("Please enter your api key:");

string apiKey = Console.ReadLine();
// TODO: add more validation here
SchedulingService schedulingService = new SchedulingService(apiKey);

Debug.WriteLine("service initialized");
