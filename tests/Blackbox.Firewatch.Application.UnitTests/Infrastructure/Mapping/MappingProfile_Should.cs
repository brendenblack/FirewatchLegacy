using AutoMapper;
using Blackbox.Firewatch.Domain.Bank;
using Blackbox.Firewatch.Domain.Expenses;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Blackbox.Firewatch.Application.UnitTests.Infrastructure.Mapping
{
    public class MappingProfile_Should : IClassFixture<MappingTestsFixture>
    {
        private readonly IConfigurationProvider _configuration; 
        private readonly IMapper _mapper;

        public MappingProfile_Should(ITestOutputHelper output, MappingTestsFixture fixture)
        {
            _configuration = fixture.ConfigurationProvider;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Theory]
        [InlineData(typeof(Transaction), typeof(Application.Features.Transactions.Queries.ParseCsv.TransactionModel))]
        [InlineData(typeof(Transaction), typeof(Application.Features.Transactions.Queries.FetchTransactions.FetchTransactionModel))]
        [InlineData(typeof(Category), typeof(Application.Features.Expenses.Queries.GetCategories.CategoryModel))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }
    }
}
