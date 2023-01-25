using DisprzTraining.Model;

namespace DisprzTraining.DataAccess
{
    public interface IAppointmentDAL
    {
        bool DeleteAppointment(Guid appointmentId);
        NewAppointmentId? AddAppointment(AppointmentDTO newAppointment); 
        bool? UpdateAppointment(Guid appointmentId,AppointmentDTO updateAppointment);
        PaginatedAppointments GetAllAppointments(int offSet, int fetchCount, DateTime? startDate,DateTime? endDate,string? searchTitle);
        Appointment? GetAppointmentById(Guid appointmentId);
    }
}