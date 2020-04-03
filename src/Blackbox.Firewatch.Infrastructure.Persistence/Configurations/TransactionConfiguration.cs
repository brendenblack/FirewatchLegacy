using Blackbox.Firewatch.Domain.Bank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TeixeiraSoftware.Finance;

namespace Blackbox.Firewatch.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("transactions")
                .HasKey(tx => tx.Id);

            builder.Property(t => t.Descriptions)
                .HasConversion(
                    model => JsonConvert.SerializeObject(model, Formatting.None),
                    db => JsonConvert.DeserializeObject<List<string>>(db));

            builder.Property(t => t.Currency)
                .HasConversion(
                    model => model.AlphabeticCode,
                    db => Currency.ByAlphabeticCode(db));

            ValueComparer<IList<string>> descriptionComparer = new ValueComparer<IList<string>>(
                (a, b) => a.Equals(b),
                c => c.GetHashCode());

            builder.Property(t => t.Descriptions)
                .HasConversion(
                    model => JsonConvert.SerializeObject(model, Formatting.None),
                    db => JsonConvert.DeserializeObject<List<string>>(db))
                .Metadata.SetValueComparer(descriptionComparer);

            builder.Property(t => t.Amount)
                .HasColumnType("decimal(30,2)");

            builder.HasOne(t => t.Account)
                .WithMany();
                //.HasForeignKey(t => t.AccountId);
        }
    }
}
