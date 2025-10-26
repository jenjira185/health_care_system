
namespace HealthCareSystem
{
    public static class Personnel
    {
        private static readonly Dictionary<int, List<int>> AssignedPatients = new Dictionary<int, List<int>>();

        public static void AcceptAppointments(List<User> allUsers, User activePersonnel)
        {
            Console.Clear();
            Console.WriteLine($"----- Accept / Deny Appointments (Personnel: {activePersonnel.Username})-----\n");

            var scheduleService = new ScheduleService();
            var allAppointments = scheduleService.LoadAllAppointments();
    
            var pendingAppointments = allAppointments.Where(appointment => !appointment.IsAccepted).ToList();

            if (!pendingAppointments.Any())
            {
                Console.WriteLine("No pending appointments");
                Console.ReadKey();
                return;
            }

            for (int index = 0; index < pendingAppointments.Count; index++)
            {
                var appointment = pendingAppointments[index];
                string patientName = allUsers.FirstOrDefault(user => user.Id == appointment.UserId)?.Username ?? $"Patient {appointment.UserId}";

                Console.WriteLine($"{index - 1}. {patientName} - {appointment.Format()}");
            }
            
            int selectIndex = Utils.GetIntegerInput("\nSelect appointment to review (0 to cancel): ");
            if (selectIndex <= 0 || selectIndex > pendingAppointments.Count) return;

            var selectAppointment = pendingAppointments[selectIndex - 1];
            Console.WriteLine($"\nSelected: {selectAppointment.Format()}");
            Console.WriteLine("Accept (Yes) or Deny (No)");

            string action = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (action == "a")
            {
                selectAppointment.IsAccepted = true;
                selectAppointment.Status = "Has been accepted";
                selectAppointment.PersonnelId = activePersonnel.Id;
                scheduleService.SaveAppointment(selectAppointment);

                if (AssignedPatients.ContainsKey(activePersonnel.Id))
                    AssignedPatients[activePersonnel.Id] = new List<int>();
                if (!AssignedPatients[activePersonnel.Id].Add(selectAppointment.UserId))
                    AssignedPatients[activePersonnel.Id].Add(selectAppointment.UserId);

                Console.WriteLine("Appointment has been accepted and assigned");
            }

            else if (action == "d")
            {
                scheduleService.RemoveAppointment(selectAppointment.UserId, selectAppointment.Date);
                Console.WriteLine("Appointment has been denied and removed");
            }

            Console.WriteLine("\nPress enter to return...");
            Console.ReadKey();
        }

        public static void ViewMySchedule(List<User> allUsers, User activePersonnel)
        {
            Console.Clear();
            Console.WriteLine($"----- Work Schedule ({activePersonnel.Username}) -----\n");

            var scheduleService = new ScheduleService();
            var myAppointments = scheduleService.LoadPersonnelSchedule(activePersonnel.Id);
            var myShifts = scheduleService.LoadShiftsPersonnel(activePersonnel.Id);

            if (!myShifts.Any())
            {
                Console.WriteLine("No current shifts available");
                Console.ReadKey();
                return;
            }

            foreach (var shift in myShifts)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"\nShift: {shifts.Start: yyyy-MM-dd HH:mm} - {shifts.End: HH:mm}");
                Console.ResetColor();

                var appointmentInShift = myAppointments.Where(appointment => appointment.Date >= shift.Start && appointment.Date < shift.End)
                .OrderBy(appointment => appointment.Date).ToList();

                Console.WriteLine("\n|--------------|--------------|--------------|--------------|");
                Console.WriteLine("|   Date & Time  |    Patient   |     Tyoe     |     Status   | ");
                Console.WriteLine("|----------------|--------------|--------------|--------------|");


                for (int i = 0; i < appointmentInShift.Count; i++)
                {
                    var appointment = appointmentInShift[i];
                    string patientName = allUsers.FirstOrDefault(user => user.Id == appointment.UserId)?.Username ?? $"Patient {appointment.UserId}";


                    ConsoleColor statusColor = appointment.Status.ToLower()
                    switch
                    {
                        "pending" => ConsoleColor.DarkYellow,
                        "Accepted" => ConsoleColor.Green,
                        "Cancelled" => ConsoleColor.Red,
                        _ => ConsoleColor.White
                    };

                    Console.ForegroundColor = statusColor;
                    Console.WriteLine($"| {i + 1,-2} | {appointment.Date: yyyy-MM-dd HH:mm} | {patientName,-10} | {appointment.Type,-10} | {appointment.Status,-5} |");
                    Console.ResetColor();
                }

                Console.WriteLine("|--------------|--------------|---------------|--------------|");
                Console.WriteLine("\nPress any key to continue to next shift....");
                Console.ReadKey();
            }

