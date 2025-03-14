namespace medicurebackend.Models
{
    public class Medication
    {
        public int MedicationID { get; set; }
        public int PatientID { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public required string Frequency { get; set; }
    }
}