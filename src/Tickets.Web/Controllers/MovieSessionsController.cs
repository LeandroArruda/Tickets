using Tickets.Core.Aggregates.MovieSessionAggregate;
using Tickets.Core.Aggregates.MovieSessionAggregate.Specifications;
using Tickets.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Tickets.Web.Api;

public class MovieSessionsController : BaseApiController
{
    private readonly IRepository<MovieSession> _repository;

    public MovieSessionsController(IRepository<MovieSession> repository)
    {
        _repository = repository;
    }

    // GET: api/MovieSessions
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var MovieSessions = (await _repository.ListAsync()).ToList();

        return Ok(MovieSessions);
    }

    // GET: api/MovieSessions
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var movieSession = await _repository.GetByIdAsync<Guid>(id);
        if (movieSession == null)
        {
            return NotFound();
        }

        return Ok(movieSession);
    }

    // POST: api/MovieSessions
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] MovieSessionDTO movieSessionDTO)
    {
        var createdMovieSession = await _repository.AddAsync(new MovieSession(movieSessionDTO.RoomId,
                                                                              movieSessionDTO.MovieId,
                                                                              movieSessionDTO.StartDateTime,
                                                                              movieSessionDTO.EndDateTime,
                                                                              movieSessionDTO.BreakDuration,
                                                                              movieSessionDTO.Advertisements,
                                                                              movieSessionDTO.SessionSeats));

        return Ok(createdMovieSession.Id);
    }

    // POST: api/MovieSessions/{movieSessionId}/Seats/{seatId}/Reserve
    [HttpPut("{movieSessionId:Guid}/Seats/{sessionSeatId:Guid}/Reserve")]
    public async Task<IActionResult> ReserveSeat(Guid movieSessionId, Guid sessionSeatId)
    {
        var movieSession = await _repository.FirstOrDefaultAsync(new GetMovieSessionByIdWithSeatsSpec(movieSessionId));

        if (movieSession == null) return NotFound("No such session");

        movieSession.ReserveASeat(sessionSeatId);

        await _repository.UpdateAsync(movieSession);

        return Ok();
    }
}

public record MovieSessionDTO(Guid RoomId, Guid MovieId, DateTime StartDateTime, DateTime EndDateTime, int BreakDuration, List<string> Advertisements, List<SessionSeat> SessionSeats);