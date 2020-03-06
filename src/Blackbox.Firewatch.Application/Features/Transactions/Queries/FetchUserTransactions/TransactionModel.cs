using AutoMapper;
using Blackbox.Firewatch.Application.Infrastructure.Mapping;
using Blackbox.Firewatch.Domain.Bank;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackbox.Firewatch.Application.Features.Transactions.Queries.FetchUserTransactions
{
    public class TransactionModel : IMapFrom<Transaction>
    {
        public DateTime Date { get; set; }

        public IList<string> Descriptions { get; set; } = new List<string>();

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string AccountNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Transaction, TransactionModel>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(s => s.Currency.AlphabeticCode))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(s => s.Account.AccountNumber));
        }
    }
}
