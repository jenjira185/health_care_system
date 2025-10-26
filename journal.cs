
namespace HealthCareSystem   //JournalService
{
    public class JournalService
    {
        private readonly string _journalFile = "Data/journal.txt";

        public JournalService()
        {
            EnsureDataDirectoryExists();
        }

        public List<JournalEntry> GetJournalEntries(int patientId)
        {
            var entries = new List<JournalEntry>();
            if (!File.Exists(journalFile)) return entries;

            var lines = File.ReadAllLines(journalFile);
            foreach (var line in lines)
            {
                var entry = JournalEntry.FromFileString(line);
                if (entry != null && entry.PatientId == patientId)
                {
                    entries.Add(entry);
                }
            }

            return entries;
        }

        public void AddEntry(int patientId, string authorName, string entryText)
        {
            var newEntry = new JournalEntry
            {
                PatientId = patientId,
                authorName = authorName,
                entryText = entryText,
                CreateAt = DateTime.Now
            };

            File.AppendAllLines(journalFile, new[] { newEntry.ToFileString() });
        }

        private void EnsureDataDirectoryExists()
        {
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");
        }
    }

    public class JournalEntry
    {
        public int PatientId { get; set; }
        public DateTime CreateAt { get; set; }
        public string AuthorName { get; set; }
        public string EntryText { get; set; }

        public string Format()
        {
            return $"[{CreateAt:yyyy-MM-dd HH:mm}] {AuthorName}: {EntryText}";
        }

        public string ToFileString()
        {
            return $"{PatientId}; {CreateAt:yyyy-MM-dd HH:mm};{AuthorName};{EntryText}";
        }

        public static JournalEntry? FromFileString(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return null;

            var parts = line.Split(';');
            if (parts.Length < 4) return null;

            if (!int.TryParse(parts[0], out int patientId)) return null;
            if (!DateTime.TryParse(parts[1], out DateTime createAt)) return null;

            return new JournalEntry
            {
                PatientId = patientId,
                CreateAt = createAt,
                AuthorName = parts[2],
                entryText = parts[3]
            };
        }
    }
}
