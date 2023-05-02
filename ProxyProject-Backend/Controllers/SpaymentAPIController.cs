using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.Services.Interface;

namespace ProxyProject_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpaymentAPIController : ControllerBase
    {
        private readonly ISpayment _spayment;
        public SpaymentAPIController(ISpayment spayment)
        {
            _spayment = spayment;
        }
        [HttpGet("")]
        public String Test()
        {
            RecurringJob.AddOrUpdate<ISpayment>(x => x.SyncRecords(), Cron.Minutely);
            return $"Job ID. You added one product into your checklist successfully!";
            // return "hiuhi";
        }
        [HttpGet("check-bank-account")]
        public IActionResult CheckBank()
        {
            _spayment.SyncRecords();
            return Ok();
        }
    }
}
