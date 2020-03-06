using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Features.Transactions.Commands.AddTransactions;
using Blackbox.Firewatch.Application.Features.Transactions.Queries.FetchTransactions;
using Blackbox.Firewatch.Application.Features.Transactions.Queries.FetchUserTransactions;
using Blackbox.Firewatch.Application.Features.Transactions.Queries.ParseCsv;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blackbox.Firewatch.WebApp.Controllers
{
    [Route("api/transactions")]
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
            /// <summary>
            /// The abbreviation of the bank that produced this transaction record.
            /// </summary>
            [JsonProperty("bank")]
            public string FinancialInstitution { get; set; }

            [JsonProperty("csv")]
            public string CsvContents { get; set; }

            [JsonProperty("duplicates")]
            public bool IsIdentifyDuplicates { get; set; }

        }

        /// <summary>
        /// Read a transaction summary provided by a financial institution in CSV format.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("parse")]
        public async Task<ActionResult<ParseCsvResponse>> ParseCsv([FromBody] ParseCsvModel body)
        {
            var request = new ParseCsvQuery
            {
                PersonId = _currentUserService.UserId,
                CsvContent = body.CsvContents,
                FinancialInstitutionAbbreviation = body.FinancialInstitution,
                ShouldIdentifyDuplicates = body.IsIdentifyDuplicates,
            };

            return await _mediator.Send(request);
        }

        /// <summary>
        /// Retrieve transaction records.
        /// </summary>
        /// <param name="from">An optional date filter, limiting results to only values on or later than the date specified. In yyyyMMdd format.</param>
        /// <param name="to">An optional date filter, limiting results to only values on or earlier than the date specified. In yyyyMMdd format.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<FetchUserTransactionsResponse>> FetchTransactions(
            [FromQuery] string? from, 
            [FromQuery] string? to)
        {
            DateTime fromDate;
            if (!DateTime.TryParseExact(from, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDate))
            {
                fromDate = DateTime.MinValue;
            }

            DateTime toDate;
            if (!DateTime.TryParseExact(to, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out toDate))
            {
                toDate = DateTime.MaxValue;
            }

            var request = new FetchUserTransactionsQuery
            {
                From = fromDate,
                To = toDate,
                UserId = _currentUserService.UserId
            };

            return await _mediator.Send(request);
        }


        [HttpPost]
        public async Task<ActionResult<AddTransactionsResponse>> AddTransactions([FromBody] List<AddTransactionModel> transactions)
        {
            var request = new AddTransactionsCommand
            {
                PersonId = _currentUserService.UserId,
                Transactions = transactions
            };

            var result = await _mediator.Send(request);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
        }

    }
}