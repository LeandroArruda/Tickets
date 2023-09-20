using Tickets.SharedKernel;

namespace Tickets.Core.Aggregates.TicketOrderAggregate
{
    public class Ticket : EntityBase
    {
        public Guid SessionSeatId { get; internal set; }
        public int Value { get; internal set; }

        public Ticket(Guid sessionSeatId, int value)
        {
            SessionSeatId = sessionSeatId;
            Value = value;
        }
    }
}
