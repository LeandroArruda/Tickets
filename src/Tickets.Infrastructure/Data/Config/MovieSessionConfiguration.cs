using Tickets.Core.Aggregates.MovieSessionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tickets.Infrastructure.Data.Config;

public class MovieSessionConfiguration : IEntityTypeConfiguration<MovieSession>
{
    public void Configure(EntityTypeBuilder<MovieSession> config)
    {
        config.Property<Guid>(p => p.RoomId).IsRequired();
        config.Property<Guid>(p => p.MovieId).IsRequired();
        config.Property<DateTime>(p => p.StartDateTime).IsRequired();
        config.Property<DateTime>(p => p.EndDateTime).IsRequired();

        var navigation = config.Metadata.FindNavigation(nameof(MovieSession.SessionSeats));
    }
}
