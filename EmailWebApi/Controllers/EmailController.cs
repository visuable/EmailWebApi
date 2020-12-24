using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailWebApi.Objects;
using EmailWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmailWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private IThrottlingService _throttlingService;
        private IStatusService _statusService;
        private ILogger<EmailController> _logger;
        public EmailController(IThrottlingService throttlingService, IStatusService statusService, ILogger<EmailController> logger)
        {
            _throttlingService = throttlingService;
            _statusService = statusService;
            _logger = logger;
        }
        [HttpPost]
        [Route(nameof(Send))]
        public IActionResult Send(JsonRequest<EmailDto> request)
        {
            var result = _throttlingService.Invoke(new Email()
            {
                Content = request.Input.Content
            });
            _logger.LogInformation("Сообщение отправлено");
            return Ok(new JsonResponse<List<Email>>()
            {
                Output = result
            });
        }
        [HttpPost]
        [Route(nameof(GetEmailState))]
        public IActionResult GetEmailState(JsonRequest<EmailInfo> request)
        {
            var result = _statusService.GetEmailState(request.Input);
            _logger.LogInformation("Статус сообщения получен");
            return Ok(new JsonResponse<EmailState>()
            {
                Output = result
            });
        }
        [HttpPost]
        [Route(nameof(GetApplicationState))]
        public IActionResult GetApplicationState()
        {
            var result = _statusService.GetApplicationState();
            _logger.LogInformation("Возвращено состояние приложение");
            return Ok(new JsonResponse<ApplicationState>()
            {
                Output = result
            });
        }
    }
}
