using drselina.appointment.api.Data;

namespace drselina.appointment.api.Features.Appointments;

public static class GetAppointmentById
{
    // DTOs
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
        app.MapGet("/api/appointments/{id:int}", Handle)
            .WithName("GetAppointmentById")
            .WithTags("Appointments")
            .WithOpenApi();

        return app;
    }

    // Handler
    public static async Task<IResult> Handle(int id, AppointmentDbContext db)
    {
        // Validation
        if (id <= 0)
        {
            return Results.BadRequest(new { message = "Invalid appointment ID" });
        }

        // Query
        var appointment = await db.Appointments.FindAsync(id);

        if (appointment is null)
        {
            return Results.NotFound(new { message = "Appointment not found" });
        }

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
}
