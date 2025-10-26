namespace HealthCareSystem
{
    public class WorkShift
    {
        public int PersonnelId { get; set; }
        public string PersonnelName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Department { get; set; }


        public WorkShift(int personnelId, string personnelName, DateTime start, DateTime end, string depart)
        {
            PersonnelId = personnelId;
            PersonnelName = personnelName;
            Start = start;
            End = end;
            Department = depart;
        }

        public string Format()
        {
            return $"{PersonnelName,-15}, | {Department,-12} | {Start:yyyy-MM-dd HH:mm} - {End:HH:mm}";
        }
    }
}
