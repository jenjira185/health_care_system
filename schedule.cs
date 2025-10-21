namespace HealthCareSystem;

class Schedule
{
    public List<Appointment> appointments;

    public schedule()
    {
        appointments = new List<Appointment>();
    }

    public void AddAppointment(string details)
    {
        appointments.Add(details);
    }

    public List<string> ViewSchedule()
    {
        return new List<string>(appointments);
    }
    public void RemoveAppointment(string details)
    {
        appointments.Remove(details);
    }
}