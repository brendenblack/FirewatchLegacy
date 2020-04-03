using Blackbox.Firewatch.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Application.Features.Accounts.Queries.GetAccountSummaries
{
    public class GetAccountSummariesQuery : IRequest<GetAccountSummariesResponse>
    {
        public string PersonId { get; set; }

        public class GetAccountSummariesHandler : IRequestHandler<GetAccountSummariesQuery, GetAccountSummariesResponse>
        {
            private readonly ILogger<GetAccountSummariesHandler> _logger;
            private readonly IBankContext _bankContext;

            public GetAccountSummariesHandler(ILogger<GetAccountSummariesHandler> logger, IBankContext bankContext)
            {
                _logger = logger;
                _bankContext = bankContext;
            }

            public async Task<GetAccountSummariesResponse> Handle(GetAccountSummariesQuery request, CancellationToken cancellationToken)
            {
                var models = await _bankContext.Accounts
                    .Where(a => a.OwnerId == request.PersonId)
                    .Include(a => a.Transactions)
                    .Select(a => new AccountSummaryModel
                    {
                        AccountNumber = a.AccountNumber,
                        FinancialInstitution = a.Institution.Abbreviation,
                        Transactions = a.Transactions.Count,
                        EarliestTransaction = (a.Transactions.Count > 0) ? a.Transactions.Min(tx => tx.Date) : DateTime.MinValue,
                        LatestTransaction = (a.Transactions.Count > 0) ? a.Transactions.Max(tx => tx.Date) : DateTime.MaxValue
                    })
                    .ToListAsync();

                return new GetAccountSummariesResponse { Accounts = models };
            }
        }
    }
}
