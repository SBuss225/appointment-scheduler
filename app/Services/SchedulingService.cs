using System.IO.Pipelines;
using System.Net.Http.Json;
using System.Text.Json;
using app.Models;

namespace app.Services
{
  public class SchedulingService
  {
    private static class Endpoints
    {
      public const string BASE_URL = "https://scheduling.interviews.brevium.com/api/Scheduling/";
      public const string START = "Start";
      public const string STOP = "Stop";
      public const string APPOINTMENT_REQUEST = "AppointmentRequest";
      public const string SCHEDULE = "Schedule";
    }
    
    private string apiKey;
    private HttpClient client;
    private AppointmentInfo[] schedule;
    public SchedulingService(string apiKey)
    {
      this.client = new HttpClient();
      client.BaseAddress = new Uri(Endpoints.BASE_URL);
      this.apiKey = apiKey;
      if (string.IsNullOrWhiteSpace(apiKey))
      {
        throw new ArgumentException($"invalid apiKey provided: {apiKey ?? "null"}");
      }
      try
      {
        this.RestartApi();
      } catch (Exception e)
      {
        throw new Exception($"Unable to connect to the scheduling api: {e.Message}");
      }
    }

    public Task<AppointmentInfo[]?> GetInitialSchedule()
    {
        HttpResponseMessage response = client.GetAsync(formatRequestUri(Endpoints.SCHEDULE)).Result;
        response.EnsureSuccessStatusCode();
        // TODO: add validation to ensure data is in the correct format
        return response.Content.ReadFromJsonAsync<AppointmentInfo[]>();
    }

    public async Task ScheduleAllNewAppointments()
    {
      try
      {
        var initialSchedule = await this.GetInitialSchedule();
        if (initialSchedule != null)
        {
          this.schedule = initialSchedule;
        } else
        {
          throw new Exception("Unable to retrieve initial schedule.");
        }
      } catch(Exception e)
      {
        throw e;
        // TODO: add error handling
      }
      
      AppointmentRequest? appointmentRequest = await this.GetNextAppointmentRequest();
      while (appointmentRequest != null)
      {
        AppointmentInfoRequest newAppointment = CalculateNewAppointment(appointmentRequest);
        this.AddAppointmentToSchedule(newAppointment);
        appointmentRequest = await this.GetNextAppointmentRequest();
      }      
    }

    private Task<AppointmentRequest?> GetNextAppointmentRequest()
    {
      HttpResponseMessage response = client.GetAsync(formatRequestUri(Endpoints.APPOINTMENT_REQUEST)).Result;
      response.EnsureSuccessStatusCode();
      // TODO: add validation to ensure data is in the correct format
      return response.Content.ReadFromJsonAsync<AppointmentRequest>();
    }

    private void AddAppointmentToSchedule(AppointmentInfoRequest newAppointmentInfo)
    {
      HttpResponseMessage response = client.PostAsJsonAsync<AppointmentInfoRequest>(formatRequestUri(Endpoints.SCHEDULE), newAppointmentInfo).Result;
      response.EnsureSuccessStatusCode();
      // TODO: add appointment to local copy of schedule
    }

    private AppointmentInfoRequest CalculateNewAppointment(AppointmentRequest appointmentRequest)
    {
      int patientId = appointmentRequest.personId;
      // assuming that each date in preferredDays only indicates the desired day, not the time as well
      for (int i = 0; i < appointmentRequest.preferredDays.Length; i++)
      {
        DateTime currDate = appointmentRequest.preferredDays[i];
        if (!IsPatientAvailable(currDate, patientId) || !isValidDate(currDate))
        {
          continue;
        }
        
        int startTime;
        int endTime = 16; // 4pm
        if (appointmentRequest.isNew)
        {
          startTime = 15; // 3pm
        } else
        {
          startTime = 8; // 8am
        }

        for (int j = startTime; j <= endTime; j++)
        {
          // check if preferredDoctors are available
        }
      }

      // should we schedule the patient a date outside of their preferred days range if not available?

      // TODO: update test return val
      return new AppointmentInfoRequest(Doctor.ONE, 1, new DateTime(2021, 1, 1, 3, 0, 0), true, 1);
    }

    private bool isValidDate(DateTime requestedDate)
    {
      if (requestedDate.Month >= 11)
      {
        return requestedDate.DayOfWeek != DayOfWeek.Saturday && requestedDate.DayOfWeek != DayOfWeek.Sunday;
      }
      return true;
    }

    // ensure that the requested date is seperated from all other appointments for the given patient by at least one week
    private bool IsPatientAvailable(DateTime requestedDate, int patientId)
    {
      // Could probably find a way to do this less frequently to speed things up, tradeoffs?
      Dictionary<int, List<AppointmentInfo>> appointmentsByPatient = schedule
        .GroupBy(appt => appt.personId)
        .ToDictionary(patientAppts => patientAppts.Key, patientAppts => patientAppts.ToList());
      List<AppointmentInfo> currentPatientAppointments = appointmentsByPatient[patientId];
      
      DateTime ONE_WEEK_BEFORE_APPT = requestedDate.Date.AddDays(-7);
      DateTime ONE_WEEK_AFTER_APPT = requestedDate.Date.AddDays(7);
      foreach (AppointmentInfo appt in currentPatientAppointments)
      {
        if (appt.appointmentTime.Date.CompareTo(ONE_WEEK_BEFORE_APPT) > 0 &&
            appt.appointmentTime.Date.CompareTo(ONE_WEEK_AFTER_APPT) < 0)
        {
          return false;
        }
      }
      return true;
    }
    private void RestartApi()
    {
      HttpResponseMessage response = client.PostAsync(formatRequestUri(Endpoints.START), null).Result;
      response.EnsureSuccessStatusCode();
    }

    private string formatRequestUri(string endpoint)
    {
      return $"{endpoint}?token={apiKey}";
    }
  }
}