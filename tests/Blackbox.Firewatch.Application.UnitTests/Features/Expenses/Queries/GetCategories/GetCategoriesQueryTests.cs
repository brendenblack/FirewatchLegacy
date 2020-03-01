using Blackbox.Firewatch.Application.Features.Expenses.Queries.GetCategories;
using Blackbox.Firewatch.Domain;
using Blackbox.Firewatch.Domain.Expenses;
using Blackbox.Firewatch.Infrastructure.Persistence;
using FakeItEasy;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Blackbox.Firewatch.Application.Features.Expenses.Queries.GetCategories.GetCategoriesQuery;

namespace Blackbox.Firewatch.Application.UnitTests.Features.Expenses.Queries.GetCategories
{
    [Collection("QueryTests")]
    public class GetCategoriesQueryTests
    {
        private readonly ITestOutputHelper _output;
        private readonly QueryTestFixture _fixture;

        public GetCategoriesQueryTests(ITestOutputHelper output, QueryTestFixture fixture)
        {
            this._output = output;
            this._fixture = fixture;
        }

        [Fact]
        public async Task Handle_ReturnsSystemValues()
        {
            var logger = new XUnitLogger<GetCategoriesHandler>(_output);
            var existingUser = new Person { Email = "testuser@firewatch.com" };
            var systemCategories = new List<Category>
            {
                new Category { Label = "category1", IsSystemDefault = true },
                new Category { Label = "category2", IsSystemDefault = true },
                new Category { Label = "category3", IsSystemDefault = false },
            };
            _fixture.Context.People.Add(existingUser);
            _fixture.Context.ExpenseCategories.AddRange(systemCategories);
            await _fixture.Context.SaveChangesAsync();
            
            var sut = new GetCategoriesHandler(logger, _fixture.Context, _fixture.Mapper);
            var query = new GetCategoriesQuery 
            { 
                OwnerId = existingUser.Id, 
                RequestorId = existingUser.Id 
            };

            var result = await sut.Handle(query, CancellationToken.None);

            result.Categories.Any(c => c.Label == "category1").ShouldBeTrue();
            result.Categories.Any(c => c.Label == "category2").ShouldBeTrue();

        }
    }
}