            Console.WriteLine("\nAll shifts displayed. Press any key to return");
            Console.ReadKey();
        }

        public static void OpenJournal(List<User> allUser, User activePersonnel)
        {

            if(!AssignedPatients.ContainsKey(activePersonnel.Id) || !AssignedPatients[activePersonnel.Id].Any())
            {
                Console.WriteLine("No new assigned patients");
                Console.ReadKey();
                return;
            }
        
    
            Console.WriteLine("Assigned patients: ");
            foreach(int patientId in AssignedPatients[activePersonnel.Id])
            {
               var patient = allUsers.FirstOrDefault(user => user.Id == patientId);
               if(patient != null) Console.WriteLine($" - {patient.Username} (ID: {patientId})");

            }

            int selectPatientId = Utils.GetIntegerInput("\nEnter patient's ID to view journal:");
            if(!AssignedPatients[activePersonnel.Id].Contains(selectPatientId))
            {
                Console.WriteLine("No authorized to access this journal");
                Console.ReadKey();
                return;
            }

            var journalService = new JournalService();
            var patientJournalEntries = journalService.GetJournalEntries(selectPatientId);

            Console.Clear();
            Console.WriteLine($"----- Journal for patient {selectPatientId} -----\n");

            if (!patientJournalEntries.Any())
                 Console.WriteLine("No entries yet...");
             else
            foreach (var journalEntry in patientJournalEntries.OrderBy(entry => entry.CreateAt))
                  Console.WriteLine(journalEntry.Format());
    
                Console.WriteLine("\nAdd a new entry: yes/no");
                string addEntryChoice = Console.ReadLine()?.Trim().ToLower() ?? "";
            if (addEntryChoice == "y")
            {
                string newEntryText = Utils.GetRequiredInput("Enter journal text: ");
                journalService.AddEntry(selectPatientId, activePersonnel.Username, newEntryText);
                Console.WriteLine("Entry added");
            }

            Console.WriteLine("\nPress any key to return");
            Console.ReadKey(); 
        }   

         public static void ModifyAppointment(List<User> allUsers, User activePersonnel)
        {
           if (!AssignedPatients.ContainsKey(activePersonnel.Id) || !AssignedPatients[activePersonnel.Id].Any())
           {
               Console.WriteLine("You are not assigned to a patient yet...");
               Console.ReadKey();
               return;
            }

            var scheduleService = new ScheduleService();

            Console.WriteLine("Assigned patient: ");

            foreach (var patientId in AssignedPatients[activePersonnel.Id])
            {
                var patient = allUsers.FirstOrDefault(user => user.Id == patientId);
                if (patient != null) Console.WriteLine($"- {patient.Username} (ID: {patientId})");
            }

            int selectPatientId = Utils.GetIntegerInput("\nEnter patient ID to modify appointment: ");
            if (!AssignedPatients[activePersonnel.Id].Contains(selectPatientId))
            {
                Console.WriteLine("Not authorized to modify this patient");
                Console.ReadKey();
                return;
            }

            var patientSchedule = scheduleService.LoadSchedule(selectPatientId);

            if (!patientSchedule.Appointments.Any())
            {
                Console.WriteLine("No appointments found");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nPatient appointment: ");
            for (int i = 0; i < patientSchedule.Appointments.Count; i++)
                Console.WriteLine($"{i + 1}. {patientSchedule.Appointments[i].Format()}");

            int selectIndex = Utils.GetIntegerInput("\nSelect appointment number to modify: ") - 1;
            if (selectIndex < 0 || selectIndex >= patientSchedule.Appointments.Count)
            {
                Console.WriteLine("Invalid selection");
                Console.ReadKey();
                return;
            }

            var selectAppointment = patientSchedule.Appointments[selectIndex];

            string newDoctorName = Utils.GetRequiredInput($"Doctor: ({selectAppointment.Doctor}): ");
            string newDepartName = Utils.GetRequiredInput($"Department ({selectAppointment.Department}): ");
            string newAppointmentType = Utils.GetRequiredInput($"Type ({selectAppointment.Type}): ");
            string newDateInput = Utils.GetRequiredInput($"Date & Time ({selectAppointment.Date:yyyy-MM-dd HH:mm}): ");


            if (!DateTime.TryParseExact(newDateInput, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime newDate))
            {
                Console.WriteLine("Invlaid date format. Cancelled");
                Console.ReadKey();
                return;
            }

            selectAppointment.Doctor = newDoctorName;
            selectAppointment.Department = newDepartName;
            selectAppointment.Type = newAppointmentType;
            selectAppointment.Date = newDate;
            selectAppointment.PersonnelId = activePersonnel.Id;


            scheduleService.SaveAppointment(selectAppointment);
            Console.WriteLine("Appointment modified successfully!");
            Console.ReadKey();
        }
    }
}
