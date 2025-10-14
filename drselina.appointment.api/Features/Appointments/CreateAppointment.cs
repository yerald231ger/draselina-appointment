using drselina.appointment.api.Data;

namespace drselina.appointment.api.Features.Appointments;

public static class CreateAppointment
{
    // DTOs
    public record Request(
        string PatientName,
        string PatientEmail,
        string PatientPhone,
        DateTime AppointmentDate,
        string Reason,
        string? Notes
    );

    public record Response(
        int Id,
        string PatientName,
        string PatientEmail,
        string PatientPhone,
        DateTime AppointmentDate,
        string Reason,
        string? Notes,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    // Endpoint Registration
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/appointments", Handle)
            .WithName("CreateAppointment")
            .WithTags("Appointments")
            .WithOpenApi();

        return app;
    }

    // Handler
    public static async Task<IResult> Handle(Request request, AppointmentDbContext db)
    {
        // Validation
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.PatientName))
            errors.Add("Patient name is required");

        if (string.IsNullOrWhiteSpace(request.PatientEmail))
            errors.Add("Patient email is required");
        else if (!IsValidEmail(request.PatientEmail))
            errors.Add("Patient email is not valid");

        if (string.IsNullOrWhiteSpace(request.PatientPhone))
            errors.Add("Patient phone is required");

        if (request.AppointmentDate < DateTime.UtcNow.Date)
            errors.Add("Appointment date cannot be in the past");

        if (string.IsNullOrWhiteSpace(request.Reason))
            errors.Add("Appointment reason is required");

        if (errors.Any())
        {
            return Results.BadRequest(new { message = "Validation failed", errors });
        }

        // Create entity
        var appointment = new Appointment
        {
            PatientName = request.PatientName.Trim(),
            PatientEmail = request.PatientEmail.Trim(),
            PatientPhone = request.PatientPhone.Trim(),
            AppointmentDate = request.AppointmentDate,
            Reason = request.Reason.Trim(),
            Notes = request.Notes?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        // Save to database
        db.Appointments.Add(appointment);
        await db.SaveChangesAsync();

        // Mapping
        var response = new Response(
            appointment.Id,
            appointment.PatientName,
            appointment.PatientEmail,
            appointment.PatientPhone,
            appointment.AppointmentDate,
            appointment.Reason,
            appointment.Notes,
            appointment.CreatedAt,
            appointment.UpdatedAt
        );

        return Results.Created($"/api/appointments/{appointment.Id}", response);
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
