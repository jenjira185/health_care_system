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
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using HealthCareSystem;






static int GetIndexAddOne(List<User> users)
{
    return users.Count + 1;
}

//MAIN PROGRAM

List<Location> locations = new List<Location>();
List<Appointment> appointments = new List<Appointment>();

locations.Add(new Location("Skåne", "Lunds UniversitetsSjukhus"));
locations.Add(new Location("Stockholm", "Karolinska sjukhus"));

List<User> users = FileHandler.LoadFromCsv();

User? active_user = null;
bool running = true;


Console.Clear();
StartMenu(users);


void StartMenu(List<User> users)
{
    while (true)
    {
        Console.WriteLine("Welcome to this site");
        Console.WriteLine("1. Request Registration (For patients)");
        Console.WriteLine("2. Request Registration (For Admin)");
        Console.WriteLine("3. Log In");

        switch (Utils.GetIntegerInput("Pick a number: "))
        {
            case 1:
                string newUser = Utils.GetIntegerInput("Enter your username: ");
                Console.Clear();

                string newPass = Utils.GetIntegerInput("Enter your password: ");
                Console.Clear();

                Console.WriteLine("Request sent");
                users.Add(new User(Utils.GetIndexAddOne(users), newUser, newPass, Role.Patient));
                FileHandler.SaveUsersToCsv(users);
                break;

            case 2:
                string newAdmin = Utils.GetRequiredInput("Enter your username: ");
                Console.WriteLine();

                Console.WriteLine("Enter your password: ");
                string newAdminPass = Console.ReadLine() ?? "".Trim();
                Console.Clear();

                Console.WriteLine("Request sent");
                users.Add(new User(GetIndexAddOne(users), newAdmin, newAdminPass, Role.Admin));

                FileHandler.SaveUsersToCsv(users);
                break;

            case 3:
                MainMenu();
                break;
        }
    }
}

void MainMenu()
{
    while (running)
    {
        Console.Clear();

        if (active_user = null)
        {
            Console.WriteLine("--- Health care System ----");
            string username = Utils.GetRequiredInput("Username: ");
            string password = Utils.GetRequiredInput("Password: ");

            foreach (User user in users)
            {
                if (user.TryLogin(username, password))
                {
                    active_user = user;
                    break;
                }
            }

            if (active_user = null)
            {
                Console.WriteLine("Incorrect login. Press enter to try agaon: ");
                Console.ReadLine();
            }
        }

        else
        {
            if (active_user.GetRegistration() != Registration.Accept)
            {
                Utils.DisplayAlertText("Your account is still pending. Need to wait for the admin to accept. Press ENTER to continue: ");
                Console.ReadKey();
                active_user = null;
                break;
            }

            Console.Clear();
            Console.WriteLine($"Logged in as: {active_user.Username} ({active_user.GetRole()})");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine(active_user.GetRole());

            switch (active_user.GetRole())
            {
                case Role.Admin:
                    AdminMenu(users, locations, active_user);
                    break;

                case Role.Personnel:
                    PersonnelMenu(users, active_user, appointments);
                    break;

                case Role.Patient:
                    PatientMenu(active_user, users.Where(user => user.GetRole() == Role.Personnel && user.PersonnelRole == PersonnelRoles.Doctor).ToList(), users);
                    break;
            }

            string? input = Console.ReadLine();
            if (input == "logout")
            {
                active_user = null;
                break;
            }
            else if (input == "return")
            {
                continue;
            }
        }
    }
}


