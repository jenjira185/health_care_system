namespace HealthCareSystem;

class Location
{
    public int locationID;
    public string Region;
    public string Adress;
    public string Name;
    public List<string> schedule();     //a string for schedule to be included in location

    public Location(string region, string adress, string name)
    {
        locationID = ID;
        Region = region;
        Adress = adress;
        Name = name;
        schedule = new List<string>();    //Initializing the schedule like an empty list
    }

    public int GetLocationID()      //Instead of using get; set; I use the method to return the value
    {
        return locationID;
    }
    public string GetName()
    {
        return Name;
    }

    public List<string> GetSchedule()     //This returns the whole schedule 
    {
        return schedule;      
    }

    public void AddToSchedule(string appointmentDetails)     //This method is used to add an appointment in the schedule
    {
        schedule.Add(appointmentDetails);
    }

    public void UpdateLocationDetails(string newRegion, string newAdress)     //This method lets the Admin to get an updated information
    {
        Region = newRegion;
        Adress = newAdress;
    }
}
