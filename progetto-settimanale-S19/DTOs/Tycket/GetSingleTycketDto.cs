using progetto_settimanale_S19.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using progetto_settimanale_S19.DTOs.Event;
using progetto_settimanale_S19.DTOs.Account;

namespace progetto_settimanale_S19.DTOs.Tycket
{
    public class GetSingleTycketDto
    {
        public int TycketId { get; set; }
        public int EventId { get; set; }
        public required string UserId { get; set; }
        public DateTime Purchased { get; set; }
        public GetSingleEventDto Event { get; set; }
        public UserDto User { get; set; }
    }
}
