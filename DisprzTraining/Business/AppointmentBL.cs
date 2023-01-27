using DisprzTraining.CustomException;
using DisprzTraining.DataAccess;
using DisprzTraining.Model;
using DisprzTraining.Model.Result;
using DisprzTraining.Extensions;

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
            if (newAppointment.appointmentEndTime < newAppointment.appointmentStartTime) throw new InputTimeErrorException(AppointmentErrorResponse.EndTimeLessThanStartTime);
            else if (newAppointment.appointmentEndTime == newAppointment.appointmentStartTime) throw new InputTimeErrorException(AppointmentErrorResponse.SameTiming);
            else if (newAppointment.appointmentStartTime < DateTime.Now) throw new InputTimeErrorException(AppointmentErrorResponse.PastTiming);
            else if (newAppointment.appointmentEndTime > DateTimeExtension.GetEndOfDay(newAppointment.appointmentStartTime.Date)) throw new InputTimeErrorException(AppointmentErrorResponse.TimeRange);

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
            if (updateAppointment.appointmentEndTime < updateAppointment.appointmentStartTime) throw new InputTimeErrorException(AppointmentErrorResponse.EndTimeLessThanStartTime);
            else if (updateAppointment.appointmentEndTime == updateAppointment.appointmentStartTime) throw new InputTimeErrorException(AppointmentErrorResponse.SameTiming);
            else if (updateAppointment.appointmentStartTime < DateTime.Now) throw new InputTimeErrorException(AppointmentErrorResponse.PastTiming);
            else if (updateAppointment.appointmentEndTime > DateTimeExtension.GetEndOfDay(updateAppointment.appointmentStartTime.Date)) throw new InputTimeErrorException(AppointmentErrorResponse.TimeRange);
            else
            {
                bool? isUpdated = _appointmentDAL.UpdateAppointment(appointmentId, updateAppointment);
                return isUpdated != null ? (isUpdated == true ? true : false) : null;
            }
        }
    }
}
