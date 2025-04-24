using Microsoft.AspNetCore.SignalR;

namespace medicurebackend.Hubs
{
    public class NotificationHub : Hub
    {
        // Send a general notification
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        // Specific notification for new patient registration
        public async Task SendPatientRegistrationNotification(string patientName)
        {
            var message = $"New patient registered: {patientName}";
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        // Specific notification for appointment reminder
        public async Task SendAppointmentReminder(string patientName, string appointmentDate)
        {
            var message = $"Reminder: {patientName} has an appointment on {appointmentDate}";
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        // Specific notification for appointment status update
        public async Task SendAppointmentStatusUpdate(string appointmentDetails)
        {
            var message = $"Appointment status updated: {appointmentDetails}";
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
