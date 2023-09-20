using Tickets.SharedKernel;

namespace Tickets.Core.Aggregates.MovieSessionAggregate.Events
{
    public class MovieSessionSoldOutEvent : DomainEventBase
    {
        public Guid MovieSessionId { get; private set; }

        public MovieSessionSoldOutEvent(Guid movieSessionId)
        {
            MovieSessionId = movieSessionId;
        }
    }
}
