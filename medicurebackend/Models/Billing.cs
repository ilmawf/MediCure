namespace medicurebackend.Models
{
    public class Billing
    {
        public int BillingID { get; set; }
        public int PatientID { get; set; }
        public decimal Amount { get; set; }

       
        public DateTime Date { get; set; }     // Billing date
        public string? Status { get; set; }     // Billing status (Paid/Unpaid, etc.)

       
        public Patient? Patient { get; set; }
    }
}
