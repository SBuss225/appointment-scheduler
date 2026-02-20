// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using app.Models;
using app.Services;

// TODO: allow for retries 
// NOTE: If this were hosted, I would store the api key in AWS Secret or another credential vault rather than requesting each time
Console.WriteLine("Please enter your api key:");

string apiKey = Console.ReadLine();
// TODO: add more validation here
SchedulingService schedulingService = new SchedulingService(apiKey);
Debug.WriteLine("service initialized");

schedulingService.ScheduleAllNewAppointments();
// call stop endpoint
Console.WriteLine("All new appointments scheduled.");


