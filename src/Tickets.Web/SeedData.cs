using Tickets.Core.Aggregates.MovieSessionAggregate;
using Tickets.Core.Aggregates.TicketOrderAggregate;
using Tickets.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Tickets.Web;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var dbContext = new AppDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null))
        {
            if (dbContext.MovieSessions.Any())
            {
                return;   // DB has been seeded
            }

            PopulateTestData(dbContext);
        }
    }

    public static void PopulateTestData(AppDbContext dbContext)
    {
        foreach (var item in dbContext.MovieSessions)
        {
            dbContext.Remove(item);
        }
        foreach (var item in dbContext.Seats)
        {
            dbContext.Remove(item);
        }
        foreach (var item in dbContext.TicketOrders)
        {
            dbContext.Remove(item);
        }
        foreach (var item in dbContext.Tickets)
        {
            dbContext.Remove(item);
        }

        var movieSession1Seat1 = new SessionSeat(1, "a");
        var movieSession1Seat2 = new SessionSeat(1, "b");
        var movieSession1Seat3 = new SessionSeat(2, "a");
        var movieSession1Seat4 = new SessionSeat(2, "b");
        var movieSession1Seat5 = new SessionSeat(3, "a");

        MovieSession movieSession1 = new MovieSession(Guid.NewGuid(), Guid.NewGuid(), DateTime.Today.AddDays(-1), DateTime.Now.AddDays(20), 0, new List<string>(), new List<SessionSeat>()
        {
          movieSession1Seat1,
          movieSession1Seat2,
          movieSession1Seat3,
          movieSession1Seat4,
          movieSession1Seat5
         });

        var session2Seat1 = new SessionSeat(1, "a");
        var session2Seat2 = new SessionSeat(1, "b");
        var session2Seat3 = new SessionSeat(2, "a");
        var session2Seat4 = new SessionSeat(2, "b");

        MovieSession movieSession2 = new MovieSession(Guid.NewGuid(), Guid.NewGuid(), DateTime.Today.AddDays(-10), DateTime.Now.AddDays(10), 0, new List<string>(), new List<SessionSeat>()
        {
          session2Seat1,
          session2Seat2,
          session2Seat3,
          session2Seat4
        });

        dbContext.MovieSessions.Add(movieSession1);
        dbContext.MovieSessions.Add(movieSession2);

        dbContext.SaveChanges();

        //Reserve almost all the seats 
        movieSession1.ReserveASeat(movieSession1Seat1.Id);
        movieSession1.ReserveASeat(movieSession1Seat2.Id);
        movieSession1.ReserveASeat(movieSession1Seat3.Id);
        movieSession1.ReserveASeat(movieSession1Seat4.Id);

        movieSession2.ReserveASeat(session2Seat1.Id);
        movieSession2.ReserveASeat(session2Seat2.Id);
        movieSession2.ReserveASeat(session2Seat3.Id);
        movieSession2.ReserveASeat(session2Seat4.Id);

        //Sell almost all the seats
        var movieSession1Ticket1 = new Ticket(movieSession1.SessionSeats.ToArray()[0].Id, 10);
        var movieSession1Ticket2 = new Ticket(movieSession1.SessionSeats.ToArray()[1].Id, 15);
        var MovieSession1Ticket3 = new Ticket(movieSession1.SessionSeats.ToArray()[2].Id, 10);
        var MovieSession1Ticket4 = new Ticket(movieSession1.SessionSeats.ToArray()[3].Id, 10);

        dbContext.TicketOrders.Add(new TicketOrder(movieSession1.Id, Guid.NewGuid(), DateTime.Now, new List<Ticket>()
        {
          movieSession1Ticket1,
          movieSession1Ticket2,
          MovieSession1Ticket3,
          MovieSession1Ticket4
        }));

        movieSession1.MarkSeatsAsSold(new[] { movieSession1Ticket1.SessionSeatId, movieSession1Ticket2.SessionSeatId, MovieSession1Ticket3.SessionSeatId, MovieSession1Ticket4.SessionSeatId });

        var movieSession2Ticket1 = new Ticket(movieSession2.SessionSeats.ToArray()[0].Id, 20);
        var movieSession2Ticket2 = new Ticket(movieSession2.SessionSeats.ToArray()[1].Id, 22);
        var movieSession2Ticket3 = new Ticket(movieSession2.SessionSeats.ToArray()[2].Id, 20);
        var movieSession2Ticket4 = new Ticket(movieSession2.SessionSeats.ToArray()[3].Id, 10);

        dbContext.TicketOrders.Add(new TicketOrder(movieSession2.Id, Guid.NewGuid(), DateTime.Now, new List<Ticket>()
        {
            movieSession2Ticket1,
            movieSession2Ticket2,
            movieSession2Ticket3,
            movieSession2Ticket4,
        }));

        movieSession2.MarkSeatsAsSold(new[] { movieSession2Ticket1.SessionSeatId, movieSession2Ticket2.SessionSeatId, movieSession2Ticket3.SessionSeatId, movieSession2Ticket4.SessionSeatId });

        //At this point, just the Session seat 3a on Movie session 1 should be available

        dbContext.SaveChanges();
    }
}