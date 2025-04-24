using Microsoft.EntityFrameworkCore;
using medicurebackend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using medicurebackend.Hubs;  // Add namespace for SignalR Hub
using medicurebackend.Services;  // Add namespace for EmailService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext for SQL Server
builder.Services.AddDbContext<HospitalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add controllers for API endpoints
builder.Services.AddControllers();

// Add CORS to allow cross-origin requests (important for frontend-backend communication)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()  // Allow any origin (you can restrict it for production)
               .AllowAnyMethod()
               .AllowAnyHeader());
});

// Register SignalR service (this allows communication via SignalR)
builder.Services.AddSignalR();

// Register EmailService (optional, for sending emails via a service like Mailgun or Brevo)
builder.Services.AddSingleton<EmailService>(provider =>
    new EmailService(
        builder.Configuration["Mailjet:ApiKey"], 
        builder.Configuration["Mailjet:ApiSecret"], 
        builder.Configuration["Mailjet:FromEmail"]
    )
);

var app = builder.Build();

// Configure Swagger and Swagger UI for development (API documentation)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS before other middleware (for frontend-backend communication)
app.UseCors("AllowAll");

// Enable HTTPS redirection (optional, for production environments)
// app.UseHttpsRedirection();

// Use authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers (API endpoints)
app.MapControllers();

// Map the SignalR Hub to a route for real-time communication
app.MapHub<NotificationHub>("/notificationHub");  // Maps the SignalR hub to the /notificationHub endpoint

// Start the application
app.Run();
