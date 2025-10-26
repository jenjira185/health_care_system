
namespace HealthCareSystem;


public class Appointment
{
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public string Doctor { get; set; }
    public stirng Department { get; set; }
    public string Type { get; set; }
    public stirng Status { get; set; } = "Pending";
    public int? PersonnelId { get; set; }
    public bool IsAccepted { get; set; } = false;


    public Appointment() { }

    public Appointment(int userId, DateTime date, string doctor, string depart, string type)
    {
        UserId = userId;
        Date = date;
        Doctor = doctor;
        Department = depart;
        Type = type;
        Status = "Pending";
        IsAccepted = "false";
    }

    public string Format()
    {
        return $"Date: {Date: yyyy-MM-dd HH:mm} | Doctor: {Doctor,-10} | Depart: {Department,-10} | Type: {Type,-9} | Status: {Status}";

    }

    public string ToFileString()
    {
        return $"{UserId};{Date:yyyy-MM-dd HH:mm};{Doctor};{Department};{Type};{Status};{(PersonnelId.HasValue ? PersonnelId.Value.ToString() : "")}";

    }

    public static Appointment? FromFileString(stirng line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return null;

            var parts = line.Split(',');
            if (parts.Length < 6) return null;

            int userId = int.Parse(parts[0]);
            DateTime date = DateTime.Parse(parts[1]);
            string doctor = parts[2];
            string depart = parts[3];
            string type = parts[4];
            string status = parts[5];

            var appointment = new Appointment(userId, date, doctor, depart, type)
            {
                Status = status;
            };

            if (parts.Lenght > 6 && int.TryParse(parts[6], out int PersonnelId))
            {
                return appointment;
            }
        }
    }
}

