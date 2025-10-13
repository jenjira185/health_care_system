namespace HealthCareSystem;


// class for journal with keys
class journal
{
    public string Title;
    public string Descriptions;
    public DateTime dateCreated;
    public DateTime dateLast;

    //using ID so the patient journal can get access to correct personnel
    public int patientID;
    public int staffID;



    // A constructor to create a new journal
    public journal(string title, string desc, int patID, int inChargeID)
    {
        Title = title;
        Descriptions = desc;
        patientID = patID;
        staffID = inChargeID;

        //using datetime to know when the journal have been created
        dateCreated = DateTime.Now;
        dateLast = DateTime.Now;
    }


    // An new constructor for the journal to update
    public void UpdateJournal(string newDesc, int modifierStaffID)
    {
        Descriptions = newDesc;
        staffID = modifierStaffID;
        dateLast = DateTime.Now;   //using datetime for journal's last update of the patient
    }
}
