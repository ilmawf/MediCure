public class PrescriptionRequest
{
    public int DoctorID { get; set; }
    public string PrescriptionDetails { get; set; } = string.Empty;  
    public string Medication { get; set; } = string.Empty;  
    public string Dosage { get; set; } = string.Empty;  
    
}
