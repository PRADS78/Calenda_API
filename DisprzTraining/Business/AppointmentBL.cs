using DisprzTraining.CustomException;
using DisprzTraining.DataAccess;
using DisprzTraining.Model;
using DisprzTraining.Model.Result;

namespace DisprzTraining.Business
{
    public class AppointmentBL : IAppointmentBL
    {
        private readonly IAppointmentDAL _appointmentDAL;

        public AppointmentBL(IAppointmentDAL appointmentDAL)
        {
            _appointmentDAL = appointmentDAL;
        }

        //get appointment
        public PaginatedAppointments GetAllAppointments(int offSet, int fetchCount, DateTime? startDate, DateTime? endDate, string? searchTitle)
        {
            return _appointmentDAL.GetAllAppointments(offSet, fetchCount, startDate, endDate, searchTitle);
        }

        //get appointment by Id      
        public Appointment? GetAppointmentById(Guid appointmentId)
        {
            var appointmentById = _appointmentDAL.GetAppointmentById(appointmentId);
            return appointmentById != null ? appointmentById : null;
        }

        // create new appointment

        public NewAppointmentId? AddAppointment(AppointmentDTO newAppointment)
        {
            if (newAppointment.appointmentEndTime < newAppointment.appointmentStartTime) throw new InputErrorException(AppointmentErrorResponse.EndTimeLessThanStartTime);
            else if (newAppointment.appointmentEndTime == newAppointment.appointmentStartTime) throw new InputErrorException(AppointmentErrorResponse.SameTiming);
            else if (newAppointment.appointmentStartTime < DateTime.Now) throw new InputErrorException(AppointmentErrorResponse.PastTiming);
            else return _appointmentDAL.AddAppointment(newAppointment);
        }

        //delete Appointment
        public bool DeleteAppointment(Guid appointmentId)
        {
            return _appointmentDAL.DeleteAppointment(appointmentId);
        }

        //update appointment
        public bool? UpdateAppointment(Guid appointmentId, AppointmentDTO updateAppointment)
        {
            if (updateAppointment.appointmentEndTime < updateAppointment.appointmentStartTime) throw new InputErrorException(AppointmentErrorResponse.EndTimeLessThanStartTime);
            else if (updateAppointment.appointmentEndTime == updateAppointment.appointmentStartTime) throw new InputErrorException(AppointmentErrorResponse.SameTiming);            
            else if (updateAppointment.appointmentStartTime < DateTime.Now) throw new InputErrorException(AppointmentErrorResponse.PastTiming);
            else{
            bool? isUpdated = _appointmentDAL.UpdateAppointment(appointmentId, updateAppointment);
            return isUpdated != null ? (isUpdated == true ? true : false) : null;
            }
        }
    }
}
