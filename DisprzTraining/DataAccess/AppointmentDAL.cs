using DisprzTraining.Model;

namespace DisprzTraining.DataAccess
{
    public class AppointmentDAL : IAppointmentDAL
    {


        private static List<Appointment> _userAppointments = new();

        PaginatedAppointments appointmentsFound = new PaginatedAppointments();




        public PaginatedAppointments GetAllAppointments(int offSet, int fetchCount, DateTime? startDate, DateTime? endDate, string? searchTitle)
        {
            var appointmentMatched = (from appointment in _userAppointments
                                      where (
                                            (startDate == null ? (endDate == null ? true : (appointment.appointmentEndTime.Date<= endDate.Value))                                             
                                                                        : (endDate == null ? (appointment.appointmentStartTime.Date >= startDate.Value)                                            
                                                                            : (appointment.appointmentStartTime.Date >= startDate.Value
                                                                                    && appointment.appointmentEndTime.Date <= endDate.Value)
                                                                                    )
                                            )
                                            && ((String.IsNullOrWhiteSpace(searchTitle))||(appointment.appointmentTitle.ToLower().Contains(searchTitle.ToLower())))
                                            )
                                      orderby (appointment.appointmentStartTime)
                                      select appointment).ToList();

            if (appointmentMatched.Any() && fetchCount > 0)
            {
                appointmentsFound.appointments = appointmentMatched.Skip(offSet).Take(fetchCount).ToList();
                appointmentsFound.isTruncated = fetchCount >= appointmentMatched.Skip(offSet).Count() ? false : true;
            }
            else
            {
                appointmentsFound.appointments = appointmentMatched;
                appointmentsFound.isTruncated = false;
            }
            return appointmentsFound;
        }


        public Appointment? GetAppointmentById(Guid appointmentId)
        {
            var appointmentById = (from appointment in _userAppointments
                                   where appointment.appointmentId == appointmentId
                                   select appointment).FirstOrDefault();
            return appointmentById;
        }


        private bool CheckAppointmentConflict(DateTime startTime, DateTime endTime, List<Appointment> appointments)
        {
            var CheckAppointmentPresent = (from appointment in appointments
                                           where (appointment.appointmentStartTime < endTime) && (startTime < appointment.appointmentEndTime)
                                           select appointment).ToList();
            return (CheckAppointmentPresent.Any() ? false : true);
        }

        public NewAppointmentId? AddAppointment(AppointmentDTO newAppointment)
        {
            var noConflict = _userAppointments.Any() ?
                                CheckAppointmentConflict(newAppointment.appointmentStartTime, newAppointment.appointmentEndTime, _userAppointments)
                                    : true;
            if (noConflict)
            {
                Appointment appointmentToBeAdded = new()
                {
                    appointmentId = Guid.NewGuid(),
                    appointmentStartTime = newAppointment.appointmentStartTime,
                    appointmentEndTime = newAppointment.appointmentEndTime,
                    appointmentTitle = newAppointment.appointmentTitle,
                    appointmentDescription = newAppointment.appointmentDescription
                };
                _userAppointments.Add(appointmentToBeAdded);
                return new NewAppointmentId() { Id = appointmentToBeAdded.appointmentId };
            }
            return null;
        }

        public bool DeleteAppointment(Guid appointmentId)
        {
            var appointmentMatched = GetAppointmentById(appointmentId);
            return (appointmentMatched != null ? _userAppointments.Remove(appointmentMatched) : false);
        }

        public bool? UpdateAppointment(Guid appointmentId, AppointmentDTO updateAppointment)
        {
            var appointmentMatched = GetAppointmentById(appointmentId);
            if (appointmentMatched == null)
                return null;
            else
            {
                var appointmentsFound = (from appointment in _userAppointments
                                         where (appointment.appointmentId != appointmentId)
                                         select appointment).ToList();

                var noConflict = appointmentsFound.Any() ?
                                    CheckAppointmentConflict(updateAppointment.appointmentStartTime, updateAppointment.appointmentEndTime, appointmentsFound)
                                        : true;

                if (noConflict)
                {
                    DeleteAppointment(appointmentId);
                    Appointment appointmentToBeUpdated = new()
                    {
                        appointmentId = appointmentId,
                        appointmentStartTime = updateAppointment.appointmentStartTime,
                        appointmentEndTime = updateAppointment.appointmentEndTime,
                        appointmentTitle = updateAppointment.appointmentTitle,
                        appointmentDescription = updateAppointment.appointmentDescription
                    };
                    _userAppointments.Add(appointmentToBeUpdated);
                    return true;
                }
                return false;
            }

        }
    }

}

