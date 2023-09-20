using Tickets.Core.Aggregates.MovieSessionAggregate.Specifications;
using Tickets.Core.Aggregates.TicketOrderAggregate.Events;
using Tickets.SharedKernel.Interfaces;
using MediatR;

namespace Tickets.Core.Aggregates.MovieSessionAggregate.Events.Handlers
{
    internal class TicketOrderCreatedEventHandler : INotificationHandler<TicketOrderCreatedEvent>
    {
        private readonly IRepository<MovieSession> _repository;

        public TicketOrderCreatedEventHandler(IRepository<MovieSession> repository)
        {
            _repository = repository;
        }

        public async Task Handle(TicketOrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            var movieSession = await _repository.FirstOrDefaultAsync(new GetMovieSessionByIdWithSeatsSpec(notification.MovieSessionId), cancellationToken);

            if (movieSession == null)
            {
                throw new Exception($"Movie session with id {notification.MovieSessionId} not found.");
            }

            movieSession.MarkSeatsAsSold(notification.SessionSeatIds);

            await _repository.SaveChangesAsync(cancellationToken);
        }
    }
}
