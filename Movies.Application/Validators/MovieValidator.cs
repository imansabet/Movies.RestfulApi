﻿
using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;

namespace Movies.Application.Validators;

public class MovieValidator : AbstractValidator<Movie>
{
    private readonly IMovieRepository  _movieRcepository;
    public MovieValidator(IMovieRepository movieRepository)
    {
        _movieRcepository = movieRepository;
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Genres) 
            .NotEmpty();

        RuleFor(x => x.YearOfRelease)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);

        RuleFor(x => x.Slug)
            .MustAsync(ValidateSlug)
            .WithMessage("This movie already exists in the system"); 
    }

    private async Task<bool> ValidateSlug(Movie movie,string slug, CancellationToken token)
    {
        var existingMovie = await _movieRcepository.GetBySlugAsync(slug);
        if (existingMovie is not null)
        {
            return existingMovie.Id == movie.Id;
        }
        return existingMovie is null;
    }
}
