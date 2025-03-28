using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace progetto_settimanale_S19.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string Place { get; set; }

        public int ArtistId { get; set; }

        [ForeignKey("ArtistId")]
        public Artist Artist { get; set; }

        public ICollection<Tycket> Tyckets { get; set; }
    }
}
