using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Domain.Bank;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Application.Features.Accounts.Commands.AddAccount
{
    public class AddAccountCommand : IRequest<Result<int>>
    {
        public string PersonId { get; set; }

        public string AccountNumber { get; set; }

        public string FinancialInstitutionAbbreviation { get; set; }
        
        public string Nickname { get; set; }

        public class AddAccountHandler : IRequestHandler<AddAccountCommand, Result<int>>
        {
            private readonly ILogger<AddAccountHandler> _logger;
            private readonly IBankContext _bankContext;

            public AddAccountHandler(ILogger<AddAccountHandler> logger, IBankContext bankContext)
            {
                _logger = logger;
                _bankContext = bankContext;
            }

            public async Task<Result<int>> Handle(AddAccountCommand request, CancellationToken cancellationToken)
            {
                var account = new Account()
                {
                    OwnerId = request.PersonId,
                    AccountNumber = request.AccountNumber,
                    Institution = FinancialInstitution.FromAbbreviation(request.FinancialInstitutionAbbreviation),
                    DisplayName = request.Nickname,
                };

                _bankContext.Accounts.Add(account);

                await _bankContext.SaveChangesAsync();

                _logger.LogInformation("Account {} was created with id {} for user {}",
                    account.AccountNumber,
                    account.Id,
                    account.OwnerId);

                return Result.Ok(account.Id);
            }
        }
    }
}
