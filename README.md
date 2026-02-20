# appointment-scheduler

A quick MVP meant to schedule appointments for a doctor's office. 

## Instructions

This project is a simple doctor appointment scheduler. Your program should read a series of appointment requests from a queue and then schedule these appointments for the practice. When you run your program, it should retrieve the current appointment list, then retrieve and schedule a new appointment for each appointment request in the queue. Your program will interact with the practice's schedule using a simple web API created for this project. You can find swagger docs for the API here:  https://scheduling.interviews.brevium.com/swagger/index.html   
 
This project can be completed using the programming language of your choice, but scripting languages (Bash, PowerShell, JavaScript, etc.) are not allowed. 

Constraints 

You may only request the initial state of the schedule once per run of your program. 
Scheduling constraints 
Appointments may only be scheduled on the hour. 
Appointments can be scheduled as early as 8 am UTC and as late as 4 pm UTC. 
Appointments may only be scheduled on weekdays during the months of November and December 2021. 
Appointments can be scheduled on holidays. 
For a given doctor, you may only have one appointment scheduled per hour (though different doctors may have appointments at the same time). 
For a given patient, each appointment must be separated by at least one week. For example, if Bob Smith has an appointment on 11/17 you may schedule another appointment on or before 11/10 or on or after 11/24. 
Appointments for new patients may only be scheduled for 3 pm and 4 pm. 
We have set up the initial schedule and appointment requests in a way that prevents you from getting into a state where an appointment is impossible to schedule. The initial schedule is sparse enough that the first appointments that you schedule cannot completely block a later appointment from being scheduled. That should make the problem significantly easier. 