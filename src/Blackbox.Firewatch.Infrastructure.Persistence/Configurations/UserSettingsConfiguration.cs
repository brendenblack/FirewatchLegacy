using Blackbox.Firewatch.Application.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TeixeiraSoftware.Finance;

namespace Blackbox.Firewatch.Infrastructure.Persistence.Configurations
{
    //public class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
    //{
    //    public void Configure(EntityTypeBuilder<UserSettings> builder)
    //    {
    //        builder.ToTable("user_settings");

    //        builder.HasOne(us => us.User)
    //            .WithMany();

    //        builder.Property(us => us.DefaultCurrency)
    //            .HasConversion(
    //                model => model.AlphabeticCode,
    //                db => Currency.ByAlphabeticCode(db));


    //    }
    //}
}
