using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Application.Features.Transactions.Commands.AddTransactions
{
    public class AddTransactionsCommand : PersonScopedAuthorizationRequiredRequest, IRequest<Result<AddTransactionsResponse>>
    {
        public List<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();

        public override string[] AuthorizedRoles => new string[] { Roles.User };

        public bool IsCreateAccounts { get; set; } = true;

        public class AddTransactionsHandler : IRequestHandler<AddTransactionsCommand, Result<AddTransactionsResponse>>
        {
            private readonly ILogger<AddTransactionsHandler> logger;
            private readonly IBankContext bankContext;

            public AddTransactionsHandler(
                ILogger<AddTransactionsHandler> logger,
                IBankContext bankContext)
            {
                this.logger = logger;
                this.bankContext = bankContext;
            }

            public async Task<Result<AddTransactionsResponse>> Handle(AddTransactionsCommand request, CancellationToken cancellationToken)
            {
                var accounts = await bankContext.Accounts
                    .Where(a => a.OwnerId == request.RequestorId)
                    .ToListAsync();


                throw new NotImplementedException();
            }
        }
    }
}
