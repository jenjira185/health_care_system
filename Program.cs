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




using System.Collections;
using System.Diagnostics;
using System.Reflection.Metadata;
using HealthCareSystem;


List<User> users = new();
List<Appointment> appointments = new List<Appointment>();
List<Journal> journals = new List<Journal>();
List<Location> locations = new List<Location>();

User? user_active = null;
Menu menu = menu.None;

bool running = true;

while(running)
{
    try { Console.Clear(); }
    catch { }
    switch (menu)
    {
        case menu.None;
    }
    if (user_active == null)
    {
        Console.WriteLine("[1] Login");
        Console.WriteLine("[2] Register");
        Console.WriteLine("[3] quit");
        switch (Console.ReadLine())
        {
            case "1": menu = Menu.Login;
                break;
            case "2": menu = Menu.Register;
                break;
            case "3": running = false;
                break;
        }
    }
    else
    {
        menu = Menu.Main;
    }
    break;

//HELP 
    case Menu.Login:
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
                if (user.TryLogin(email, password))
                {
                    user_active = user;
                    break;
                }
            }
            menu = Menu.None;
        } break;

    case Menu.Register:
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
        } break;

        
}

//commenting this out while waiting....
