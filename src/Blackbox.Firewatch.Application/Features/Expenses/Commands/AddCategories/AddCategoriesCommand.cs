using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Domain.Expenses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Application.Features.Expenses.Commands.AddCategories
{
    public class AddCategoriesCommand : PersonScopedAuthorizationRequiredRequest, IRequest<Result<AddCategoriesResponse>>
    {
        public List<string> CategoryLabels { get; set; } = new List<string>();

        public override string[] AuthorizedRoles => throw new NotImplementedException();

        public class AddCategoriesHandler : IRequestHandler<AddCategoriesCommand, Result<AddCategoriesResponse>>
        {
            private readonly ILogger<AddCategoriesHandler> _logger;
            private readonly IExpenseRepository _expenseRepository;

            public AddCategoriesHandler(
                ILogger<AddCategoriesHandler> logger,
                IExpenseRepository expenseRepository)
            {
                this._logger = logger;
                this._expenseRepository = expenseRepository;
            }

            public async Task<Result<AddCategoriesResponse>> Handle(AddCategoriesCommand request, CancellationToken cancellationToken)
            {
                if (request.CategoryLabels.Count == 0)
                {
                    _logger.LogWarning("Received empty request to create categories from user {}", request.RequestorId);
                    return Result.Ok(new AddCategoriesResponse());
                }

                var systemCategoryLabels = await _expenseRepository.ExpenseCategories
                    .Where(c => c.IsSystemDefault)
                    .Select(c => c.Label)
                    .ToListAsync();

                var createdCategories = new List<Category>();

                foreach (var categoryLabel in request.CategoryLabels)
                {
                    // Skip over any categories that share a label with a system-defined value.
                    if (systemCategoryLabels.Any(c => c == categoryLabel))
                    {
                        continue;
                    }

                    // Skip over any categories that already exist.
                    var alreadyExists = _expenseRepository.ExpenseCategories
                        .Where(c => c.CreatorId == request.OwnerId)
                        .Where(c => c.Label == categoryLabel)
                        .Any();

                    if (alreadyExists)
                    {
                        continue;
                    }

                    var category = new Category
                    {
                        Label = categoryLabel,
                        CreatedOn = DateTime.Now,
                        CreatorId = request.OwnerId,
                        IsSystemDefault = false
                    };

                    _expenseRepository.ExpenseCategories.Add(category);
                    createdCategories.Add(category);
                }

                await _expenseRepository.SaveChangesAsync(cancellationToken);

                var createdIds = createdCategories
                    .Select(c => c.Id)
                    .ToList();

                return Result.Ok(new AddCategoriesResponse { CreatedIds = createdIds });
            }
        }
    }
}
