using Ambev.DeveloperEvaluation.Messaging.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public sealed class OutboxConfiguration : IEntityTypeConfiguration<OutboxEntity>
{
    public void Configure(EntityTypeBuilder<OutboxEntity> builder)
    {
        builder.ToTable("outbox");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(o => o.Type)
            .HasColumnName("type")
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(o => o.Payload)
            .HasColumnName("payload")
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(o => o.OccurredOn)
            .HasColumnName("occurred_on")
            .IsRequired();

        builder.Property(o => o.ProcessedOn)
            .HasColumnName("processed_on");

        builder.HasIndex(o => new { o.OccurredOn, o.ProcessedOn })
            .HasDatabaseName("idx_outbox_unprocessed")
            .HasFilter("processed_on IS NULL")
            .IncludeProperties(o => new { o.Id, o.Type, o.Payload });
    }
}