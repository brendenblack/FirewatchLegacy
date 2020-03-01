using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbox.Firewatch.Application.Features.Expenses.Commands.AddExpenses
{
    public class AddExpensesCommand : PersonScopedAuthorizationRequiredRequest, IRequest<Result<AddExpensesResponse>>
    {
        public override string[] AuthorizedRoles => throw new NotImplementedException();

        public class AddExpensesHandler : IRequestHandler<AddExpensesCommand, Result<AddExpensesResponse>>
        {
            private readonly ILogger<AddExpensesHandler> logger;

            public AddExpensesHandler(ILogger<AddExpensesHandler> logger)
            {
                this.logger = logger;
            }

            public Task<Result<AddExpensesResponse>> Handle(AddExpensesCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
