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

namespace Blackbox.Firewatch.Application.Features.Transactions.Queries.FetchTransactions
{
    [Obsolete("Use FetchUserTransactions instead.")]
    public class FetchTransactionsQuery : PersonScopedAuthorizationRequiredRequest, IRequest<FetchTransactionsResponse>
    {
        public DateTime From { get; set; } = DateTime.MinValue;

        public DateTime To { get; set; } = DateTime.MaxValue;
        //public override string[] AuthorizedRoles => throw new NotImplementedException();

        public class FetchTransactionsHandler : IRequestHandler<FetchTransactionsQuery, FetchTransactionsResponse>
        {
            private readonly ILogger<FetchTransactionsHandler> _logger;
            private readonly IBankContext _repository;
            private readonly IMapper _mapper;

            public FetchTransactionsHandler(ILogger<FetchTransactionsHandler> logger, IBankContext repository, IMapper mapper)
            {
                _logger = logger;
                _repository = repository;
                _mapper = mapper;
            }

            

            public async Task<FetchTransactionsResponse> Handle(FetchTransactionsQuery request, CancellationToken cancellationToken)
            {
                //    var transactions = _repository.Transactions
                //        .Where(t => t.Account.OwnerId == request.OwnerId)
                //        .Where(t => t.Date >= request.From)
                //        .Where(t => t.Date <= request.To);

                var transactions = await _repository.Transactions
                    .Where(t => t.Account.OwnerId == request.OwnerId)
                    .Where(t => t.Date >= request.From)
                    .Where(t => t.Date <= request.To)
                    .ProjectTo<FetchTransactionModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                //var models = await _mapper.ProjectTo<TransactionModel>(transactions)
                //    .ToListAsync();

                //var t = await _mapper.ProjectTo<TransactionModel>(
                //    _repository.Transactions.Where(t => t.Account.OwnerId == request.OwnerId)
                //        .Where(t => t.Date >= request.From)
                //        .Where(t => t.Date <= request.To))
                //    .ToListAsync();

                //var models = _mapper.Map<List<TransactionModel>>(transactions);

                return new FetchTransactionsResponse { Transactions = transactions };
            }
        }
    }
}
