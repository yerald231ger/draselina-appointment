namespace drselina.appointment.api.Features.Appointments;

public static class AppointmentEndpoints
{
    public static void MapAppointmentEndpoints(this IEndpointRouteBuilder app)
    {
        GetAllAppointments.MapEndpoint(app);
        GetAppointmentById.MapEndpoint(app);
        CreateAppointment.MapEndpoint(app);
        UpdateAppointment.MapEndpoint(app);
        DeleteAppointment.MapEndpoint(app);
    }
}
