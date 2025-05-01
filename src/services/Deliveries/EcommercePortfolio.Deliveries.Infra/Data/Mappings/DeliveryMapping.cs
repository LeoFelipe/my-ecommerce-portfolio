using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EcommercePortfolio.Deliveries.Domain.Entities;

namespace EcommercePortfolio.Deliveries.Infra.Data.Mappings;

public class DeliveryMapping : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.ToTable("delivery");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.OrderId)
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(c => c.ClientId)
            .HasColumnName("client_id")
            .IsRequired();

        builder.Property(c => c.ExpectedDate)
            .HasColumnName("expected_date")
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(c => c.DateMade)
            .HasColumnName("date_made")
            .HasColumnType("timestamp");

        builder.Property(c => c.DeliveryStatus)
            .HasColumnName("delivery_status")
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
                .HasColumnName("zip_code")
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
    }
}