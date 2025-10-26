namespace HelathcareSystem
{
    public class Schedule
    {
        public int UserId { get; set; }
        public List<Appointment> Appointments { get; set; }

        public Schedule(int userId)
        {
            UserId = userId;
            Appointments = new List<Appointment>();
        }

        public void AddAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                Console.WriteLine("Added a null appointment - it is now ignored");
                return;
            }

            if (appointment.UserId != UserId)
            {
                Console.WriteLine($"Appointmnet does not belong to user {UserId} - IGNORED");
                return;
            }

            Appointments.Add(appointment);
        }

        public List<Appointment> GetAppointmentsForWeek(DateTime weekStart)
        {
            DateTime weekEnd = weekStart.AddDays(7);

            var weekAppointments = Appointments.Where(appointment => appointment.Date >= weekStart && appointment.Date < weekEnd)
            .OrderBy(appointment => appointment.Date).ToList();
            return weekAppointments;
        }

        public void PrintSchedule()
        {
            Console.WriteLine($"\n---- Schedule for user {UserId} ----");

            if (!Appointments.Any())
            {
                Console.WriteLine("No appointments found");
                return;
            }
            
            foreach(var appointment in Appointments.OrderBy(appointment => appointment.Date))
            {
                Console.WriteLine(appointment.Format());
            }
        }
    }
}