using Microsoft.EntityFrameworkCore;
using medicurebackend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the DbContext with the SQL Server connection string
builder.Services.AddDbContext<HospitalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

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

var app = builder.Build();  

// Configure Swagger and Swagger UI for development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS before other middleware
app.UseCors("AllowAll");

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Use authorization middleware
app.UseAuthorization();

// Map controllers (API endpoints)
app.MapControllers();

// Start the application
app.Run();
