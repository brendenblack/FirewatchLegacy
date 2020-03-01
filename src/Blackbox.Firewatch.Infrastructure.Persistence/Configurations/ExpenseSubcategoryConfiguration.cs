using Blackbox.Firewatch.Domain.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Infrastructure.Persistence.Configurations
{
    public class ExpenseSubcategoryConfiguration : IEntityTypeConfiguration<Subcategory>
    {
        public void Configure(EntityTypeBuilder<Subcategory> builder)
        {
            builder.ToTable("expense_subcategories");

            builder.HasOne(sc => sc.ParentCategory)
                .WithMany(c => c.Subcategories)
                .HasForeignKey(sc => sc.ParentCategoryId);
        }
    }
}
