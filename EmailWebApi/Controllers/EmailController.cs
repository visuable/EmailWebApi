using System.Threading.Tasks;
using AutoMapper;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;
using EmailWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailWebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStatusService _statusService;
        private readonly IThrottlingService _throttlingService;

        public EmailController(IThrottlingService throttlingService, IStatusService statusService, IMapper mapper)
        {
            _throttlingService = throttlingService;
            _statusService = statusService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Отсылает сообщение.
        /// </summary>
        /// <returns>EmailInfoDto</returns>
        /// <returns>Возвращает EmailInfo данного сообщеня.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(JsonResponse<EmailInfoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(JsonResponse<EmailInfoDto>), StatusCodes.Status500InternalServerError)]
        [Route(nameof(Send))]
        public async Task<IActionResult> Send([FromBody] JsonRequest<EmailDto> request)
        {
            var email = _mapper.Map<Email>(request.Input);
            var result = await _throttlingService.Invoke(email);
            return Ok(new JsonResponse<EmailInfoDto>
            {
                Output = _mapper.Map<EmailInfoDto>(result)
            });
        }

        /// <summary>
        ///     Возвращает статус сообщения по дате и/или Guid.
        /// </summary>
        /// <returns>EmailStateDto</returns>
        [HttpPost]
        [ProducesResponseType(typeof(JsonResponse<EmailStateDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(JsonResponse<EmailInfoDto>), StatusCodes.Status500InternalServerError)]
        [Route(nameof(GetEmailState))]
        public async Task<IActionResult> GetEmailState([FromBody] JsonRequest<EmailInfoDto> request)
        {
            var emailInfo = _mapper.Map<EmailInfo>(request.Input);
            var result = await _statusService.GetEmailState(emailInfo);
            return Ok(new JsonResponse<EmailStateDto>
            {
                Output = _mapper.Map<EmailStateDto>(result)
            });
        }

        /// <summary>
        ///     Возвращает общую статистику приложения.
        /// </summary>
        /// <returns>ApplicationStateDto</returns>
        [HttpPost]
        [ProducesResponseType(typeof(JsonResponse<ApplicationStateDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(JsonResponse<object>), StatusCodes.Status500InternalServerError)]
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