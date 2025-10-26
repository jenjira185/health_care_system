
namespace HealthCareSystem;


// class for journal with keys
class Journal
{
    public string Name;
    public string Descriptions;
    public User Owner;


    // A constructor to create a new journal
    public Journal(string name, string descriptions, User owner)
    {
        Name = name;
        Descriptions = descriptions;
        Owner = owner;
    }

    public string Info()
    {
        return $"{Name} - by: {Owner.Email}";
    }

    public string ToSaveString()
    {
        return $"{Name}, {Description}, {Owner.Email}";
    }
}
