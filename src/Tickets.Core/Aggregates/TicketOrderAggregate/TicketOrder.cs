using Tickets.Core.Aggregates.TicketOrderAggregate.Events;
using Tickets.SharedKernel;
using Tickets.SharedKernel.Interfaces;

namespace Tickets.Core.Aggregates.TicketOrderAggregate
{
    public class TicketOrder : EntityBase, IAggregateRoot
    {
        public new int Id { get; private set; }
        public Guid MovieSessionId { get; private set; }
        public Guid BuyerId { get; private set; }
        public DateTime TicketOrderDateAndTime { get; private set; }
        private readonly List<Ticket> _tickets;
        public IReadOnlyCollection<Ticket> Tickets => _tickets;
        public int TotalValue => _tickets.Sum(t => t.Value);

        protected TicketOrder()
        {
            _tickets = new List<Ticket>();
        }

        public TicketOrder(Guid movieSessionId, Guid buyerId, DateTime orderDateAndTime, List<Ticket> tickets)
        {
            MovieSessionId = movieSessionId;
            BuyerId = buyerId;
            TicketOrderDateAndTime = orderDateAndTime;
            _tickets = tickets;

            RegisterDomainEvent(new TicketOrderCreatedEvent(Id, MovieSessionId, _tickets.Select(x => x.SessionSeatId).ToList(), TotalValue));
        }
    }
}
