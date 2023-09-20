using Tickets.Core.Aggregates.MovieSessionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tickets.Infrastructure.Data.Config;

public class SeatConfiguration : IEntityTypeConfiguration<SessionSeat>
{
    public void Configure(EntityTypeBuilder<SessionSeat> builder)
    {
        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.Status).IsRequired();
    }
}