using Blackbox.Firewatch.Application.Common.Exceptions;
using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Common.Pipeline;
using Blackbox.Firewatch.Application.Features.Transactions.Queries.ParseCsv;
using Blackbox.Firewatch.Application.Security;
using FakeItEasy;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Blackbox.Firewatch.Application.UnitTests.Common.Pipeline
{
    [Obsolete]
    public class PersonScopedAuthorizationPreProcessorTests
    {
        private readonly ITestOutputHelper _output;

        public PersonScopedAuthorizationPreProcessorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        //[Fact]
        //public async Task ProcessThrows_WhenRequestorIsNotOwnerOrAdmin()
        //{
        //    var logger = new XUnitLogger<PersonScopedAuthorizationPreProcessor<ParseCsvQuery>>(_output);
        //    var identityService = A.Fake<IIdentityService>();
        //    A.CallTo(() => identityService.IsUserInRole("2345", Roles.Administrator))
        //        .Returns(Task.FromResult(false));
        //    IRequestPreProcessor<ParseCsvQuery> sut = new PersonScopedAuthorizationPreProcessor<ParseCsvQuery>(logger, identityService);
        //    var query = new ParseCsvQuery
        //    {
        //        OwnerId = "1234",
        //        RequestorId = "2345"
        //    };

        //    var exception = await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.Process(query, CancellationToken.None));
        //}

        //[Fact]
        //public async Task ProcessReturnsWhenRequestorIsNotOwnerAndIsAdmin()
        //{
        //    var logger = new XUnitLogger<PersonScopedAuthorizationPreProcessor<ParseCsvQuery>>(_output);
        //    var identityService = A.Fake<IIdentityService>();
        //    A.CallTo(() => identityService.IsUserInRole("2345", Roles.Administrator))
        //        .Returns(Task.FromResult(true));
        //    IRequestPreProcessor<ParseCsvQuery> sut = new PersonScopedAuthorizationPreProcessor<ParseCsvQuery>(logger, identityService);
        //    var query = new ParseCsvQuery
        //    {
        //        OwnerId = "1234",
        //        RequestorId = "2345"
        //    };
            
        //    await sut.Process(query, CancellationToken.None);
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public async Task ProcessReturns(bool isAdmin)
        //{
        //    var logger = new XUnitLogger<PersonScopedAuthorizationPreProcessor<ParseCsvQuery>>(_output);
        //    var identityService = A.Fake<IIdentityService>();
        //    A.CallTo(() => identityService.IsUserInRole("2345", Roles.Administrator))
        //        .Returns(Task.FromResult(isAdmin));
        //    IRequestPreProcessor<ParseCsvQuery> sut = new PersonScopedAuthorizationPreProcessor<ParseCsvQuery>(logger, identityService);
        //    var query = new ParseCsvQuery
        //    {
        //        OwnerId = "1234",
        //        RequestorId = "1234"
        //    };

        //    await sut.Process(query, CancellationToken.None);
        //}
    }
}
