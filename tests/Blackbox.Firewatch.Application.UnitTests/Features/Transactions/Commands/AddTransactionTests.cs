using Blackbox.Firewatch.Application.Features.Transactions.Commands.AddTransactions;
using Blackbox.Firewatch.Domain;
using Blackbox.Firewatch.Domain.Bank;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Blackbox.Firewatch.Application.Features.Transactions.Commands.AddTransactions.AddTransactionsCommand;

namespace Blackbox.Firewatch.Application.UnitTests.Features.Transactions.Commands
{
    public class AddTransactionTests : CommandTestBase
    {
        private readonly ITestOutputHelper _output;

        public AddTransactionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenAccountsExist()
        {
            var person = new Person { Id = Guid.NewGuid().ToString() };
            base.Context.People.Add(person);
            await base.Context.SaveChangesAsync();
            var account = new Account
            {
                Owner = person,
                OwnerId = person.Id,
                AccountNumber = "12345",
                Institution = FinancialInstitution.RoyalBank
            };
            base.Context.Accounts.AddRange(account);
            await base.Context.SaveChangesAsync();
            var logger = new XUnitLogger<AddTransactionsHandler>(_output);
            var sut = new AddTransactionsHandler(logger, base.Context);
            var command = new AddTransactionsCommand
            {
                PersonId = person.Id,
                Transactions = new List<AddTransactionModel>
                {
                    new AddTransactionModel
                    {
                        Amount = 100,
                        Currency = "CAD",
                        Date = DateTime.Now,
                        AccountNumber = account.AccountNumber,
                    }
                }
            };

            var result = await sut.Handle(command, CancellationToken.None);

            result.IsSuccess.ShouldBe(true);
            result.Value.CreatedIds.Count.ShouldBe(1);
        }

        [Fact]
        public async Task Handle_ShouldPersist()
        {
            var person = new Person { Id = Guid.NewGuid().ToString() };
            base.Context.People.Add(person);
            await base.Context.SaveChangesAsync();
            var account = new Account
            {
                Owner = person,
                OwnerId = person.Id,
                AccountNumber = "12345",
                Institution = FinancialInstitution.RoyalBank
            };
            base.Context.Accounts.AddRange(account);
            await base.Context.SaveChangesAsync();
            var logger = new XUnitLogger<AddTransactionsHandler>(_output);
            var sut = new AddTransactionsHandler(logger, base.Context);
            var command = new AddTransactionsCommand
            {
                PersonId = person.Id,
                Transactions = new List<AddTransactionModel>
                {
                    new AddTransactionModel
                    {
                        Amount = 100,
                        Currency = "CAD",
                        Date = DateTime.Now,
                        AccountNumber = account.AccountNumber,
                    }
                }
            };

            var result = await sut.Handle(command, CancellationToken.None);

            result.Value.CreatedIds.ShouldNotBeEmpty();
            foreach (var id in result.Value.CreatedIds)
            {
                base.Context.Transactions.Any(tx => tx.Id == id).ShouldBeTrue();
            }
        }
    }
}
