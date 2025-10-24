namespace HealthCareSystem;

class Admin : User
{
    public List<string> Permissions { get; set; }
    public List<string> Locations { get; set; }
    public List<Personnel> PersonnelAccounts { get; set; }
    public List<string> Schedule { get; set; }

    public Admin(string name, string password)
    : base(name, password)
    {
        Permissions = new List<string>();
        Locations = new List<string>();
        PersonnelAccounts = new List<Personnel>();
        Schedule = new List<string>();
    }

    public void HandlePermissions(string permissions)
    {
        if (IsLoggedIn)
        {
            Permissions.Add(permission);
            Console.WriteLine($"Added permission: {permission}");
        }
        else
        {
            Console.WriteLine("Please log in first");
        }
    }

    public void AddLocation(string location)
    {
        if (IsLoggedIn)
        {
            Locations.Add(location);
            Console.WriteLine($"Added new location: {location}");
        }
        else
        {
            Console.WriteLine("Please log in first");
        }
    }

    public void CreatePersonnelAccount(string name, string password, string location)
    {
        if (IsLoggedIn)
        {
            Personnel newPersonnel = new Personnel(name, password, location);
            PersonnelAccounts.Add(newPersonnel);
            Console.WriteLine($"Created personnel account: {name} {location}");
        }
        else
        {
            Console.WriteLine("Please log in first");
        }
    }

    public void ViewPermissionsList()
    {
        Console.WriteLine("Permissions list: ");
        if (Permissions.Count == 0)
            Console.WriteLine("No permissions assigned");
        else
            for (int i = 0; i < Permissions.Count; i++)
                Console.WriteLine($"- {Permissions[i]}");
    }

    public override void ViewSchedule()
    {
        Console.WriteLine($"Admin schedule for {Name}: ");
        if (Schedule.Count == 0)
            Console.WriteLine("No scheduled items");
        else
            for (int i = 0; i < Schedule.Count; i++)
                Console.WriteLine($"- {Schedule[i]}");
    }
}
