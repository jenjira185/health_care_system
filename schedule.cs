using System;
using System.Collections.Generic;
using System.Security.Cryptography;
namespace HealthCareSystem;

public class Appointment
{
    public int AppointmentID { get; set; }
    public string PatientName { get; set; }
    public string personnelName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }

    public Appointment(int id, string patient, string personnel, DateTime start, DateTime end, string status)
    {
        AppointmentID = id;
        PatientName = patient;
        personnelName = personnel;
        StartDate = start;
        EndDate = end;
        Status = status;
    }

    public override string ToString()
    {
        return $"{AppointmentID}: {PatientName} with {personnelName} on {StartDate.ToShortDateString()} ({Status})";
    }
}

public class Schedule
{
    public List<Appointment> Appointments { get; set; }

    public Schedule()
    {
        Appointments = new List<Appointment>();
    }

    public void AddAppointment(Appointment newAppointment)
    {
        Appointments.Add(newAppointment);
    }

    public void ViewSchedule()
    {
        if (Appointments.Count == 0)
        {
            Console.WriteLine("No appointments scheduled.");
            return;
        }

        Console.WriteLine("Scheduled appointments: ");
        foreach (var a in Appointments)
        {
            Console.WriteLine(a.ToString());
        }
    }
}
