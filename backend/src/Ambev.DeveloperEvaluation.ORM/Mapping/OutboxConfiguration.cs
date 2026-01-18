using Ambev.DeveloperEvaluation.Messaging.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public sealed class OutboxConfiguration : IEntityTypeConfiguration<Outbox>
{
    public void Configure(EntityTypeBuilder<Outbox> builder)
    {
        builder.ToTable("Outbox");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(l => l.Type)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(l => l.Payload)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(l => l.OccurredOn)
            .IsRequired();

        builder.Property(l => l.ProcessedOn);

        builder.HasIndex(l => new { l.OccurredOn, l.ProcessedOn })
            .HasDatabaseName("idx_outbox_messages_unprocessed")
            .HasFilter("\"ProcessedOn\" IS NULL")
            .IncludeProperties(l => new { l.Id, l.Type, l.Payload });
    }
}