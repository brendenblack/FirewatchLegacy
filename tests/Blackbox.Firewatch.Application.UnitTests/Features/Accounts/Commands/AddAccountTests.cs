using Blackbox.Firewatch.Application.Features.Accounts.Commands.AddAccount;
using Blackbox.Firewatch.Domain;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Blackbox.Firewatch.Application.Features.Accounts.Commands.AddAccount.AddAccountCommand;

namespace Blackbox.Firewatch.Application.UnitTests.Features.Accounts.Commands
{
    public class AddAccountTests : CommandTestBase
    {
        private readonly ITestOutputHelper _output;

        public AddAccountTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task HandleShould_ReturnIdOfCreatedAccount()
        {
            var person = new Person { Id = Guid.NewGuid().ToString() };
            base.Context.People.Add(person);
            await base.Context.SaveChangesAsync();
            var sut = new AddAccountHandler(new XUnitLogger<AddAccountHandler>(_output), base.Context);
            var request = new AddAccountCommand
            {
                PersonId = person.Id,
                AccountNumber = "abcd5",
                FinancialInstitutionAbbreviation = "RBC",
            };

            var response = await sut.Handle(request, CancellationToken.None);

            response.IsSuccess.ShouldBeTrue();
            response.Value.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task HandleShould_PersistAccount()
        {
            var person = new Person { Id = Guid.NewGuid().ToString() };
            base.Context.People.Add(person);
            await base.Context.SaveChangesAsync();
            var sut = new AddAccountHandler(new XUnitLogger<AddAccountHandler>(_output), base.Context);
            var request = new AddAccountCommand
            {
                PersonId = person.Id,
                AccountNumber = "abcd5",
                FinancialInstitutionAbbreviation = "RBC",
            };

            var response = await sut.Handle(request, CancellationToken.None);

            base.Context.Accounts.Any(a => a.Id == response.Value).ShouldBeTrue();
        }

    }
}
