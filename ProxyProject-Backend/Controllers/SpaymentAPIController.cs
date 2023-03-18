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
        [HttpGet("")]
        public String Test()
        {
            RecurringJob.AddOrUpdate<ISpayment>(x => x.SyncRecords(), Cron.Minutely);
            return $"Job ID. You added one product into your checklist successfully!";
            // return "hiuhi";
        }
    }
}
