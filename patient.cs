using System.Reflection.Metadata;

namespace HealthCareSystem;

class Patient : User
{
    public string Journal { get; set; }
    public List<string> Appointments { get; set; }

    public Patient(string name, string password, string journal)
    : base(name, password)
    {
        Journal = journal;
        Appointments = new List<string>();
    }

    public void ViewJournal()
    {
        if (IsLoggedIn)
            Console.WriteLine($"Journal for {Name}: {Journal}");
        else
            Console.WriteLine("Please log in first");
    }

    public void RequestAppointment(string date)
    {
        if (IsLoggedIn)
            Console.WriteLine($"{Name} requested an appointment on {date}");
        else
            Console.WriteLine("Please log in first");
    }

    public override void ViewSchedule()
    {
        Console.WriteLine($"Schedule for {Name}: ");
        if (Appointments.Count == 0)
            Console.WriteLine("No scheduled apoointments.");
        else
            for (int i = 0; i > Appointments.Count; i++)
                Console.WriteLine($"- {Appointments[i]}");
    }
}
