using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EmailWebApi.Objects;
using EmailWebApi.Objects.Dto;
using EmailWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmailWebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IStatusService _statusService;
        private readonly IThrottlingService _throttlingService;
        private readonly IMapper _mapper;

        public EmailController(IThrottlingService throttlingService, IStatusService statusService, IMapper mapper)
        {
            _throttlingService = throttlingService;
            _statusService = statusService;
            _mapper = mapper;
        }
        /// <summary>
        /// Отсылает сообщение.
        /// </summary>
        /// <param name="request">Экземлпяр EmailDto.</param>
        /// <returns>EmailInfoDto</returns>
        [HttpPost]
        [ProducesResponseType(typeof(JsonResponse<EmailInfoDto>), StatusCodes.Status200OK)]
        [Route(nameof(Send))]
        public async Task<IActionResult> Send(JsonRequest<EmailDto> request)
        {
            var email = _mapper.Map<Email>(request.Input);
            var result = await _throttlingService.Invoke(email);
            return Ok(new JsonResponse<EmailInfoDto>()
            {
                Output = _mapper.Map<EmailInfoDto>(result)
            });
        }
        /// <summary>
        /// Возвращает статус сообщения по дате и/или Guid.
        /// </summary>
        /// <param name="request">Экземпляр EmailInfoDto</param>
        /// <returns>EmailStateDto</returns>
        [HttpPost]
        [ProducesResponseType(typeof(JsonResponse<EmailStateDto>), StatusCodes.Status200OK)]
        [Route(nameof(GetEmailState))]
        public async Task<IActionResult> GetEmailState(JsonRequest<EmailInfoDto> request)
        {
            var emailInfo = _mapper.Map<EmailInfo>(request.Input);
            var result = await _statusService.GetEmailState(emailInfo);
            return Ok(new JsonResponse<EmailStateDto>()
            {
                Output = _mapper.Map<EmailStateDto>(result)
            });
        }
        /// <summary>
        /// Возвращает общую статистику приложения.
        /// </summary>
        /// <returns>ApplicationStateDto</returns>
        [HttpPost]
        [ProducesResponseType(typeof(JsonResponse<ApplicationStateDto>), StatusCodes.Status200OK)]
        [Route(nameof(GetApplicationState))]
        public async Task<IActionResult> GetApplicationState()
        {
            var result = await _statusService.GetApplicationState();
            return Ok(new JsonResponse<ApplicationStateDto>
            {
                Output = result
            });
        }
    }
}