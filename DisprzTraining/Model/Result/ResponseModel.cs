namespace DisprzTraining.Model
{
    //response for creation
    public class NewAppointmentId
    {
        public Guid Id { get; set; }
    }

        public class ErrorResponse
    {
        public string language { get; set; }=string.Empty;
        public string errorMessage { get; set; }=string.Empty;
        public string errorCode { get; set; }=string.Empty;
    }


        public class PaginatedAppointments
    {
        public bool isTruncated { get; set; }
        public List<Appointment> appointments { get; set; }=new List<Appointment>();
    }
    
}  