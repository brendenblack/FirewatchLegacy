using Blackbox.Firewatch.Domain.Bank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("bank_accounts");

            builder.HasOne(a => a.Owner)
                .WithMany()
                .HasForeignKey(a => a.OwnerId);

            builder.Property(a => a.Institution)
                .HasConversion(
                v => v.Abbreviation,
                v => FinancialInstitution.FromAbbreviation(v));

            builder.HasMany(a => a.Transactions)
                .WithOne();
        }

    }
}
