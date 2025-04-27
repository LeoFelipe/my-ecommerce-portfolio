using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EcommercePortfolio.Orders.Domain.Entities;

namespace EcommercePortfolio.Orders.Infra.Data.Mappings;

public class OrderMapping : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("order");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.PaymentId)
            .HasColumnName("payment_id")
            .IsRequired();

        builder.Property(c => c.ClientId)
            .HasColumnName("client_id")
            .IsRequired();

        builder.Property(c => c.TotalValue)
            .HasColumnName("total_value")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(c => c.OrderStatus)
            .HasColumnName("order_status")
            .HasColumnType("varchar(50)")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp")
            .IsRequired();

        builder.OwnsOne(p => p.Address, e =>
        {
            e.Property(pe => pe.ZipCode)
                .HasColumnName("zipCode")
                .HasColumnType("varchar(20)")
                .IsRequired();

            e.Property(pe => pe.State)
                .HasColumnName("state")
                .HasColumnType("varchar(50)")
                .IsRequired();

            e.Property(pe => pe.City)
                .HasColumnName("city")
                .HasColumnType("varchar(50)")
                .IsRequired();

            e.Property(pe => pe.StreetAddress)
                .HasColumnName("street_address")
                .HasColumnType("varchar(100)")
                .IsRequired();

            e.Property(pe => pe.NumberAddres)
                .HasColumnName("number_addres")
                .HasColumnType("int")
                .IsRequired();
        });

        builder.HasMany(c => c.OrderItems)
            .WithOne(c => c.Order)
            .HasForeignKey(c => c.OrderId);
    }
}
