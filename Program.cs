//As a user = Log in/out
//As a user = request registration as a patient

//As an Admin with sufficent permissions = give admins the permission to handle --
//-- the permission system 
//As an Admin with sufficent permissions = give admins the permission to create accounts for personnel
//As an Admin with sufficent permissions = give Admins the permission to view a list of who has permission to what
//As an Admin with sufficent permissions = add locations
//As an Admin with sufficent permissions = accept user registration as patients
//As an Admin with sufficent permissions = deny user registration as patient
//As an Admin with sufficent permissions = create accounts for personnel
//As an Admin with sufficent permissions = view a list of who has permission to what

//As an Personnel with sufficent permissions = view a patients journal entries
//As an Personnel with sufficent permissions = mark journal entries with diff levels of read permissions
//As an Personnel with sufficent permissions = register appointments
//As an Personnel with sufficent permissions = modify appointments
//As an Personnel with sufficent permissions = approve appointment request
//As an Personnel with sufficent permissions = view the schedule of a location

//As a patient = view my own journals
//As a patient = request an appointment 
//As a patient = view my schedule


using System.Diagnostics;
using HealthCareSystem;


List<User> users = new();
List<Journal> journals = new List<Journal>();
List<Appointment> appointments = new List<Appointment>();

if (File.Exists("users.save"))
{
    string[] lines = File.ReadAllLines("users.save");
    foreach (string line in lines)
    {
        string[] data = line.Split(",");
        users.Add(new(data[0], data[1]));
    }
}

if (File.Exists("journal.save"))
{
    string[] lines = File.ReadAllLines("journal.save");
    foreach (string line in lines)
    {
        string[] data = line.Split(",");
        User? journal_owner = null;
        journal_owner = users.Find(user => user.Email == data[2]);
        Debug.Assert(journal_owner != null);

        journals.Add(new Journal(data[0], data[1], journal_owner));
    }
}

if (File.Exists("appointment.save"))
{
    string[] lines = File.ReadAllLines("appointment.save");
    foreach (string line in lines)
    {
        string[] details = line.Split(",");

        User? appointment_patient = null;
        User? appointment_personnel = null;

        string[] trade_users = details[0].Split(",");
        string patient_email = trade_users[0];
        string personnel_email = trade_users[1];

        foreach (User user in users)
        {
            if (user.Email == patient_email)
            {
                appointment_patient = user;
            }
            if (user.Email == personnel_email)
            {
                appointment_personnel = user;
            }
            if (appointment_patient != null && appointment_personnel != null)
            {
                break;
            }
        }


        Debug.Assert(appointment_patient != null);
        Debug.Assert(appointment_personnel != null);

        Appointment saved_appointment = new Appointment(appointment_patient, appointment_personnel);

        switch (details[details.Length - 1])
        {
            case "Pending":
                saved_appointment.Status = AppointmentStatus.Pending;
                break;
            case "Accepted":
                saved_appointment.Status = AppointmentStatus.Accepted;
                break;
            case "Denied":
                saved_appointment.Status = AppointmentStatus.Denied;
                break;
            default:
                Debug.Fail("Invalid status found in appointment");
                break;
        }

        for (int i = 0; i < details.Length - 1; ++i)
        {
            string[] data = details[i].Split(".");

            foreach (Journal journal in journals)
            {
                if (journal.Name == data[0])
                {
                    Debug.Assert(journal.Owner.Email == data[1]);
                    saved_appointment.Journals.Add(journal);
                    break;
                }
            }
        }

        appointments.Add(saved_appointment);
    }
}


User? user_active = null;
Admin admin = Admin.None;