static void SuperAdminMenu(List<User> users, List<Location> locations, User active_user)
{
    Console.WriteLine("\nSuperAdmin Options: ");
    Console.WriteLine("1. Grant admin to add location: ");
    Console.WriteLine("2. Overview of Permissions");
    Console.WriteLine("3. Grant admin to handle registration");
    Console.WriteLine("4. Grant admin to create personnel");
    Console.WriteLine("5. Grant admin to check list of user permissions");
    Console.WriteLine("6. View pending admin registration request");
    Console.WriteLine("7. Assign admins to certain regions");
    Console.WriteLine("8. Logout");


    int input = Utils.GetIntegerInput("Pick a number: ");

    switch (input)
    {
        case 1:
            {
                Console.WriteLine("A list of all admins");

                foreach (User user in users.Where(user => user.GetRole() == Role.Admin))
                {
                    Console.WriteLine($"{user.ToString()}");
                }

                string adminName = Utils.GetRequiredInput("Pick admin name you want to handle: ");
                User? adminUser = users.Find(user => user.Username.Equals(adminName, StringComparison.OrdinalIgnoreCase));
                if (adminUser = null)
                {
                    string acceptOrDeny = Utils.GetRequiredInput($"You picked: {adminUser.Username}. Would you like to accept(y) or deny(d) the permission for addingg location? \n");
                    switch (acceptOrDeny)
                    {
                        case "y":
                            adminUser.GrantPermission(Permissions.AddLocation);
                            Utils.DisplaySucessText($"You have denied the permission");
                            break;
                        default:
                            Utils.DisplayAlertText("Only y / n is handled");
                            break;
                    }
                }

                else
                {
                    Utils.DisplayAlertText("There is no admin with the name");
                }
            }

            FileHandler.SavedUsersCsv(users);
            break;

        case 2:
            Console.WriteLine("Overview regarding the permissions for all users");

            Console.WriteLine($"\nAll users: ");
            foreach (var user in users)
            {
                Console.WriteLine($"{user.Username} - {user.GetRole()} - Permissions: {string.Join(",", user.PermissionList)})");
            }
            Console.Write($"\nPress ENTER to continue: ");
            break;

        case 3:
            {
                Console.WriteLine("A list of admins");

                foreach (User user in users.Where(user => user.GetRole() == Role.Admin))
                {
                    Console.WriteLine($"{user.ToString()}");
                }

                string adminName = Utils.GetRequiredInput("Pick admin name you would like to handle: ");
                User? adminUser = users.Find(user => user.Username.Equals(adminName, StringComparison.OrdinalIgnoreCase));
                if (adminUser != null)
                {
                    string acceptOrDeny = Utils.GetRequiredInput($"You picked: {adminUser.Username}. Would you like to accept (y) or deny (n) the permission for handling registration request?\n");
                    switch (acceptOrDeny)
                    {
                        case "y":
                            adminUser.GrantPermission(Permissions.AddRegistrations);
                            Utils.DisplaySucessText($"You accepted the permission handle registration for user {adminName}");
                            break;

                        case "d":
                            adminUser.RevokePermission(Permissions.AddRegistrations);
                            Utils.DisplaySucessText($"You have denied permission handle registration for user {adminName}");
                            break;
                        default:
                            Utils.DisplayAlertText("Only y / n is available");
                            break;
                    }
                }

                else
                {
                    Utils.DisplayAlertText("No admin with that name found");
                }
            }

            FileHandler.SavedUsersCsv(users);
            break;

        case 4:
            {
                Console.WriteLine("A list of all admin");

                foreach (User user in users.Where(user => user.GetRole() == Role.Admin))
                {
                    Console.WriteLine($"{user.ToString()}");
                }

                string adminName = Utils.GetRequiredInput("Pick admin you would like to handle: ");
                User? adminUser = users.Find(user => user.Username.Equals(adminName, StringComparison.OrdinalIgnoreCase));
                if (adminUser != null)
                {
                    string acceptOrDeny = Utils.GetRequiredInput($"You picked: {adminUser.Username}. Would you like to accept (y) / deny (d) the permission ofr handling registratin request?\n");
                    switch (acceptOrDeny)
                    {
                        case "y":
                            adminUser.GrantPermission(Permissions.AddPersonnel);
                            Utils.DisplaySucessText($"You have accepted the permission to create personnel for admin: {adminName}");
                            break;
                        default:
                            Utils.DisplayAlertText("Only y / n is available");
                            break;
                    }
                }
                else
                {
                    Utils.DisplayAlertText("...");
                }
            }

            FileHandler.SavedUsersCsv(users);
            break;

        case 5:
            {
                Console.WriteLine("A list of all admins");

                foreach (User user in users.Where(user => user.GetRole() == Role.Admin))
                {
                    Console.WriteLine($"{user.ToString()}");
                }

                string adminName = Utils.GetRequiredInput("Pick admin name you would like to handle: ");
                User? adminUser = users.Find(user => user.Username.Equals(adminName, StringComparison.OrdinalIgnoreCase));
                if (adminUser != null)
                {
                    string acceptOrDeny = Utils.GetRequiredInput($"You picked: {adminUser.Username}. Would you like to accept (y) / deny (d) the permission for viewing all users permissions?\n");
                    switch (acceptOrDeny)
                    {
                        case "y":
                            adminUser.GrantPermission(Permission.AddAdmin);
                            Utils.DisplaySucessText($"You have accepted the permission for admin: {adminName}");
                            break;

                        case "d":
                            adminUser.RevokePermission(Permission.AddAdmin);
                            Utils.DisplaySucessText($"You have denied the permission for admin: {adminName}");
                            break;
                        default:
                            Utils.DisplayAlertText("Only y / n is available");
                            break;
                    }
                }

                else
                {
                    Utils.DisplayAlertText("No admin with the name found");
                }

            }
            FileHandler.SavedUsersCsv(users);
            break;

        case 6:
            {
                if (active_user.GetRole() == Role.SuperAdmin)
                {
                    Console.WriteLine("\nAll admins with pending request: ");
                    foreach (User user in users.Where(user => user.GetRole() == Role.Admin && user.GetRegistration() == Registration.Pending))
                    {
                        Console.WriteLine($"{user.ToString()}");
                    }

                    string adminHandling = Utils.GetRequiredInput("Pick admin username you would like to handle: ");
                    User? adminUser = users.Find(user => user.Username.Equals(adminHandling, StringComparison.OrdinalIgnoreCase));
                    if (adminUser != null)
                    {
                        string acceptOrDeny = Utils.GetRequiredInput($"You picked: {adminUser.Username}. Would you like to accpet (y) / deny (d) the request?\n");
                        switch (acceptOrDeny)
                        {
                            case "y":
                                adminUser.AcceptPending();
                                Utils.DisplaySucessText("Admin registration accepted");
                                break;

                            case "d":
                                adminUser.DenyPending();
                                Utils.DisplaySucessText("Admin registration denied");
                                break;
                            default:
                                Utils.DisplayAlertText("Only y / n is available");
                                break;
                        }
                    }
                    else
                    {
                        Utils.DisplayAlertText("No admin by the name has been found");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid inpuit. Please try again");
                }
                break;
            }
        case 7:
            Console.WriteLine("Assign admins to region");

            if (!users.Any(user => user.GetRole() == Role.Admin))
            {
                Utils.DisplayAlertText("No admins found");
                break;
            }
            List<User> AdminList = new List<User>();
            for (int i = 0; i < users.Count; ++i)
            {
                if (users[i].GetRole() == Role.Admin)
                {
                    AdminList.Add((User)users[i]);
                    Console.WriteLine(AdminList.Count + ":" + users[i].Username);
                }
            }

            int chosenIndex = Utils.GetIntegerInput("Choose an admin by number: ") - 1;
            if (chosenIndex < 0 || chosenIndex >= AdminList.Count)
            {
                Utils.DisplayAlertText("Invalid number.");
                break;
            }

            User chosenAdmin = AdminList[chosenIndex];
            Region[] regions = (Region[]Enum.GetValues(typeof(Region)));
            for (int i = 0; i < regions.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {regions[i]}");
            }

            int regionChoice = Utils.GetIntegerInput($"Chose a region for {chosenAdmin.Username}: ");
            if (regionChoice < 0 || regionChoice >= regions.Length)
            {
                Utils.DisplayAlertText("Invalid region");
                break;
            }

            Region selectRegion = regions[regionChoice];
            chosenAdmin.assignedRegion(selectRegion);
            Utils.DisplaySucessText(chosenAdmin.Username + " has been assigned to region " + selectRegion);
            break;

        case 8:
            FileHandler.SavedUsersCsv(users);
            Console.WriteLine("\n1. Write 'logout' to log out");
            Console.WriteLine("2. Write 'return' to go back");

            break;
        default:
            Utils.DisplayAlertText("invalid input");
            break;
    }
}



void AdminMenu(List<User> user, List<Location>, locations, User active_user)
{
    Console.WriteLine("\nAdmin options");
    Console.WriteLine("1. create account");
    Console.WriteLine("2. see list of all users");
    Console.WriteLine("3. add locations");
    Console.WriteLine("4. vie all locations");
    Console.WriteLine("5. see pending patient request");
    Console.WriteLine("6. see user permissions");
    Console.WriteLine("7. view my schedule");
    Console.WriteLine("8. view my region");
    Console.WriteLine("9. logout");

    switch(Utils.GetIntegerInput("Choice: "))
    {
        case 1:
            Console.WriteLine("Create account for personnel or admin");
        if (active_user.HasPermission(Permissions.AddPersonnel))
            {
                Console.WriteLine("1. Create account for personnel"); 
            }
            if (active_user.HasPermission(Permissions.AddAdmin))
            {
                Console.WriteLine("2. Create account for admin"); 
            }
            Console.WriteLine("3. Return");
            switch (Utils.GetIntegerInput("Choose a number: "))
            {
                case 1:
                    if (!active_user.HasPermission(Permissions.AddPersonnel)) 
                    {
                        Utils.DisplayAlertText("You can not do that"); 
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Create new personnel account"); 
                        string newUser = Utils.GetRequiredInput("enter username: ");
                        string newPass = Utils.GetRequiredInput("enter password: ");

                        users.Add(new User(Utils.GetIndexAddOne(users), newUser, newPass, Role.Personnel)); 
                        User? UserLastCreated = users.Last(); 
                        int chooseRole = Utils.GetIntegerInput("Pick role for the personnel: 1.Doctor, 2.Nurse, 3.Administrator. (Choose a number): "); 
                        string doctorDetails = "";
                        switch (chooseRole)
                        {
                            case 1:
                                doctorDetails = Utils.GetRequiredInput("enter doctor's department: "); 
                                break;
                            case 2:
                            case 3:
                                break;
                        }
                        UserLastCreated.SetRolePersonel(chooseRole, UserLastCreated, doctorDetails);
                        FileHandler.SaveUsersToCsv(users);
                        Utils.DisplaySuccessText($"New personnel account for {newUser} created ");
                    }
                    break;
                case 2:
                    if (!active_user.HasPermission(Permissions.AddAdmin))
                    {
                        Utils.DisplayAlertText("You can not do that"); 
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Create new personnel account");
                        string newUser = Utils.GetRequiredInput("enter username: ");
                        string newPass = Utils.GetRequiredInput("enter password: ");
                        users.Add(new User(Utils.GetIndexAddOne(users), newUser, newPass, Role.Admin));
                        FileHandler.SaveUsersToCsv(users);
                        Utils.DisplaySuccessText($"New admin account for {newUser} created ");

                    }
                    break;
                case 3:
                    break;

            }

            break;
        case 2:
            Console.WriteLine("\nAll users:");
            foreach (var user in users) 
            {
                Console.WriteLine($"Username: {user.Username} - Role: {user.GetRole()}"); 
            }
            break;
        case 3:

            if (active_user.GetRole() == Role.Admin && active_user.HasPermission(Permissions.AddLocation)) 
            {
                Console.WriteLine("nter the region of the location you wish to add: "); 
                string region = Console.ReadLine() ?? "".Trim();

                Console.WriteLine("enter the name of the hospital you wish to add: "); 
                string hospital = Console.ReadLine() ?? "".Trim();

                locations.Add(new Location(region, hospital)); 
            }
            else
            {
                Utils.DisplayAlertText("Access denied. Contact superadmin for permission"); 
            }


            break;

        case 4:
            Console.WriteLine("All locations currently in the system:\n");
            foreach (var location in locations) 
            {
                Console.WriteLine(location.ToString()); 
            }
            break;
        case 5:

            if (active_user.GetRole() == Role.Admin && active_user.HasPermission(Permissions.AddRegistrations)) 
            {


                Console.WriteLine("\nAll patients with pending request:");
                foreach (User user in users.Where(user => user.GetRole() == Role.Patient && user.GetRegistration() == Registration.Pending)) 
                {
                    Console.WriteLine($"{user.ToString()}"); 
                }
                 
                string patientHandling = Utils.GetRequiredInput("Pick patient name you would like to handle:  ");
                User? patientUser = users.Find(user => user.Username.Equals(patientHandling, StringComparison.OrdinalIgnoreCase)); 
                if (patientUser != null)
                {
                    string acceptOrDeny = Utils.GetRequiredInput($"You picked: {patientUser.Username}. Would you like to accept(y) or deny(d) the request:  "); 
                    switch (acceptOrDeny)
                    {
                        case "y":
                            patientUser.AcceptPending(); 
                            Utils.DisplaySuccessText("Request accepted.");
                            break;

                        case "d":
                            patientUser.DenyPending();   
                            Utils.DisplaySuccessText("Request denied.");
                            break;
                        default:
                            Utils.DisplayAlertText("Only y or n is available"); 
                            break;
                    }
                }
                else
                {
                    Utils.DisplayAlertText("No patient by the name was found"); 
                }
            }
            else
            {
                Utils.DisplayAlertText("Access denied. Contact superadmin for permission");
            }
            break;
        case 6:
            if (active_user.GetRole() == Role.Admin && active_user.HasPermission(Permissions.ViewPermissions)) 
            {
                Console.WriteLine($"\nAll users:");
                foreach (var user in users) 
                {
                    Console.WriteLine($"{user.Username} - {user.GetRole()} - Permissions: {string.Join(", ", user.PermissionList)}"); 
                }
            }
            else
            {
                Utils.DisplayAlertText("Access denied. Contact superadmin for permission"); 
            }

            break;
        case 7:
            ViewSchedule(active_user); 
            break;

        case 8:
            Console.WriteLine("See my assigned region");
            bool found = false; 
            foreach (User user in users) 
            {
                if (user.GetRole() == Role.Admin) 
                {
                    Region? region = user.GetAssignedRegion(); 
                    if (region == null || region == Region.None) 
                    {
                        Console.WriteLine(user.Username + " has no region assigned."); 
                    }
                    else
                    {
                        Console.WriteLine(user.Username + " is assigned to region: " + region); 
                    }

                    found = true; 
                }
            }
            if (!found) 
            {
                Utils.DisplayAlertText("No admins found");
            }
            break;
        case 9:
            FileHandler.SaveUsersToCsv(users);
            Console.WriteLine("\n1. Write 'logout' to log out.");
            Console.WriteLine("2. Write 'return' to go back.");
            break;

        default:
            Console.WriteLine("Invalid input");
            break;
    }

}




// PERSONNEL MENU METHOD



void PersonnelMenu(List<User> users, User active_user, List<Appointment> appointments)

{
    ScheduleService scheduleService = new ScheduleService();

    Console.Clear();
    Console.WriteLine($"\n(Personnel) Menu - Logged in as {active_user.Username}");
    Console.WriteLine("1. Open assigned patient journal");
    Console.WriteLine("2. Modify patient appointment"); 
    Console.WriteLine("3. Accept/Deny patient appointment request");
    Console.WriteLine("4. View my schedule");
    Console.WriteLine("5. View patient journal");
    Console.WriteLine("6. Register appointments");
    Console.WriteLine("7. Logout");


    int input = Utils.GetIntegerInput("\nChoice: ");

    switch (input)
    {
        case 1:
            
            Personnel.OpenJournal(users, active_user);
            break;

        case 2:
            Personnel.ModifyAppointment(users, active_user);
            break;
        case 3: 
            Personnel.ApproveAppointments(users, active_user);
            break;
        case 4:
            ViewSchedule(active_user);
            break;
        
        case 5:
            {

                foreach (User user in users)
                {
                    if (user.GetRole() == Role.Patient)
                    {
                        Console.WriteLine(user.Username);
                    }
                }
                 
                string patientHandling = Utils.GetRequiredInput("Pick patient name you would like to handle:  ");
                User? patientUser = users.Find(user => user.Username.Equals(patientHandling, StringComparison.OrdinalIgnoreCase));
                if (patientUser != null)
                {
                    Console.WriteLine(patientUser);
                    Console.ReadLine();
                    Console.WriteLine("Press enter to continue");
                }


                else
                {
                    Utils.DisplayAlertText("No patient by the name was found");
                }
                break;

            }

        case 6:
            {
                
                foreach (User user in users)

                {
                    Console.WriteLine(user);
                }

                string patientHandling = Utils.GetRequiredInput("Pick patient name you would like to handle: ");
                
                User? patientUser = users.Find(user => user.Username.Equals(patientHandling, StringComparison.OrdinalIgnoreCase));
                
                if (patientUser != null)
                {

                    string department = Utils.GetRequiredInput("Department / location");
                    string type = Utils.GetRequiredInput("Type of appointment (checkup, operations)");
                    string dateInput = Utils.GetRequiredInput("Date and time, format (yyyy-MM-dd HH:mm):");

                    if (!DateTime.TryParseExact(dateInput, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime appointmentDate))
                    
                    {
                        Utils.DisplayAlertText("Invalid date format. Please use yyyy-MM-dd HH:mm");
                        Console.ReadKey();
                        break;
                    }

                    Appointment newAppointment = new Appointment(patientUser.Id, appointmentDate, "", department, type);

                    scheduleService.SaveAppointment(newAppointment);

                    Utils.DisplaySuccessText($"Appointment with {patientUser.Username} on {appointmentDate:yyyy-MM-dd HH:mm} has been booked.");
                    Console.ReadKey();
                    break;
                }
            }
            break;
        case 7:
            Console.WriteLine("\n1. Write 'logout' to log out.");
            Console.WriteLine("2. Write 'return' to go back.");
            break;
        default:
            Utils.DisplayAlertText("Invalid option.");
            break;
    }

}


//PATIENT MENU METHOD

void PatientMenu(User active_user, List<User> doctorsList, List<User> users)
{
    
    ScheduleService scheduleService = new ScheduleService();

    Console.Clear();
    Console.WriteLine("\n(Patient) Menu Choices:");
    Console.WriteLine("1. See Journal");
    Console.WriteLine("2. Book appointment");
    Console.WriteLine("3. See my appointments");
    Console.WriteLine("4. Cancel appointment");
    Console.WriteLine("5. Request a doctor");
    Console.WriteLine("6. View my doctors");
    Console.WriteLine("7. View my schedule");
    Console.WriteLine("8. Logout");

    int input = Utils.GetIntegerInput("\nChoice: ");

    switch (input)
    {
        case 1:
            Console.Clear();
            Console.WriteLine($"--- Patient Journal for {activeUser.Username} ---\n");

             
            var journalService = new JournalService();

            
            var entries = journalService.GetJournalEntries(active_user.Id);

            
            if (entries.Count == 0)
            {
                Console.WriteLine("(No journal entries found)");
            }
            else
            {
                Console.WriteLine("Your Journal Entries: \n");
                foreach (var entry in entries.OrderBy(entry => entry.CreatedAt))
                {
                    Console.WriteLine(entry.Format());
                }
            }
            Console.WriteLine("\nPress any key to return to menu...");
            break;

        
        case 2:
            Console.WriteLine("\n--- Create New Appointment ---");
            Console.WriteLine("All doctors:  ");
            foreach (User user in doctorsList)
            {
                Console.WriteLine(user.ToPersonnelDisplay()); 
            }
            string doctor = Utils.GetRequiredInput("Pick a doctor for your appointment: "); 
            string department = Utils.GetRequiredInput("Department / Location: ");
            string type = Utils.GetRequiredInput("Type of appointment (checkup, operation): ");
            string dateInput = Utils.GetRequiredInput("Date and time (format: yyyy-MM-dd HH:mm): ");

            
            if (!DateTime.TryParseExact(dateInput, "yyyy-MM-dd HH:mm", null,
                System.Globalization.DateTimeStyles.None, out DateTime appointmentDate)) 
            {
                Utils.DisplayAlertText("Invalid date format. Please use full yyyy-MM-dd HH:mm");
                Console.ReadKey();
                break;
            }

            Appointment newAppointment = new Appointment(active_user.Id, appointmentDate, doctor, department, type);
            scheduleService.SaveAppointment(newAppointment);

            Utils.DisplaySuccessText($"Appointment with {doctor} on {appointmentDate:yyyy-MM-dd HH:mm} has been booked.");
            Console.ReadLine();
            break;

        
        //View all appointments
        
        case 3:
            Console.WriteLine("\n--- Your Appointments ---");

           
            Schedule mySchedule = scheduleService.LoadSchedule(active_user.Id);

            if (mySchedule.Appointments.Count == 0)
            {
                Utils.DisplayAlertText("You have no upcoming appointments.");
            }
            else
            {
                mySchedule.PrintSchedule();
            }

            Console.WriteLine("\nPress ENTER to return to menu...");
            break;

        
        //Cancel an existing appointment
        
        case 4:
            Console.WriteLine("\n--- Cancel Appointment ---");

            Schedule cancelSchedule = scheduleService.LoadSchedule(active_user.Id);
            if (cancelSchedule.Appointments.Count == 0)
            {
                Utils.DisplayAlertText("Can not cancel, no appointments booked yet..."); 
                Console.ReadKey();
                break;
            }

            cancelSchedule.PrintSchedule(); 

            string cancelInput = Utils.GetRequiredInput("\nEnter the exact date and time of the appointment to cancel (yyyy-MM-dd HH:mm): ");


            if (!DateTime.TryParseExact(cancelInput, "yyyy-MM-dd HH:mm", null, 
                System.Globalization.DateTimeStyles.None, out DateTime cancelDate))
            {
                Utils.DisplayAlertText("Invalid date format.");
                Console.ReadKey();
                break;
            }

            
            scheduleService.RemoveAppointment(active_user.Id, cancelDate);
            Utils.DisplaySuccessText("Appointment canceled.");
            Console.ReadKey();
            break;
        
        case 5:
            Console.WriteLine("\n--- All Doctors to pick from ---");
            foreach (User user in doctorsList)
            {
                Console.WriteLine(user.ToPersonnelDisplay());
            }
            string doctorName = Utils.GetRequiredInput("Pick the name of the doctor: ");

            
            User? doctorObject = doctorsList.Find(user => user.Username.Equals(doctorName, StringComparison.OrdinalIgnoreCase));

            if (doctorObject != null)
            {
                bool success = active_user.AssignPersonnel(doctorObject.Id);
                if (success)
                {
                    Utils.DisplaySuccessText($"Personnel (ID: {doctorObject.Id}) assigned to patient: {active_user.Username}.");
                }
                else
                {
                    Utils.DisplayAlertText("Couldn't add personnel. The patient already has this ID, or it's the wrong role.");
                }
            }
            else
            {
                Utils.DisplayAlertText("Wrong spelling or no doctor by that name");
            }
            Console.WriteLine("\nPress ENTER to return...");
            break;
    }
}