using Blackbox.Firewatch.Domain.Bank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Infrastructure.Persistence.Configurations
{
    public class FinancialInstitutionConfiguration : IEntityTypeConfiguration<FinancialInstitution>
    {
        public void Configure(EntityTypeBuilder<FinancialInstitution> builder)
        {
            builder.HasNoKey();
        }
    }
}
