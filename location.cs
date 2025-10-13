using System.Globalization;

namespace HealthCareSystem;

class Location
{
    string Region;
    string Adress;
    string Name;

    public Location(string region, string adress, string name)
    {
        Region = region;
        Adress = adress;
        Name = name;
    }
}



/*
Event = title, datetime start, datetime end, desc, list (vilka som är involverade), location, list(participant event), list(documents som ör relaterade till detta), 
kan skapa en journal som en klass.
gällande event kan man ha en type, vilka event där location och type är appointment och start som är idag, kunna se vilka som är inlvoverade, kan använda 
--- båda 

event participant = IUser, Role, 

*/