using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Features.Transactions.Queries.ParseCsv;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blackbox.Firewatch.WebApp.Controllers
{
    [Route("api/users/{userId}/transactions")]
    [Authorize]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMediator _mediator;

        public TransactionsController(ICurrentUserService currentUserService, IMediator mediator)
        {
            _currentUserService = currentUserService;
            _mediator = mediator;
        }

        public class ParseCsvModel
        {
            [JsonProperty("bank")]
            public string FinancialInstitution { get; set; }

            [JsonProperty("csv")]
            public string CsvContents { get; set; }

            [JsonProperty("duplicates")]
            public bool IsIdentifyDuplicates { get; set; }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("parse")]
        public async Task<ActionResult<ParseCsvResponse>> ParseCsv([FromRoute] string userId, [FromBody] ParseCsvModel body)
        {
            var request = new ParseCsvQuery
            {
                OwnerId = userId, // body.UserId ?? _currentUserService.UserId,
                RequestorId = _currentUserService.UserId,
                CsvContent = body.CsvContents,
                FinancialInstitutionAbbreviation = body.FinancialInstitution,
                ShouldIdentifyDuplicates = body.IsIdentifyDuplicates,
            };

            return await _mediator.Send(request);
        }

    }
}