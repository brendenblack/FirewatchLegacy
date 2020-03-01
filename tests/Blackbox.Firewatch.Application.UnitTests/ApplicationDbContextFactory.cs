using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Infrastructure.Persistence;
using FakeItEasy;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.UnitTests
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This class requires that we create a dependency to the <see cref="Firewatch.Infrastructure.Persistence"/>
    /// project so that we can access <see cref="ApplicationDbContext"/>. This is not ideal, but does provide
    /// a huge convenience when it comes to creating tests.
    /// </remarks>
    public static class ApplicationDbContextFactory
    {
        public static ApplicationDbContext Create(string id = "")
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id)
                .Options;

            var operationStoreOptions = Options.Create(
                new OperationalStoreOptions
                {
                    DeviceFlowCodes = new TableConfiguration("DeviceCodes"),
                    PersistedGrants = new TableConfiguration("PersistedGrants")
                });

            var dateTimeMock = A.Fake<IDateTime>();
            A.CallTo(() => dateTimeMock.Now)
                .Returns(new DateTime(3001, 1, 1));

            var currentUserServiceMock = A.Fake<ICurrentUserService>();
            A.CallTo(() => currentUserServiceMock.UserId)
                .Returns("00000000-0000-0000-0000-000000000000");

            var context = new ApplicationDbContext(options, operationStoreOptions, currentUserServiceMock, dateTimeMock);

            SeedSampleData(context);

            return context;
        }

        public static void SeedSampleData(ApplicationDbContext context)
        {

        }

        public static void Destroy(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}
