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
                                            && ((String.IsNullOrWhiteSpace(searchTitle))
                                                    ||(appointment.appointmentTitle!=null
                                                            &&appointment.appointmentTitle.ToLower().Contains(searchTitle.ToLower())))
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

        private int AppointmentBinarySearch(Guid searchId)
        {
                int start=0,end=_userAppointments.Count();
                int indexFound=-1;
                while(start<end){
                    int mid=(start+end)/2;
                    var idMatched=searchId.CompareTo(_userAppointments[mid].appointmentId);
                    if(idMatched==0){
                        indexFound= mid;
                        break;
                    }
                    else if(idMatched>0){
                        start=mid+1;
                    }
                    else{
                        end=mid;
                    }
                }
                return indexFound;
              
        }

        public Appointment? GetAppointmentById(Guid appointmentId)
        {
            var appointmentIndex=-1;
            if(_userAppointments.Any())
            {
                appointmentIndex=AppointmentBinarySearch(appointmentId);
            }
            return (appointmentIndex!=-1? _userAppointments[appointmentIndex]:null);
        }
       
        public bool DeleteAppointment(Guid appointmentId)
        {
            var appointmentMatched = GetAppointmentById(appointmentId);
            return (appointmentMatched != null ? _userAppointments.Remove(appointmentMatched) : false);
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
                _userAppointments=_userAppointments.OrderBy(appointment=>appointment.appointmentId).ToList();
                return new NewAppointmentId() { Id = appointmentToBeAdded.appointmentId };
            }
            return null;
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
                    _userAppointments=_userAppointments.OrderBy(appointment=>appointment.appointmentId).ToList();
                    return true;
                }
                return false;
            }

        }
    }
}

// BinarySearch 
            // if(EventData.meetingData.ContainsKey(date)){
            //     int start=0;
            //     int end=EventData.meetingData[date].Count;
            //     while(start<end){
            //         int mid=(start+end)/2;
            //         if(EventData.meetingData[date][mid].StartTime==startTime){
            //             EventData.meetingData[date].Remove(EventData.meetingData[date][mid]);
            //             if(EventData.meetingData[date].Count==0){
            //                 EventData.meetingData.Remove(date);
            //             }
            //             return true;
            //         }
            //         else if(EventData.meetingData[date][mid].StartTime>startTime){
            //             end=mid;
            //         }
            //         else{
            //             start=mid;
            //         }
            //     }
            // }
            // return false;    

