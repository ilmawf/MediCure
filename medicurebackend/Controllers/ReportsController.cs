using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using medicurebackend.Models;
using System.IO;
using System.Linq;

namespace medicurebackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly HospitalContext _context;

        public ReportsController(HospitalContext context)
        {
            _context = context;
        }

        // Export patient data to PDF
        [HttpGet("patient-report")]
        public IActionResult GetPatientReport()
        {
            // Create a MemoryStream to hold the PDF
            var pdf = new MemoryStream();
            var writer = new PdfWriter(pdf);
            var pdfDoc = new PdfDocument(writer);
            var document = new Document(pdfDoc);

            // Get patient data from the database
            var patients = _context.Patients.ToList();

            // Add a title to the document
            document.Add(new Paragraph("Patient Report").SetFontSize(20));

            // Loop through each patient and add their details to the PDF
            foreach (var patient in patients)
            {
                document.Add(new Paragraph($"Patient Name: {patient.Name}, Email: {patient.Email}, Phone: {patient.PhoneNumber}"));
            }

            // Close the document and return the file
            document.Close();
            return File(pdf.ToArray(), "application/pdf", "PatientReport.pdf");
        }

        [HttpGet("appointments-today")]
        public IActionResult GetAppointmentsTodayReport()
        {
            var today = DateTime.Today;
            var appointmentsToday = _context.Appointments
                .Where(a => a.AppointmentDate.Date == today)
                .ToList();

            var pdf = new MemoryStream();
            var writer = new PdfWriter(pdf);
            var pdfDoc = new PdfDocument(writer);
            var document = new Document(pdfDoc);

            // Add title for the report
            document.Add(new Paragraph("Appointments for Today").SetFontSize(20));

            // Loop through appointments and add them to the PDF
            foreach (var appointment in appointmentsToday)
            {
                document.Add(new Paragraph($"Patient: {appointment.Patient.Name}, Doctor: {appointment.Doctor.Name}, Appointment Time: {appointment.AppointmentDate}"));
            }

            document.Close();
            return File(pdf.ToArray(), "application/pdf", "AppointmentsToday.pdf");
        }


        [HttpGet("appointments-week")]
        public IActionResult GetAppointmentsWeekReport()
            {
                var startOfWeek = DateTime.Today.AddDays(-7);  // Start of the last 7 days
                var appointmentsWeek = _context.Appointments
                    .Where(a => a.AppointmentDate >= startOfWeek)
                    .ToList();

                var pdf = new MemoryStream();
                var writer = new PdfWriter(pdf);
                var pdfDoc = new PdfDocument(writer);
                var document = new Document(pdfDoc);

                // Add title for the report
                document.Add(new Paragraph("Appointments in the Last 7 Days").SetFontSize(20));

                // Loop through the appointments and add them to the PDF
                foreach (var appointment in appointmentsWeek)
                {
                    document.Add(new Paragraph($"Patient: {appointment.Patient.Name}, Doctor: {appointment.Doctor.Name}, Appointment Time: {appointment.AppointmentDate}"));
                }

                document.Close();
                return File(pdf.ToArray(), "application/pdf", "AppointmentsWeek.pdf");
            }

            [HttpGet("billing-report")]
            public IActionResult GetBillingReport()
            {
                var bills = _context.Billings.ToList();  // Fetch billing records

                var pdf = new MemoryStream();
                var writer = new PdfWriter(pdf);
                var pdfDoc = new PdfDocument(writer);
                var document = new Document(pdfDoc);

                // Add title for the report
                document.Add(new Paragraph("Billing Report").SetFontSize(20));

                // Add table headers
                document.Add(new Paragraph("Patient Name | Amount | Date"));

                // Loop through each bill and add to PDF
                foreach (var bill in bills)
                {
                    document.Add(new Paragraph($"{bill.Patient.Name} | ${bill.Amount} | {bill.Date.ToString("MM/dd/yyyy")}"));
                }

                document.Close();
                return File(pdf.ToArray(), "application/pdf", "BillingReport.pdf");
            }

            [HttpGet("patient-statistics")]
            public IActionResult GetPatientStatistics()
            {
                var totalPatients = _context.Patients.Count();
                var patientsByDepartment = _context.Patients
                    .GroupBy(p => p.Department)
                    .Select(g => new { Department = g.Key, Count = g.Count() })
                    .ToList();

                var pdf = new MemoryStream();
                var writer = new PdfWriter(pdf);
                var pdfDoc = new PdfDocument(writer);
                var document = new Document(pdfDoc);

                // Add title for the report
                document.Add(new Paragraph("Patient Statistics Report").SetFontSize(20));
                document.Add(new Paragraph($"Total Patients: {totalPatients}"));

                // Add table headers for department-wise statistics
                document.Add(new Paragraph("Department | Number of Patients"));

                // Loop through departments and add stats to PDF
                foreach (var department in patientsByDepartment)
                {
                    document.Add(new Paragraph($"{department.Department} | {department.Count}"));
                }

                document.Close();
                return File(pdf.ToArray(), "application/pdf", "PatientStatisticsReport.pdf");
            }


    }
}
