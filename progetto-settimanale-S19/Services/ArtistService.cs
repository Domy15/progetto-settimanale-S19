using Microsoft.EntityFrameworkCore;
using progetto_settimanale_S19.Data;
using progetto_settimanale_S19.DTOs.Artist;
using progetto_settimanale_S19.DTOs.Event;
using progetto_settimanale_S19.Models;

namespace progetto_settimanale_S19.Services
{
    public class ArtistService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ArtistService> _logger;

        public ArtistService(ApplicationDbContext context, ILogger<ArtistService> logger)
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

        public async Task<bool> CreateArtistAsync(CreateArtistDto createArtistDto)
        {
            try
            {
                var artist = new Artist()
                {
                    Name = createArtistDto.Name,
                    Genre = createArtistDto.Genre,
                    Biografia = createArtistDto.Biografia,
                };

                _context.Artists.Add(artist);

                return await SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<GetArtistsResponseDto?> GetAllArtists()
        {
            try
            {
                var artistsList = await _context.Artists.Include(a => a.Events).ToListAsync();

                var artistsResponse = new GetArtistsResponseDto()
                {
                    artists = new List<ArtistDto>()
                };

                foreach (var artist in artistsList) 
                {
                    var newartist = new ArtistDto()
                    {
                        Id = artist.Id,
                        Name= artist.Name,
                        Genre = artist.Genre,
                        Biografia= artist.Biografia,
                        Events = artist.Events.Select(e => new EventDto()
                        {
                            Title = e.Title,
                            EventDate = e.EventDate,
                            Place = e.Place
                        }).ToList()
                    };

                    artistsResponse.artists.Add(newartist);
                }

                return artistsResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<ArtistDto?> GetArtistByIdAsync(int id)
        {
            try
            {
                var artist = await _context.Artists.Include(a => a.Events).FirstOrDefaultAsync(a => a.Id == id);

                if (artist == null)
                {
                    return null;
                }

                var artistDto = new ArtistDto()
                {
                    Id = artist.Id,
                    Name = artist.Name,
                    Genre = artist.Genre,
                    Biografia = artist.Biografia,
                    Events = artist.Events.Select(e => new EventDto()
                    {
                        Title = e.Title,
                        EventDate = e.EventDate,
                        Place = e.Place
                    }).ToList()
                };

                return artistDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateArtistAsync(int id, UpdateArtistRequestDto artist)
        {
            try
            {
                var existingArtist = await _context.Artists.FirstOrDefaultAsync(a => a.Id == id);

                if (existingArtist == null)
                {
                    return false;
                }

                existingArtist.Name = artist.Name;
                existingArtist.Genre = artist.Genre;
                existingArtist.Biografia = artist.Biografia;

                return await SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteArtistAsync(int id)
        {
            try
            {
                var artist =await _context.Artists.FirstOrDefaultAsync(a => a.Id == id);

                if (artist == null)
                {
                    return false;
                }

                _context.Artists.Remove(artist);

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
