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

namespace Blackbox.Firewatch.Application.Features.Expenses.Queries.GetCategories
{
    public class GetCategoriesQuery : PersonScopedAuthorizationRequiredRequest, IRequest<GetCategoriesResponse>
    {
        public override string[] AuthorizedRoles => throw new NotImplementedException();

        public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, GetCategoriesResponse>
        {
            private readonly ILogger<GetCategoriesHandler> logger;
            private readonly IExpenseRepository expenseRepository;
            private readonly IMapper mapper;

            public GetCategoriesHandler(
                ILogger<GetCategoriesHandler> logger, 
                IExpenseRepository expenseRepository,
                IMapper mapper)
            {
                this.logger = logger;
                this.expenseRepository = expenseRepository;
                this.mapper = mapper;
            }

            public async Task<GetCategoriesResponse> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
            {
                var categories = await expenseRepository.ExpenseCategories
                    .Where(c => c.IsSystemDefault || c.CreatorId == request.OwnerId)
                    //.ProjectTo<CategoryModel>(mapper.ConfigurationProvider)
                    .ToListAsync();

                var models = mapper.Map<List<CategoryModel>>(categories);

                return new GetCategoriesResponse
                {
                    Categories = models
                };
            }
        }
    }
}
