using drselina.appointment.api.Data;
using drselina.appointment.api.Features.Appointments;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Add DbContext
builder.Services.AddDbContext<AppointmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppointmentDbContext>("database");

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();

// Map health check endpoints
app.MapHealthChecks("/health");

// Map appointment endpoints
app.MapAppointmentEndpoints();

app.Run();