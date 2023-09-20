using System.Reflection;
using Tickets.Core.Aggregates.MovieSessionAggregate;
using Tickets.Core.Aggregates.TicketOrderAggregate;
using Tickets.SharedKernel;
using Tickets.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Tickets.Infrastructure.Data;

public class AppDbContext : DbContext
{
  private readonly IDomainEventDispatcher? _dispatcher;

  public AppDbContext(DbContextOptions<AppDbContext> options,
    IDomainEventDispatcher? dispatcher)
      : base(options)
  {
    _dispatcher = dispatcher;
  }

  public DbSet<MovieSession> MovieSessions => Set<MovieSession>();
  public DbSet<SessionSeat> Seats => Set<SessionSeat>();
  public DbSet<TicketOrder> TicketOrders => Set<TicketOrder>();
  public DbSet<Ticket> Tickets => Set<Ticket>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    // ignore events if no dispatcher provided
    if (_dispatcher == null) return result;

    // dispatch events only if save was successful
    var entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
        .Select(e => e.Entity)
        .Where(e => e.DomainEvents.Any())
        .ToArray();

    await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }

  public override int SaveChanges()
  {
    return SaveChangesAsync().GetAwaiter().GetResult();
  }
}
