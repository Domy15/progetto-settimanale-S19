using Microsoft.EntityFrameworkCore;
using progetto_settimanale_S19.Data;
using progetto_settimanale_S19.DTOs.Artist;
using progetto_settimanale_S19.DTOs.Event;
using progetto_settimanale_S19.Models;

namespace progetto_settimanale_S19.Services
{
    public class EventService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EventService> _logger;

        public EventService(ApplicationDbContext context, ILogger<EventService> logger)
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

        public async Task<bool> CreateEventAsync(CreateEventRequestDto createEventDto)
        {
            try
            {
                var artist = await _context.Artists.FirstOrDefaultAsync(a => a.Id == createEventDto.ArtistId);

                if (artist == null)
                {
                    return false;
                }

                var newEvent = new Event()
                {
                    Title = createEventDto.Title,
                    EventDate = createEventDto.EventDate,
                    Place = createEventDto.Place,
                    ArtistId = createEventDto.ArtistId,
                };

                _context.Events.Add(newEvent);

                return await SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<GetEventsResponseDto?> GetAllEventsAsync()
        {
            try
            {
                var events = await _context.Events.Include(e => e.Artist).ToListAsync();

                var eventResponse = new GetEventsResponseDto()
                {
                    Events = new List<GetSingleEventDto>()
                };

                foreach (var Event in events)
                {
                    var newEvent = new GetSingleEventDto()
                    {
                        Id = Event.Id,
                        Title = Event.Title,
                        EventDate = Event.EventDate,
                        Place = Event.Place,
                        Artist = new GetArtistDto()
                        {
                            Id = Event.Artist.Id,
                            Name = Event.Artist.Name,
                            Genre = Event.Artist.Genre,
                            Biografia = Event.Artist.Biografia,
                        }
                    };

                    eventResponse.Events.Add(newEvent);
                }

                return eventResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<GetSingleEventDto?> GetEventByIdAsync(int id)
        {
            try
            {
                var Event = await _context.Events.Include(e => e.Artist).FirstOrDefaultAsync(e => e.Id == id);

                if (Event == null)
                {
                    return null;
                }

                var newEvent = new GetSingleEventDto()
                {
                    Id = Event.Id,
                    Title = Event.Title,
                    EventDate = Event.EventDate,
                    Place = Event.Place,
                    Artist = new GetArtistDto()
                    {
                        Id = Event.Artist.Id,
                        Name = Event.Artist.Name,
                        Genre = Event.Artist.Genre,
                        Biografia = Event.Artist.Biografia,
                    }
                };

                return newEvent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateEventAsync(int id, UpdateEventRequestDto updateEvent)
        {
            try
            {
                var existingEvent = await _context.Events.FirstOrDefaultAsync(a => a.Id == id);

                if (existingEvent == null)
                {
                    return false;
                }

                existingEvent.Title = updateEvent.Title;
                existingEvent.EventDate = updateEvent.EventDate;
                existingEvent.Place = updateEvent.Place;

                return await SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            try
            {
                var Event = await _context.Events.FirstOrDefaultAsync(a => a.Id == id);

                if (Event == null)
                {
                    return false;
                }

                _context.Events.Remove(Event);

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
