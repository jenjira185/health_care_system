namespace HealthCareSystem;

class Location
{
    public int locationID;
    public string Region;
    public string Adress;
    public string Name;

    public List<string> schedule();

    public Location(string region, string adress, string name)
    {
        locationID = ID;
        Region = region;
        Adress = adress;
        Name = name;
        schedule = new List<string>();
    }
}
