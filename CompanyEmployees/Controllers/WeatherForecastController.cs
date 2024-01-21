using Contract;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ILoggerManager _LoggerManager;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, ILoggerManager loggerManager)
        {
            _logger = logger;
            _LoggerManager = loggerManager;
        }

       

        [HttpGet]
        public IEnumerable<string> Get()
        {
            _LoggerManager.LogInfo("Here is info message from our values controller.");
            _LoggerManager.LogDebug("Here is debug message from our values controller.");
            _LoggerManager.LogWarn("Here is warn message from our values controller.");
            _LoggerManager.LogError("Here is an error message from our values controller.");
            return new string[] { "value1", "value2" };
        }
    }
}
