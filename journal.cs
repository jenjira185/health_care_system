using System;
using System.Collections.Generic;
namespace HealthCareSystem;


// class for journal with keys
class Journal
{
    public string Name;
    public string Descriptions;
    public User Owner;


    // A constructor to create a new journal
    public Journal(string name, string desc, User owner)
    {
        Name = name;
        Descriptions = desc;
        Owner = owner;
    }

    public string Info()
    {
        return $"{Name} - by: {Owner.Email}";
    }

    public string ToSaveString()
    {
        return $"{Name}, {Descriptions}, {Owner.Email}";
    }
}
