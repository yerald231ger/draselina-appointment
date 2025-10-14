using drselina.appointment.api.Data;

namespace drselina.appointment.api.Features.Appointments;

public static class DeleteAppointment
{
    // Endpoint Registration
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/appointments/{id:int}", Handle)
            .WithName("DeleteAppointment")
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

        // Delete from database
        db.Appointments.Remove(appointment);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}
