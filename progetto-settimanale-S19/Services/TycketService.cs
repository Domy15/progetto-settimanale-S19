using Microsoft.EntityFrameworkCore;
using progetto_settimanale_S19.DTOs.Tycket;
using progetto_settimanale_S19.DTOs.Event;
using progetto_settimanale_S19.DTOs.Artist;
using progetto_settimanale_S19.DTOs.Account;
using progetto_settimanale_S19.Models;

namespace progetto_settimanale_S19.Data
{
    public class TycketService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TycketService> _logger;

        public TycketService(ApplicationDbContext context, ILogger<TycketService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> CreateTycketAsync(CreateTycketDto createTycketDto, string userEmail )
        {
            try
            {
                var user = await _context.ApplicationUsers.FirstOrDefaultAsync( u => u.Email == userEmail );

                if (user == null) 
                {
                    return false;
                }

                var Event = await _context.Events.FirstOrDefaultAsync(e => e.Id == createTycketDto.EventId);

                if (Event == null) 
                {
                    return false;
                }

                for (int i = 0; i < createTycketDto.Quantity; i++)
                {
                    var newTycket = new Tycket()
                    {
                        EventId = createTycketDto.EventId,
                        UserId = user.Id,
                        Purchased = DateTime.Now,
                    };

                    _context.Tyckets.Add(newTycket);
                }
                
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<GetTycketsDto?> GetAllTycketsAsync(string email)
        {
            try
            {
                var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return null;
                }

                var tyckets = await _context.Tyckets.Where(t => t.UserId == user.Id).Include(t => t.User).Include(t => t.Event).ThenInclude(e => e.Artist).ToListAsync();

                var tycketsList = new GetTycketsDto()
                {
                    tyckets = new List<GetSingleTycketDto>()
                };

                foreach (var tycket in tyckets)
                {
                    var newTycket = new GetSingleTycketDto()
                    {
                        TycketId = tycket.TycketId,
                        EventId = tycket.EventId,
                        UserId = tycket.UserId,
                        Purchased = tycket.Purchased,
                        Event = new GetSingleEventDto()
                        {
                            Id = tycket.EventId,
                            Title = tycket.Event.Title,
                            EventDate = tycket.Event.EventDate,
                            Place = tycket.Event.Place,
                            Artist = new GetArtistDto()
                            {
                                Id = tycket.Event.ArtistId,
                                Name = tycket.Event.Artist.Name,
                                Genre = tycket.Event.Artist.Genre,
                                Biografia = tycket.Event.Artist.Biografia,
                            }
                        },
                        User = new UserDto()
                        {
                            FirstName = tycket.User.FirstName ?? "Nome non disponibile",
                            LastName = tycket.User.LastName ?? "Cognome non disponibile"
                        }
                    };

                    tycketsList.tyckets.Add(newTycket);
                }

                return tycketsList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<GetSingleTycketDto?> GetTycketByIdAsync(int id)
        {
            try
            {
                var tycket = await _context.Tyckets.Include(t => t.User).Include(t => t.Event).ThenInclude(e => e.Artist).FirstOrDefaultAsync(t => t.TycketId == id);

                if (tycket == null)
                {
                    return null;
                }

                var newTycket = new GetSingleTycketDto()
                {
                    TycketId = tycket.TycketId,
                    EventId = tycket.EventId,
                    UserId = tycket.UserId,
                    Purchased = tycket.Purchased,
                    Event = new GetSingleEventDto()
                    {
                        Id = tycket.EventId,
                        Title = tycket.Event.Title,
                        EventDate = tycket.Event.EventDate,
                        Place = tycket.Event.Place,
                        Artist = new GetArtistDto()
                        {
                            Id = tycket.Event.ArtistId,
                            Name = tycket.Event.Artist.Name,
                            Genre = tycket.Event.Artist.Genre,
                            Biografia = tycket.Event.Artist.Biografia,
                        }
                    },
                    User = new UserDto()
                    {
                        FirstName = tycket.User.FirstName ?? "Nome non disponibile",
                        LastName = tycket.User.LastName ?? "Cognome non disponibile"
                    }
                };

                return newTycket;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteTycketAsync(int id)
        {
            try
            {
                var tycket = await _context.Tyckets.FirstOrDefaultAsync(t => t.TycketId == id);

                if (tycket == null)
                {
                    return false;
                }

                _context.Tyckets.Remove(tycket);

                return await SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
