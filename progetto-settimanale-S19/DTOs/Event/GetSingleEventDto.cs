using progetto_settimanale_S19.DTOs.Artist;

namespace progetto_settimanale_S19.DTOs.Event
{
    public class GetSingleEventDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required DateTime EventDate { get; set; }
        public required string Place { get; set; }
        public GetArtistDto Artist { get; set; }
    }
}
