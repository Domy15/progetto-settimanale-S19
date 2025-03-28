using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace progetto_settimanale_S19.Models
{
    public class Tycket
    {
        [Required]
        public int TycketId { get; set; }

        [Required]
        public int EventId { get; set; }

        public string UserId { get; set; }

        [Required]
        public DateTime Purchased { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
