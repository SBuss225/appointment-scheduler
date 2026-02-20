namespace app.Models
{
  public class AppointmentRequest(int requestId, int personId, DateTime[] preferredDays, Doctor[] preferredDocs, bool isNew)
  {
    public int requestId = requestId;
    public int personId = personId;
    public DateTime[] preferredDays = preferredDays;
    public Doctor[] preferredDocs = preferredDocs;
    public bool isNew = isNew;
  }
}