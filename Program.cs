//As a user = Log in/out
//As a user = request registration as a patient

//As an Admin with sufficent permissions = give admins the permission to handle --
//-- the permission system 
//As an Admin with sufficent permissions = give admins the permission to create accounts for personnel
//As an Admin with sufficent permissions = give Admins the permission to view a list of who has permission to what
//As an Admin with sufficent permissions = add locations
//As an Admin with sufficent permissions = accept user registration as patients
//As an Admin with sufficent permissions = deny user registration as patient
//As an Admin with sufficent permissions = create accounts for personnel
//As an Admin with sufficent permissions = view a list of who has permission to what

//As an Personnel with sufficent permissions = view a patients journal entries
//As an Personnel with sufficent permissions = mark journal entries with diff levels of read permissions
//As an Personnel with sufficent permissions = register appointments
//As an Personnel with sufficent permissions = modify appointments
//As an Personnel with sufficent permissions = approve appointment request
//As an Personnel with sufficent permissions = view the schedule of a location

//As a patient = view my own journals
//As a patient = request an appointment 
//As a patient = view my schedule



using HealthCareSystem;

class Program
{
    static void Main(string[] args)
    {
        Patient patient = new Patient("Alice", "1234", "Has mild allergy.");
        Personnel doctor = new Personnel("Dr. Bob", "pass", "Clinic A");
        Admin admin = new Admin("SuperAdmin", "admin123");

        //-------Admin's actions------
        admin.Login("admin123");
        admin.AddLocation("Clinic A");
        admin.HandlePermissions("ModifyAppointments");
        admin.CreatePersonnelAccount("Dr. Bob", "pass", "Clinic A");
        admin.ViewPermissionsList();
        admin.Logout();


        //-----Patient actions-----
        patient.Login("1234");
        patient.ViewJournal();
        patient.RequestAppointment("2025-10-20");
        patient.ViewSchedule();
        patient.Logout();


        //----Personnel actions------
        doctor.Login("pass");
        doctor.Patient.Add(patient);
        doctor.RegisterAppointment(patient, "2025-10-20");
        doctor.ViewPatientJournals();
        doctor.ModifyAppointment(patient, "2025-10-20", "2025-10-22");
        doctor.ApproveAppointment("2025-10-22");
        doctor.ViewLocationSchedule();
        doctor.ViewSchedule();
        doctor.Logout();

        Console.WriteLine("\n--- End of the system demo ---");
    }
}
