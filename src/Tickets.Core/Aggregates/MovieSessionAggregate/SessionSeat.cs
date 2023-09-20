using Tickets.SharedKernel;

namespace Tickets.Core.Aggregates.MovieSessionAggregate
{
    public class SessionSeat : EntityBase
    {
        public int LineId { get; private set; }
        public string RowId { get; private set; }
        public SessionSeatStatusEnum Status { get; private set; }
        public DateTime? ReservedDateTime { get; private set; }

        public SessionSeat(int lineId, string rowId)
        {
            Status = SessionSeatStatusEnum.Available;
            LineId = lineId;
            RowId = rowId;
        }

        internal void MarkAsReserved()
        {
            Status = SessionSeatStatusEnum.Reserved;
            ReservedDateTime = DateTime.Now;
        }

        internal void MarkAsSold()
        {
            Status = SessionSeatStatusEnum.Sold;
        }

        internal bool IsAvailable()
        {
            return Status == SessionSeatStatusEnum.Available;
        }

        internal void SetReservationDateTime()
        {
            ReservedDateTime = DateTime.Now;
        }
    }
}