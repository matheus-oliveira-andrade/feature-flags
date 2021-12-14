using FeatureFlag.API.FeatureFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using System.Threading.Tasks;

namespace FeatureFlag.API.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IFeatureManager _featureManager;

        public PaymentsController(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        [HttpGet("health-check")]
        public async Task<IActionResult> CheckFlags()
        {
            var flagsState = new
            {
                EnabledCreditCardPayments = await _featureManager.IsEnabledAsync("CreditCardPayments"),
                EnabledDebitCardPayments = await _featureManager.IsEnabledAsync("DebitCardPayments"),
                EnabledTransferPayments = await _featureManager.IsEnabledAsync("BusinessTimeWindow")
            };

            return Json(flagsState);
        }

        [HttpPost("credit-payment")]
        public async Task<IActionResult> CreditPayment()
        {            
            if (await _featureManager.IsEnabledAsync("CreditCardPayments"))
            {
                return Ok("Successful credit payment");
            }

            return BadRequest("This functionality is disabled");
        }

        [HttpPost("debit-payment")]
        public async Task<IActionResult> DebitPayment()
        {
            if (await _featureManager.IsEnabledAsync("DebitCardPayments"))
            {
                return Ok("Successful debit payment");
            }

            return BadRequest("This functionality is disabled");
        }

        [HttpPost("transfer-payment")]
        public async Task<IActionResult> TransferPayment()
        {
            // Custom feature flag
            if (await _featureManager.IsEnabledAsync("BusinessTimeWindow"))
            {
                return Ok("Successful transfer payment");
            }

            return BadRequest("This functionality is disabled and is enabled only in business time");
        }
    }
}
