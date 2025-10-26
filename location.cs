using System;
using System.Collections.Generic;
namespace HealthCareSystem;

public class Location
{
    public int LocationID { get; set; }
    public string Region { get; set; }
    public string Adress { get; set; }
    public string hospitalName { get; set; }
    public List<string> Schedule { get; set; }     //a string for schedule to be included in location

    public Location(int id, string region, string adress, string name)
    {
        locationID = id;
        Region = region;
        Adress = adress;
        hospitalName = name;
        Schedule = new List<string>();    //Initializing the schedule like an empty list
            
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
