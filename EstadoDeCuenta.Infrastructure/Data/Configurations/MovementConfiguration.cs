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
    public class MovementConfiguration : IEntityTypeConfiguration<Movement>
    {
        public void Configure(EntityTypeBuilder<Movement> builder)
        {
            builder.HasKey(m => m.MovementId);

            builder.Property(m => m.MovementAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(m => m.MovementDate).IsRequired();

            builder.Property(m => m.MovementType).IsRequired();

            builder.HasOne(m => m.Card)
                .WithMany(t => t.Movements)
                .HasForeignKey(m => m.CardId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
