using System.IO.Pipelines;
using System.Net.Http.Json;
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