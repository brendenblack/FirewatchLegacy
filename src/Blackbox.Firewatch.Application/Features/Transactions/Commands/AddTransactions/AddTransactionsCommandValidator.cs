using Blackbox.Firewatch.Application.Common.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackbox.Firewatch.Application.Features.Transactions.Commands.AddTransactions
{
    public class AddTransactionsCommandValidator : AbstractValidator<AddTransactionsCommand>
    {
        public AddTransactionsCommandValidator(IBankContext bankContext)
        {
            RuleFor(c => c.PersonId)
                .Must(id => bankContext.People.Any(p => p.Id == id))
                .WithMessage(id => $"No person exists with specified id {id}.");


            RuleForEach(c => c.Transactions)
                .SetValidator(new TransactionModelValidator());
        }
    }
}
