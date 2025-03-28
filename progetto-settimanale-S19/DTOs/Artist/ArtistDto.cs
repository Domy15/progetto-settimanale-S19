using progetto_settimanale_S19.DTOs.Event;

namespace progetto_settimanale_S19.DTOs.Artist
{
    public class ArtistDto
    {
        public required int Id { get; set;  }
        public required string Name { get; set; }
        public required string Genre { get; set; }
        public required string Biografia { get; set; }
        public ICollection<EventDto> Events { get; set; }
    }
}
