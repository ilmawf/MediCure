using System;

namespace medicurebackend.Models
{
    public class Billing
    {
        public int BillingID { get; set; }
        public int PatientID { get; set; }
        public int AppointmentID { get; set; }
        public decimal Amount { get; set; }
        public DateTime BillingDate { get; set; }
        public string Status { get; set; } // E.g., Paid, Pending
    }
}
