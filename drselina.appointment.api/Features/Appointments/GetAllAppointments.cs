using drselina.appointment.api.Data;
using Microsoft.EntityFrameworkCore;

namespace drselina.appointment.api.Features.Appointments;

public static class GetAllAppointments
{
    // DTOs
    public record Request(DateTime? StartDate, DateTime? EndDate);

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
        app.MapGet("/api/appointments", Handle)
            .WithName("GetAllAppointments")
            .WithTags("Appointments")
            .WithOpenApi();

        return app;
    }

    // Handler
    public static async Task<IResult> Handle(
        AppointmentDbContext db,
        DateTime? startDate,
        DateTime? endDate)
    {
        // Validation
        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
        {
            return Results.BadRequest(new { message = "Start date cannot be after end date" });
        }

        // Query
        var query = db.Appointments.AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(a => a.AppointmentDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(a => a.AppointmentDate <= endDate.Value);
        }

        var appointments = await query
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();

        // Mapping
        var response = appointments.Select(a => new Response(
            a.Id,
            a.PatientName,
            a.PatientEmail,
            a.PatientPhone,
            a.AppointmentDate,
            a.Reason,
            a.Notes,
            a.CreatedAt,
            a.UpdatedAt
        )).ToList();

        return Results.Ok(response);
    }
}
