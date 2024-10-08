﻿using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;
using System.Threading;

namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IValidator<Movie> _movieValidator;

    public MovieService(IMovieRepository movieRepository, IValidator<Movie> movieValidator)
    {
        _movieRepository = movieRepository;
        _movieValidator = movieValidator;
    }

    public async Task<bool> CreateAsync(Movie movie , CancellationToken cancellationToken = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie , cancellationToken : cancellationToken);
        return  await _movieRepository.CreateAsync(movie,cancellationToken);
    }

    public Task<bool> DeleteByIdAsync(Guid id,Guid? userid = default, CancellationToken cancellationToken = default)
    {
        return _movieRepository.DeleteByIdAsync(id,, cancellationToken);
    }

    public  Task<IEnumerable<Movie>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _movieRepository.GetAllAsync(cancellationToken);
     }

    public Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _movieRepository.GetByIdAsync(id, cancellationToken);
    }

    public Task<Movie?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return _movieRepository.GetBySlugAsync(slug, cancellationToken);
    }

    public async Task<Movie?> UpdateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken);

        var movieExists = await _movieRepository.ExistsByIdAsync(movie.Id);
        if (!movieExists)
        {
            return null;
        }
        await _movieRepository.UpdateAsync(movie, cancellationToken);
        return movie; 
    }
}
