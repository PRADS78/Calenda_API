using System.ComponentModel.DataAnnotations;

namespace DisprzTraining.Model
{
    public class GetAppointmentQueryParameters
    {
        [Required]
        public int offSet { get; set; } = 0;
        [Required]
        public int fetchCount { get; set; } = 10;
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string? searchTitle { get; set; }
    }
}