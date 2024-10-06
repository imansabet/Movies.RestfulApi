using Movies.Application.Models;

namespace Movies.Application.Repositories;

public interface IMovieRepository
{
    Task<bool> CreateAsync(MovieDto movie);
    Task<MovieDto?> UpdateAsync(MovieDto movie);
    Task<MovieDto?> GetByIdAsync(Guid id);
    Task<bool> DeleteByIdAsync(Guid id);
    Task<MovieDto?> GetBySlugAsync(string slug);
    Task<IEnumerable<MovieDto>> GetAllAsync();
    Task<bool> ExistsByIdAsync(Guid id);
}
