using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Features.Accounts.Commands.AddAccount;
using Blackbox.Firewatch.Application.Features.Accounts.Queries.GetAccountSummaries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blackbox.Firewatch.WebApp.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMediator _mediator;

        public AccountsController(ICurrentUserService currentUserService, IMediator mediator)
        {
            _currentUserService = currentUserService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<GetAccountSummariesResponse>> GetAccountSummaries()
        {
            var request = new GetAccountSummariesQuery { PersonId = _currentUserService.UserId };

            return await _mediator.Send(request);
        }

        public class CreateAccountModel
        {
            public string AccountNumber { get; set; }

            public string Nickname { get; set; }

            public string Bank { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<int>> CreateAccount([FromBody] CreateAccountModel model)
        {
            var command = new AddAccountCommand
            {
                AccountNumber = model.AccountNumber,
                FinancialInstitutionAbbreviation = model.Bank,
                Nickname = model.Nickname,
                PersonId = _currentUserService.UserId
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Created("", result.Value);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
        }
    }
}
