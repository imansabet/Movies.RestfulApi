﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers
{
    //[Route("api")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService  movieService)
        {
            _movieService = movieService;
        }

        [Authorize(AuthConstants.AdminUserClaimName)]
        [HttpPost(ApiEndpoints.Movies.Create)]
        public async Task<IActionResult> Create([FromBody] CreateMovieRequest request , CancellationToken cancellationToken )
        {
            var movie = request.MapToMovie();

            await _movieService.CreateAsync(movie,cancellationToken);
            return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
        }

        [HttpGet(ApiEndpoints.Movies.Get)]
        public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetUserId();

            var movie = Guid.TryParse(idOrSlug, out var id)
                ? await _movieService.GetByIdAsync(id,userId ,cancellationToken)
                : await _movieService.GetBySlugAsync(idOrSlug, userId, cancellationToken);

            if(movie is null)
            {
                return NotFound();
            }
            var response = movie.MapToResponse();
            return Ok(response);
        }
        
        [HttpGet(ApiEndpoints.Movies.GetAll)]
        public async Task<IActionResult> GetAll( CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetUserId();

            var movies = await _movieService.GetAllAsync(userId, cancellationToken);
            var moviesResponse = movies.MapToResponse();
            return Ok(moviesResponse);
        }   

        [Authorize(AuthConstants.TrustedMemberPoilcyname )]
        [HttpPut(ApiEndpoints.Movies.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id,
            [FromBody] UpdateMovierequest request, CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetUserId();

            var movie = request.MapToMovie(id);

            var updatedMovie = await _movieService.UpdateAsync(userId,movie, cancellationToken);

            if (updatedMovie is null)
            {
                return NotFound();
            }
            var response = movie.MapToResponse();
            return Ok(response);
        }

        [Authorize(AuthConstants.AdminUserClaimName)]
        [HttpPut(ApiEndpoints.Movies.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _movieService.DeleteByIdAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
