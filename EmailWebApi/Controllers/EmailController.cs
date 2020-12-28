using System.Collections.Generic;
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
        private readonly IStatusService _statusService;
        private readonly IThrottlingService _throttlingService;

        public EmailController(IThrottlingService throttlingService, IStatusService statusService)
        {
            _throttlingService = throttlingService;
            _statusService = statusService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(JsonResponse<object>), StatusCodes.Status200OK)]
        [Route(nameof(Send))]
        public async Task<IActionResult> Send(JsonRequest<EmailDto> request)
        {
            await _throttlingService.Invoke(new Email
            {
                Content = request.Input.Content
            });
            return Ok(new JsonResponse<object>());
        }

        [HttpPost]
        [ProducesResponseType(typeof(JsonResponse<EmailInfo>), StatusCodes.Status200OK)]
        [Route(nameof(GetEmailState))]
        public async Task<IActionResult> GetEmailState(JsonRequest<EmailInfo> request)
        {
            var result = await _statusService.GetEmailState(request.Input);
            return Ok(new JsonResponse<EmailState>()
            {
                Output = result
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(JsonResponse<ApplicationState>), StatusCodes.Status200OK)]
        [Route(nameof(GetApplicationState))]
        public async Task<IActionResult> GetApplicationState()
        {
            var result = await _statusService.GetApplicationState();
            return Ok(new JsonResponse<ApplicationState>
            {
                Output = result
            });
        }
    }
}