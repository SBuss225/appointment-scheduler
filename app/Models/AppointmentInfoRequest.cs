namespace app.Models
{
  public class AppointmentInfoRequest(Doctor doctorId, int personId, DateTime appointmentTime, bool isNewPatientAppointment, int requestId)
  {
    public Doctor doctorId = doctorId;
    public int personId = personId;
    public DateTime appointmentTime = appointmentTime;
    public bool isNewPatientAppointment = isNewPatientAppointment;
    public int requestId = requestId;
  }
}