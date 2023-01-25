using System.ComponentModel.DataAnnotations;

namespace DisprzTraining.Model
{
    public class AppointmentDTO
    {
        [Required(ErrorMessage ="Please Enter Valid DateTime")]
        public DateTime appointmentStartTime { get; set; }

        [Required(ErrorMessage ="Please Enter Valid DateTime")]
        public DateTime appointmentEndTime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Title")]
        public string? appointmentTitle { get; set;}
        
        public string? appointmentDescription { get; set; }
    }
}
