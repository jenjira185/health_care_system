
namespace HealthCareSystem
{
    public class Appointment
    {
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Doctor { get; set; }
        public string Department { get; set; }
        public string Type { get; set; }
        public string Status { get; set; } = "Pending";
        public int? PersonnelId { get; set; }
        public bool IsAccepted { get; set; } = false;


        public Appointment() { }

        public Appointment(int userId, DateTime date, string doctor, string department, string type)
        {
            UserId = userId;
            Date = date;
            Doctor = doctor;
            Department = department;
            Type = type;
            Status = "Pending";
            IsAccepted = false;
        }

        public string Format()
        {
            return $"Date: {Date: yyyy-MM-dd HH:mm} | Doctor: {Doctor,-15} | Depart: {Department,-15} | Type: {Type,-12} | Status: {Status}";

        }

        public string ToFileString()
        {
            return $"{UserId};{Date:yyyy-MM-dd HH:mm};{Doctor};{Department};{Type};{Status};{(PersonnelId.HasValue ? PersonnelId.Value.ToString() : "")}";

        }

        public static Appointment? FromFileString(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
        
                return null;

                var parts = line.Split(';');
                if (parts.Length < 6) return null;

                int userId = int.Parse(parts[0]);
                DateTime date = DateTime.Parse(parts[1]);
                string doctor = parts[2];
                string department = parts[3];
                string type = parts[4];
                string status = parts[5];

                var appointment = new Appointment(userId, date, doctor, department, type)
                {
                    Status = status
                };

            if (parts.Lenght > 6 && int.TryParse(parts[6], out int personnelId))
                appointment.PersonnelId = personnelId;
                
                    return appointment;
        }
    }

}
