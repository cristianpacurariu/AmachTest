using Amach.GetCreditData;
using Amach.GetCreditData.SwaggerExamples;
using Amach.HttpClients.CreditData;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Amach.Controllers
{
    [ApiController]
    [Route("")]
    public class DefaultController : ControllerBase
    {
        /// <summary>
        /// Healthcheck to make sure the service is up
        /// </summary>
        /// <response code="200">The service is up and running.</response>
        [HttpGet("ping")]
        public IActionResult Ping() => Ok();

        /// <summary> Return aggregated credit data </summary>
        /// <param name="ssn"> Social security number. </param>
        /// <param name="validator">The ssn validator </param>
        /// <param name="creditDataHttpClient">The credit data http client </param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <remarks>
        /// Example: 424-11-9327, 553-25-8346, 287-54-7823
        /// </remarks>
        /// <response code="200">Aggregated credit data for given ssn.</response>
        /// <response code="400">Invalid ssn.</response>
        /// <response code="404">Credit data not found for given ssn.</response>
        [HttpGet("credit-data/{ssn}")]
        [Produces("application/json")]
        //[SwaggerRequestExample(typeof(GetCreditDataRequest), typeof(GetCreditDataRequestExample))]
        //[SwaggerResponseExample(StatusCodes.Status200OK, typeof(CreditDataEmma))]
        [ProducesResponseType(typeof(CreditData), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = null)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = null)]
        public async Task<IActionResult> CreditData(
            [FromRoute] string ssn,
            [FromServices] IValidator<GetCreditDataRequest> validator,
            [FromServices] CreditDataHttpClient creditDataHttpClient,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(new GetCreditDataRequest { Ssn = ssn }, cancellationToken);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var personalDetailsTask = creditDataHttpClient.GetPersonalDetailsAsync(ssn, cancellationToken);

            var assessedIncomeTask = creditDataHttpClient.GetAssesedIncomeAsync(ssn, cancellationToken);
            var debtTask = creditDataHttpClient.GetDebtAsync(ssn, cancellationToken);

            await Task.WhenAll(personalDetailsTask, assessedIncomeTask, debtTask);
            if (personalDetailsTask.IsFaulted
                || (personalDetailsTask.IsCompletedSuccessfully && personalDetailsTask.Result is null))
                return NotFound();

            var dto = new CreditData
            {
                FirstName = personalDetailsTask.Result.FirstName,
                LastName = personalDetailsTask.Result.LastName,
                Address = personalDetailsTask.Result.Address,
                AssessedIncome = assessedIncomeTask.Result.AssessedIncome,
                BalanceOfDebt = debtTask.Result.BalanceOfDebt,
                Complaints = debtTask.Result.Complaints,
            };
            
            return Ok(dto);
        }
    }
}