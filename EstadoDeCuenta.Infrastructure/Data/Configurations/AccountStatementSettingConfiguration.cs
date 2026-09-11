using EstadoDeCuenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstadoDeCuenta.Infrastructure.Data.Configurations
{
    public class AccountStatementSettingConfiguration : IEntityTypeConfiguration<AccountStatementSetting>
    {
        public void Configure(EntityTypeBuilder<AccountStatementSetting> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.InterestPercentage).HasPrecision(5, 2);
            builder.Property(s => s.MinimumPaymentPercentage).HasPrecision(5, 2);
            builder.HasData(new AccountStatementSetting
            {
                Id = 1,
                InterestPercentage = 25m,
                MinimumPaymentPercentage = 5m
            });
        }
    }
}
