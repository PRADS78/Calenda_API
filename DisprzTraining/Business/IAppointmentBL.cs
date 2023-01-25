using DisprzTraining.Model;


namespace DisprzTraining.Business
{
    public interface IAppointmentBL
    {
        PaginatedAppointments GetAllAppointments(int offSet, int fetchCount,DateTime? startDate,DateTime? endDate,string? searchTitle);
        NewAppointmentId? AddAppointment(AppointmentDTO newAppointment);
        bool DeleteAppointment(Guid appointmentId);
        bool? UpdateAppointment(Guid appointmentId, AppointmentDTO existingAppointment);
        Appointment? GetAppointmentById(Guid appointmentId);
    }
}