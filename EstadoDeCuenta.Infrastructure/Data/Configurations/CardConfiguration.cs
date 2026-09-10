using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstadoDeCuenta.Domain.Entities;

namespace EstadoDeCuenta.Infrastructure.Data.Configurations
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey(t => t.CardId);

            builder.Property(t => t.CardNumber).IsRequired().HasMaxLength(20);

            builder.Property(t => t.CardLimit).HasPrecision(18, 2);

            builder.HasOne(t => t.Client)
                .WithMany(c => c.Cards)
                .HasForeignKey(t => t.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Movements)
                .WithOne(m => m.Card)
                .HasForeignKey(m => m.CardId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
