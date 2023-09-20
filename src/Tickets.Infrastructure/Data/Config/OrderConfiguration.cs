using Tickets.Core.Aggregates.TicketOrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tickets.Infrastructure.Data.Config;

public class OrderConfiguration : IEntityTypeConfiguration<TicketOrder>
{
    public void Configure(EntityTypeBuilder<TicketOrder> builder)
    {
        builder.Property<Guid>(p => p.MovieSessionId).IsRequired();
        builder.Property<Guid>(p => p.BuyerId).IsRequired();
        builder.Property<DateTime>(p => p.TicketOrderDateAndTime).IsRequired();
    }
}