using System.ComponentModel.DataAnnotations;

namespace progetto_settimanale_S19.DTOs.Artist
{
    public class CreateArtistDto
    {
        public required string Name { get; set; }
        public required string Genre { get; set; }
        public required string Biografia { get; set; }
    }
}
