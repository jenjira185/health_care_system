
namespace HealthCareSystem
{
    class Location
    {
        public string Region { get; set; }
        public string HospitalName { get; set; }
        public List<User> adminLocation { get; private set; }     //a string for schedule to be included in location

        public Location(string region, string hostiName)
        {
           Region = region;
           HospitalName = hostiName;
        }

        public override string ToString()
        {
           return $"{HospitalName} {Region}";
        }
    }
}


