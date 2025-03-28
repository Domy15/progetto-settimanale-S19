using System.ComponentModel.DataAnnotations;

namespace progetto_settimanale_S19.DTOs.Event
{
    public class CreateEventRequestDto
    {
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string Place { get; set; }
        public int ArtistId { get; set; }
    }
}
