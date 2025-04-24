using System;

namespace medicurebackend.Models
{
    public class Alert
    {
        public int AlertID { get; set; }  // Unique identifier for the alert
        public string? Title { get; set; } // Title of the alert (e.g., "Low Stock")
        public string? Message { get; set; } // Message explaining the alert
        public DateTime Date { get; set; }  // Date and time when the alert was created
        public bool IsRead { get; set; } // Whether the alert has been read by the user
    }
}
