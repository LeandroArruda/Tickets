using Tickets.Core.Aggregates.TicketOrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tickets.Infrastructure.Data.Config;

public class OrderTicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.Property<Guid>(p => p.Id).IsRequired();
        builder.Property<int>(p => p.Value).IsRequired();
    }
}