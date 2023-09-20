using Tickets.Core.Aggregates.MovieSessionAggregate.Events;
using Tickets.SharedKernel;
using Tickets.SharedKernel.Interfaces;

namespace Tickets.Core.Aggregates.MovieSessionAggregate
{
    public class MovieSession : EntityBase, IAggregateRoot
    {
        public Guid RoomId { get; private set; }
        public Guid MovieId { get; private set; }
        public DateTime StartDateTime { get; private set; }
        public DateTime EndDateTime { get; private set; }
        public int BreakDuration { get; private set; }
        private readonly List<string> _advertisements;
        public MovieSessionStatusEnum Status => _sessionSeats.All(s => s.Status == SessionSeatStatusEnum.Sold) ? MovieSessionStatusEnum.SoldOut : MovieSessionStatusEnum.Active;
        public IReadOnlyCollection<string> Advertisements => _advertisements;
        private readonly List<SessionSeat> _sessionSeats;
        public IReadOnlyCollection<SessionSeat> SessionSeats => _sessionSeats;

        protected MovieSession()
        {
            _advertisements = new List<string>();
            _sessionSeats = new List<SessionSeat>();
        }

        public MovieSession(Guid roomId,
                            Guid movieId,
                            DateTime startDateTime,
                            DateTime endDateTime,
                            int breakDuration,
                            List<string> advertisements,
                            List<SessionSeat> sessionSeats)
        {
            RoomId = roomId;
            MovieId = movieId;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            BreakDuration = breakDuration;
            _advertisements = advertisements;
            _sessionSeats = sessionSeats;
        }

        public void MarkSeatsAsSold(IEnumerable<Guid> sessionSeatIds)
        {
            List<SessionSeat> setsToUpdate = _sessionSeats.Where(s => sessionSeatIds.Any(x => s.Id == x)).ToList();

            setsToUpdate.ForEach(s =>
            {
                s.MarkAsSold();
            });

            if (IsSoldOut())
            {
                RegisterDomainEvent(new MovieSessionSoldOutEvent(Id));
            }
        }

        public void ReserveASeat(Guid sessionSeatId)
        {
            var seat = _sessionSeats.Find(s => s.Id == sessionSeatId);

            if (seat is null)
            {
                throw new Exception("Seat not found");
            }

            seat.MarkAsReserved();
        }

        public bool IsSoldOut()
        {
            return _sessionSeats.All(s => s.Status == SessionSeatStatusEnum.Sold);
        }
    }
}
