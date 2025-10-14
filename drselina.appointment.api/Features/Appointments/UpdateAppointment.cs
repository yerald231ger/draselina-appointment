using drselina.appointment.api.Data;

namespace drselina.appointment.api.Features.Appointments;

public static class UpdateAppointment
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
        app.MapPut("/api/appointments/{id:int}", Handle)
            .WithName("UpdateAppointment")
            .WithTags("Appointments")
            .WithOpenApi();

        return app;
    }

    // Handler
    public static async Task<IResult> Handle(int id, Request request, AppointmentDbContext db)
    {
        // Validation
        if (id <= 0)
        {
            return Results.BadRequest(new { message = "Invalid appointment ID" });
        }

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

        // Query
        var appointment = await db.Appointments.FindAsync(id);

        if (appointment is null)
        {
            return Results.NotFound(new { message = "Appointment not found" });
        }

        // Update entity
        appointment.PatientName = request.PatientName.Trim();
        appointment.PatientEmail = request.PatientEmail.Trim();
        appointment.PatientPhone = request.PatientPhone.Trim();
        appointment.AppointmentDate = request.AppointmentDate;
        appointment.Reason = request.Reason.Trim();
        appointment.Notes = request.Notes?.Trim();
        appointment.UpdatedAt = DateTime.UtcNow;

        // Save to database
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

        return Results.Ok(response);
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
