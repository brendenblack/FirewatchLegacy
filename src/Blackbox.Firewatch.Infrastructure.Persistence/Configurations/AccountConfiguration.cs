using Blackbox.Firewatch.Domain.Bank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : AuditableEntityConfiguration<Account> //IEntityTypeConfiguration<Account>
    {
        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            base.Configure(builder);

            builder.ToTable("bank_accounts");

            builder.HasDiscriminator(a => a.AccountType)
                .HasValue<VisaAccount>(AccountTypes.VISA)
                .HasValue<Account>(AccountTypes.OTHER);

            builder.HasOne(a => a.Owner)
                .WithMany(p => p.Accounts)
                .HasForeignKey(a => a.OwnerId);

            builder.Property(a => a.Institution)
                .HasConversion(
                v => v.Abbreviation,
                v => FinancialInstitution.FromAbbreviation(v));

            //builder.HasOne(a => a.Institution)
            //    .WithMany();

            builder.HasMany(a => a.Transactions)
                .WithOne();

            //base.Configure(builder);
        }

    }
}
