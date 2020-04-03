using Blackbox.Firewatch.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Infrastructure.Persistence.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("people")
                .HasKey(p => p.Id);

            builder.HasMany(p => p.Accounts)
                .WithOne()
                .HasForeignKey(a => a.OwnerId);
        }
    }
}
