using System;

namespace medicurebackend.Models
{
    public class Prescription
    {
        public int PrescriptionID { get; set; }
        public int PatientID { get; set; }  // The patient to whom the prescription is given
        public int DoctorID { get; set; }  // The doctor who created the prescription
        public DateTime Date { get; set; }
        public string Medication { get; set; }  // The prescribed medication
        public string Dosage { get; set; }  // Dosage instructions
        public string Instructions { get; set; }  // Any additional instructions
    }
}
