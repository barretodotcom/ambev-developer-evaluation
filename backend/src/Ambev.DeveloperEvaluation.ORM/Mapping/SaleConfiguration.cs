using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");
        
        builder.HasKey(sale => sale.Id);
        builder.Property(s => s.SaleNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.CustomerId)
            .IsRequired();

        builder.Property(s => s.CustomerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.BranchId)
            .IsRequired();

        builder.Property(s => s.BranchName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Status)
            .HasConversion<int>()
            .IsRequired()
            .HasSentinel(SaleStatus.None);

        builder.Property(s => s.SaleDate)
            .IsRequired();

        builder.Property(s => s.UpdatedAt);

        builder.OwnsMany(s => s.Items, sa =>
        {
            sa.WithOwner().HasForeignKey("SaleId");
            sa.HasKey(l => l.Id);
            sa.Property(l => l.Id);

            sa.Property(l => l.Status)
                .HasSentinel(SaleItemStatus.None)
                .HasConversion<int>()
                .IsRequired();
            
            sa.Property(i => i.ProductId).IsRequired();
            sa.Property(i => i.ProductName).IsRequired().HasMaxLength(200);
            sa.Property(i => i.Quantity).IsRequired();
            
            sa.Property(i => i.UnitPrice)
                .HasConversion(
                dest => dest.Value,
                src => Money.Create(src)
                )
                .IsRequired();
            
            sa.Property(i => i.DiscountPercentage)
                .HasConversion(
                    dest => dest.Value,
                    src => Percentage.Create(src)
                    )
                .IsRequired();
                
            sa.ToTable("SaleItems");
        });
    }
}