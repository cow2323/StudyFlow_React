using System.ComponentModel.DataAnnotations;

namespace StudyFlow.DTOs; 

public class BookingDto
{
    

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value Must be 1 or greater")]
        public int RoomId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        


}