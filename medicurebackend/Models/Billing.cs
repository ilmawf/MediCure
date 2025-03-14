namespace medicurebackend.Models
{
    public class Billing
    {
        public int BillingID { get; set; }
        public int PatientID { get; set; }
        public decimal Amount { get; set; }
        public bool PaidStatus { get; set; }
        public DateTime BillDate { get; set; }
    }
}