bool running = true;
while(running)
{
    try { Console.Clear(); } catch { }
    switch(admin)
    {
        case Amin.None:
        {
                if (user_active == null)
                {
                    Console.WriteLine("[1] . Login");
                    Console.WriteLine("[2] - Logout");
                    Console.WriteLine("[3] - Quit");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            admin = Admin.Login;
                            break;
                        case "2":
                            admin = Admin.Register;
                            break;
                        case "3":
                            running = false;
                            break;
                    }
                }
            else
            {
                admin = Admin.Main;
            }
        } break;

        case Admin.Login:
        {
            try { Console.Clear(); } catch { }
                Console.Write("email: ");
                string? email = Console.ReadLine();
                try { Console.Clear(); } catch { }
                Console.Write("password: ");
                string? password = Console.ReadLine();

                try { Console.Clear(); } catch { }
                Debug.Assert(email != null);
                Debug.Assert(password != null);

                foreach (User user in users)
                {
                    if (user.TryLogIn(email, password))
                    {
                        user_active = user;
                        break;
                    }
                }
                admin = Admin.None;
        } break;

        case Admin.Register:
        {
            try { Console.Clear(); } catch { }
                Console.Write("email: ");
                string? email = Console.ReadLine();
            try { Console.Clear(); } catch { }
                Console.Write("password: ");
                string? password = Console.ReadLine();

            try { Console.Clear(); } catch { }
                Debug.Assert(email != null);
                Debug.Assert(password != null);

            users.Add(new User(email, password));

            string[] users_save_string = new string[users.Count];
            for (int i = 0; i < users.Count; ++i)
            {
                users_save_string[i] = users[i].ToSaveString();
            }
            File.WriteAllLines("users.saves", users_save_string);

            admin = Admin.None;
        } break;

        case Admin.Main;
        {
            try { Console.Clear(); } catch { }
                Console.WriteLine("------Trading System-----");
                Console.WriteLine("[1] - Add new item");
                Console.WriteLine("[2] - View item listings");
                Console.WriteLine("[3] - Send trade request");
                Console.WriteLine("[4] - Logout");
                Console.WriteLine("[5] - Quit");

            switch(Console.ReadLine())
            {
                case "1":
                {
                    try { Console.Clear(); } catch { }
                        Console.Write("name");
                         string? name = Console.ReadLine();
                    try { Console.Clear(); } catch { }
                        Console.Write("description");
                        string? description = Console.ReadLine();

                    try { Console.Clear(); } catch { }
                        Debug.Assert(name != null);
                        Debug.Assert(description != null);
                            //Debug.Assert(user_active != null);

                    journals.Add(new Journal(name, description, user_active));
                    string[] journals_save_string = new string[journals.Count];
                    for (int i = 0; i < journals.Count; ++i)
                    {
                        journals_save_string[i] = journals[i].ToSaveString();
                    }
                    File.WriteAllLines("journals.save", journals_save_string);

                    admin = Admin.None;
                } break;

                case "2":
                {
                    try { Console.Clear(); } catch { }
                    foreach (Journal journal in journals)
                    {
                        if (journal.Owner != user_active)
                        {
                            Console.WriteLine(journal.Info());
                        }
                    }
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ReadLine();      
                } break;

                case "3":
                    admin = Admin.Trade;
                    break;

                case "L":
                    user_active = null;
                    admin = Admin.None;
                    break;
                case "Q":
                    running = false;
                    break;
            }    
        } break;

        case Admin.Trade:
        {
            try { Console.Clear(); } catch { }
                for (int i = 0; i < users.Count; ++i)
                {
                    if (users[i] != user_active)
                    {
                        Console.WriteLine($"id: {i} - email: {users[i].Email}");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Enter ID of user to trade with");
                Console.Write("ID: ");
                string? input_index = Console.ReadLine();
                Debug.Assert(input_index != null);

                User? user_trade = null;

                if (int.TryParse(input_index, out int index) && index < users.Count)
                {
                    user_trade = users[index];
                }

                if (user_trade != user_active && user_trade != null)
                {
                    Debug.Assert(user_active != null);
                    Trade trade = new Trade(user_active, user_trade);

                    bool trading = true;
                    while (trading)
                    {
                        try { Console.Clear(); } catch { }
                        Console.WriteLine($"(user_trade.Email)'s journals: ");
                        for (int i = 0; i < journals.Count; ++i)
                        {
                            Journal journal = journals[i];
                            if (journal.Owner == user_trade)
                            {
                                Console.WriteLine($"[{i}] - {journal.Name}");
                            }
                        }

                        Console.WriteLine("---------------");
                        Console.WriteLine("Your journal:");
                        for (int i = 0; i < journals.Count; ++i)
                        {
                            Journal journal = journals.[i];
                            if (journal.Owner == user_active)
                            {
                                Console.WriteLine($"[{i}] - {journal.Name}");
                            }
                        }

                        Console.WriteLine("Enter DONE in order to finalize trade");
                        Console.WriteLine("Enter ID for item you would like to add to the trade");
                        string? journal_input_index = Console.ReadLine();
                        Debug.Assert(journal_input_index != null);

                        if (int.TryParse(journal_input_index, out int journal_index) && journal_index < journals.Count)
                        {
                            Journal journal_for_trade = journals[journal_index];
                            if (journal_for_trade.Owner == user_active || journal_for_trade.Owner == user_trade)
                            {
                                trade.Journals.Add(journal_for_trade);
                            }
                        }
                        else if (journal_input_index == "DONE")
                        {
                            trading = false;
                        }
                    }

                    trades.Add(trade);
                    string[] trades_save_string = new string { trades.Count };
                    for (int i = 0; i < trades.Count; ++i)
                    {
                        trades_save_string[i] = trades[i].ToSaveString();
                    }
                    File.WriteAllLines("trades.save", trades_save_string);
                }
                admin = Admin.None;
        } break;
    }
}

internal class Appointment
{
}

internal class Journal
{
}

internal class User
{
}















/*
 static void Main(string[] args)

    Patient patient = new Patient("Alice", "1234", "Has mild allergy.");
    Personnel doctor = new Personnel("Dr. Bob", "pass", "Clinic A");
    Admin admin = new Admin("SuperAdmin", "admin123");

    //-------Admin's actions------
    admin.Login("admin123");
    admin.AddLocation("Clinic A");
    admin.HandlePermissions("ModifyAppointments");
    admin.CreatePersonnelAccount("Dr. Bob", "pass", "Clinic A");
    admin.ViewPermissionsList();
    admin.Logout();


    //-----Patient actions-----
    patient.Login("1234");
    patient.ViewJournal();
    patient.RequestAppointment("2025-10-20");
    patient.ViewSchedule();
    patient.Logout();


    //----Personnel actions------
    doctor.Login("pass");
    doctor.Patient.Add(patient);
    doctor.RegisterAppointment(patient, "2025-10-20");
    doctor.ViewPatientJournals();
    doctor.ModifyAppointment(patient, "2025-10-20", "2025-10-22");
    doctor.ApproveAppointment("2025-10-22");
    doctor.ViewLocationSchedule();
    doctor.ViewSchedule();
    doctor.Logout();

    Console.WriteLine("\n--- End of the system demo ---");

*/