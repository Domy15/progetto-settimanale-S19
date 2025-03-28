using System.ComponentModel.DataAnnotations;

namespace progetto_settimanale_S19.DTOs.Event
{
    public class UpdateEventRequestDto
    {
        public required string Title { get; set; }
        public required DateTime EventDate { get; set; }
        public required string Place { get; set; }
    }
}
