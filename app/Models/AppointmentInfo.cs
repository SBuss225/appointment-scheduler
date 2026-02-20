namespace app.Models
{
  public class AppointmentInfo(Doctor doctorId, int personId, DateTime appointmentTime, bool isNewPatientAppointment)
  {
    public Doctor doctorId = doctorId;
    public int personId = personId;
    public DateTime appointmentTime = appointmentTime;
    public bool isNewPatientAppointment = isNewPatientAppointment;
  }
}