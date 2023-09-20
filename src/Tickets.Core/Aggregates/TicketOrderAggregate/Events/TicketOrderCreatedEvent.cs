using Tickets.SharedKernel;

namespace Tickets.Core.Aggregates.TicketOrderAggregate.Events
{
    public class TicketOrderCreatedEvent : DomainEventBase
    {
        public int TicketOrderId { get; private set; }
        public Guid MovieSessionId { get; private set; }
        public List<Guid> SessionSeatIds { get; private set; }
        public int TotalValue { get; private set; }

        public TicketOrderCreatedEvent(int ticketOrderId, Guid movieSessionId, List<Guid> sessionSeatIds, int totalValue)
        {
            TicketOrderId = ticketOrderId;
            MovieSessionId = movieSessionId;
            SessionSeatIds = sessionSeatIds;
            TotalValue = totalValue;
        }
    }
}
