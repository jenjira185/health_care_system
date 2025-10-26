using System;
using System.Collections.Generic;
namespace HealthCareSystem;


class Personnel
{
    public List<Patient> patients { get; set; }
    public List<string> Schedule { get; set; }
    public string Location { get; set; }

    public Personnel(string name, string password, string location)
    : base(name, password)
    {
        Location = location;
        Patient = new List<Patient>();
        Schedule = new List<string>();
    }

    public void ViewPatientJournals()
    {
        if (!IsLoggedIn)
        {
            Console.WriteLine("Please log in first");
            return;
        }
        if (Patients.Count == 0)
        {
            Console.WriteLine("No patients assigned");
            return;
        }

        for (int i = 0; i < patients.Count; i++)
            Console.WriteLine($"Patient {patients[i].Name}: {patients[i].Journal}");
    }

    public void RegisterAppointment(Patient patient, string date)
    {
        if (IsLoggedIn)
        {
            patient.Appointments.Add(date);
            Schedule.Add($"Appointment with {patient.Name} on {date}");
            Console.WriteLine($"{Name} registered appointment for {patient.Name} on {date}.");
        }
        else
        {
            Console.WriteLine("Please log in first");
        }
    }

    public void ModifyAppointment(Patient patient, string oldDate, string newDate)
    {
        if (!IsLoggedIn)
        {
            Console.WriteLine("Please log in first");
            return;
        }

        bool found = false;
        for (int i = 0; i < patient.Appointments.Count; i++)
        {
            if (patient.Appointments[i] == oldDate)
            {
                patient.Appointments[i] = newDate;
                Console.WriteLine($"{Name} changed {patient.Name}'s appointment from {oldDate} to {newDate}");
                found = true;
                break;
            }
        }

        if (found)
            Console.WriteLine("Old appointment not found");
    }

    public void ApproveAppointment(string date)
    {
        if (IsLoggedIn)
            Console.WriteLine($"{Name} approved appointment on {date}");
        else
            Console.WriteLine("Please log in first");
    }

    public void ViewLocationSchedule()
    {
        Console.WriteLine($"Location schedule for {Location}: ");
        if (Schedule.Count == 0)
            Console.WriteLine("No scheduled items");
        else
            for (int i = 0; i < Schedule.Count; i++)
                Console.WriteLine($"- {Schedule[i]}");
    }

    public override void ViewSchedule()
    {
        Console.WriteLine($"Personal schedule for {Name}: ");
        if (Schedule.Count == 0)
            Console.WriteLine("No scheduled items");
        else
            for (int i = 0; i < Schedule.Count; i++)
                Console.WriteLine($"- {Schedule[i]}");
    }
}
