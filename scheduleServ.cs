
namespace HealthCareSystem
{
    public class ScheduleService
    {
        private readonly string appointmentsFile = "Data/appointments.txt";
        private readonly string shiftsFile = "Data/shifts.txt";

        public Schedule LoadSchedule(int userId)
        {
            EnsureDataDirectoryExist();
            var schedule = new Schedule(userId);

            if (!File.Exists(appointmentsFile))
                return schedule;

            var lines = File.ReadAllLines(appointmentsFile);

            foreach (var line in lines)
            {
                var appointment = appointment.FromFileString(line);
                if (appointment != null && appointment.UserId == userId)
                    schedule.AddAppointment(appointment);
            }
            return schedule;
        }

        public void SaveAppointment(Appointment appointment)
        {
            EnsureDataDirectoryExists();

            var appointmentLines = File.Exists(appointmentsFile)
            ? File.ReadAllLines(appointmentsFile).ToList() : new List<string>();

            appointmentLines.RemoveAll(existingLine =>
            {
                var existingAppointment = Appointment.FromFileString(existingLine);
                return existingAppointment != null && existingAppointment.UserId == appointment.UserId && existingAppointment.Date == appointment.Date;
            });
            appointmentLines.Add(appointment.ToFileString());
            File.WriteAllLines(appointmentsFile, appointmentLines);
        }

        public void RemoveAppointment(int userId, DateTime date)
        {
          if (!File.Exists(_appointmentsFile))
           return;

          var appointmentLines = File.ReadAllLines(_appointmentsFile).ToList();

      
          appointmentLines.RemoveAll(line =>
          {
            var appointment = Appointment.FromFileString(line);
            return appointment != null &&
                 appointment.UserId == userId &&
                 appointment.Date == date;
          });

          File.WriteAllLines(_appointmentsFile, appointmentLines);
        }
        public List<Appointment> LoadAllAppointments()
        {
            if (!File.Exists(appointmentsFile))
                return new List<Appointment>();

            return File.ReadAllLines(appointmentsFile).Select(Appointment.FromFileString).Where(a => a != null).Cast<Appointment>().ToList();
        }

        public List<Appointment> LoadPersonnelSchedule(int personnelId)
        {
            return LoadAllAppointments().Where(a => a.PersonnelId == personnelId).OrderBy(a => a.Date).ToList();
        }

        public List<Shift> LoadAllShifts()
        {
            EnsureDateDirectoryExists();

            if (!File.Exists(shiftsFile))
                return new List<Shift>();

            return File.ReadAllLines(shiftsFile).Select(Shift.FromFileString).Where(b => b != null).Cast<Shift>().ToList();
        }

        public List<Shift> LoadShiftsForPersonnel(int personnelId)
        {
            return LoadAllShifts().Where(b => b.PersonnelId == personnelId).OrderBy(b => b.Start).ToList();
        }

        public void SaveShift(Shift shift)
        {
            EnsureDateDirectoryExists();

            var shiftLines = File.Exists(shiftsFile) ? File.ReadAllLines(shiftsFile).ToList() : new List<string>();

            shiftLines.Add(shift.ToFileString());
            File.WriteAllLines(shiftsFile, shiftLines);
        }

        public List<Shift> GetColleaguesShift(Shift currentShift)
        {
            return LoadAllShifts().Where(otherShift =>
            otherShift.PersonnelId != currentShift.PersonnelId &&
            (
                (otherShift.Start >= currentShift.Start && otherShift.Start < currentShift.End) ||
                (otherShift.End < currentShift.Start && otherShift.End <= currentShift.End) ||
                (otherShift.Start <= currentShift.Start && otherShift.End >= currentShift.End)

            ))
            .OrderBy(b => b.Start).ToList();
        }

        private void EnsureDataDirectoryExist()
        {
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");
        }


        public class Shift
        {
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public int PersonnelId { get; set; }

            public Shift() { }

            public Shift(DateTime start, DateTime end, int personnelId)
            {
                Start = start;
                End = end;
                PersonnelId = personnelId;
            }

            public string ToFileString()
            {
                return $"{Start:yyyy-MM-dd HH:mm};{End:yyyy-MM-dd HH:mm};{PersonnelId}";
            }

            public static Shift? FromFileString(string line)
            {
                if (string.IsNullOrWhiteSpace(line))
                    return null;

                var parts = line.Split(',');
                if (parts.Length < 3)
                    return null;

                if (!DateTime.TryParse(parts[0], out DateTime start))
                    return null;

                if (!DateTime.TryParse(parts[1], out DateTime end))
                    return null;
                if (!int.TryParse(parts[2], out int personnelId))
                    return null;

                return new Shift(start, end, personnelId);
            }
        }
    }
}
