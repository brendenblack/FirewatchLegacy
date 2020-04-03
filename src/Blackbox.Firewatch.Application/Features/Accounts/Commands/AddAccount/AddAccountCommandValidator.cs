using Blackbox.Firewatch.Application.Common.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackbox.Firewatch.Application.Features.Accounts.Commands.AddAccount
{
    public class AddAccountCommandValidator : AbstractValidator<AddAccountCommand>
    {
        private readonly IBankContext _bankContext;

        public AddAccountCommandValidator(IBankContext bankContext)
        {
            _bankContext = bankContext;

            RuleFor(c => c.AccountNumber)
                .Must((command, number) => !_bankContext.Accounts
                    .Where(a => a.AccountNumber == command.AccountNumber)
                    .Where(a => a.OwnerId == command.PersonId)
                    .Any())
                .WithMessage("An account with that number already exists.");
        }
    }
}
