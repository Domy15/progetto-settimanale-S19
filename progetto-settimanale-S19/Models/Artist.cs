using System.ComponentModel.DataAnnotations;

namespace progetto_settimanale_S19.Models
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string Biografia { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
