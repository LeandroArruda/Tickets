using Ardalis.Specification;

namespace Tickets.Core.Aggregates.MovieSessionAggregate.Specifications
{
    public class GetMovieSessionByIdWithSeatsSpec : Specification<MovieSession>
    {
        public GetMovieSessionByIdWithSeatsSpec(Guid id)
        {
            Query.Where(m => m.Id == id).Include(movieSession => movieSession.SessionSeats);
        }
    }
}