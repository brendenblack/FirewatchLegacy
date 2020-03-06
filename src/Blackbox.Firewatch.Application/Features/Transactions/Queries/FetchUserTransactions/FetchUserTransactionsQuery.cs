using AutoMapper;
using AutoMapper.QueryableExtensions;
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

namespace Blackbox.Firewatch.Application.Features.Transactions.Queries.FetchUserTransactions
{
    public class FetchUserTransactionsQuery : IRequest<FetchUserTransactionsResponse>
    {
        /// <summary>
        /// The user to fetch the transactions for.
        /// </summary>
        public string UserId { get; set; }

        //public List<string> AccountNumbers { get; set; } = new List<string>();

        /// <summary>
        /// Limit the result set to only include transactions that happened on or after this date. Default is <see cref="DateTime.MinValue"/>.
        /// </summary>
        public DateTime From { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Limit the rest set to only include transactions that happened on or before this date. Default is <see cref="DateTime.MaxValue"/>.
        /// </summary>
        public DateTime To { get; set; } = DateTime.MaxValue;


        public class FetchUserTransactionsHandler : IRequestHandler<FetchUserTransactionsQuery, FetchUserTransactionsResponse>
        {
            

            private readonly ILogger<FetchUserTransactionsHandler> _logger;
            private readonly IBankContext _bankContext;
            private readonly IMapper _mapper;

            public FetchUserTransactionsHandler(
                ILogger<FetchUserTransactionsHandler> logger,
                IBankContext bankContext,
                IMapper mapper)
            {
                _logger = logger;
                _bankContext = bankContext;
                _mapper = mapper;
            }

            public async Task<FetchUserTransactionsResponse> Handle(FetchUserTransactionsQuery request, CancellationToken cancellationToken)
            {
                var transactions = await _bankContext.Transactions
                    .Where(t => t.Account.OwnerId == request.UserId)
                    .Where(t => t.Date >= request.From)
                    .Where(t => t.Date <= request.To)
                    .ProjectTo<TransactionModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new FetchUserTransactionsResponse { Transactions = transactions };
            }
        }
    }
}
