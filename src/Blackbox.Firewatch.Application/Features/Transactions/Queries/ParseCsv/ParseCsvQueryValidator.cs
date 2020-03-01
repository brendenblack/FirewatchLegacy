using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Domain.Bank;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackbox.Firewatch.Application.Features.Transactions.Queries.ParseCsv
{
    public class ParseCsvQueryValidator : AbstractValidator<ParseCsvQuery>
    {
        public ParseCsvQueryValidator(IEnumerable<ITransactionParser> transactionParsers)
        {
            RuleFor(c => c.FinancialInstitutionAbbreviation)
                .Must(fi => HaveSupportedParser(transactionParsers, fi))
                .WithMessage(i => $"'{i}' is not a supported financial institution. Must be one of {string.Join(',', FinancialInstitution.AllInstitutions.Select(i => i.Abbreviation))}.");
        }

        public bool HaveSupportedParser(IEnumerable<ITransactionParser> transactionParsers, string requestedInstitution)
        {
            return transactionParsers.Any(p => p.SupportedInstitution.Abbreviation == requestedInstitution);
        }
    }
}
