using System;
using System.Collections.Generic;
namespace HealthCareSystem;

class User
{
    public string Name { get; set; }
    public string Password { get; set; }
    public bool IsLoggedIn { get; set; }

    public User(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public virtual void Login(string password)
    {
        if (password == Password)
        {
            IsLoggedIn = true;
            Console.WriteLine($"{Name} logged in succesfully");
        }
        else
        {
            Console.WriteLine("Incorrect password");
        }
    }

    public virtual void Logout()
    {
        if (IsLoggedIn)
        {
            IsLoggedIn = false;
            Console.WriteLine($"{Name} logged out");
        }
        else
        {
            Console.WriteLine($"{Name} is not logged in");
        }
    }
    public virtual void ViewSchedule()
    {
        Console.WriteLine("No schedule available.");
    }
}
