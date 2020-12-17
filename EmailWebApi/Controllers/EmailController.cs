using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EmailWebApi.Models;
using EmailWebApi.Models.Controllers;
using EmailWebApi.Models.Dto;
using EmailWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailWebApi.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private IEmailService _service;
        private IThrottlerService<Guid> _throttlerService;
        private IMapper _mapper;
        public EmailController(IEmailService service, IMapper mapper, IThrottlerService<Guid> throttlerService)
        {
            _service = service;
            _mapper = mapper;
            _throttlerService = throttlerService;
        }
        [Route(nameof(SendEmail))]
        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] JsonRequest<EmailDto> request)
        {
            //var result = _service.SendEmail(_mapper.Map<Email>(request.Value));
            var result = _throttlerService.Invoke(_service.SendEmail(_mapper.Map<Email>(request.Value)));
            var response = new JsonResponse<Guid>(await result);
            return Ok(response);
        }
        [Route(nameof(GetEmailStatus))]
        [HttpPost]
        public IActionResult GetEmailStatus(JsonRequest<Guid> request)
        {
            var result = _service.GetEmailStatus(request.Value);
            var response = new JsonResponse<EmailDto>(_mapper.Map<EmailDto>(result));
            return Ok(response);
        }
        [Route(nameof(GetCurrentStatus))]
        [HttpPost]
        public IActionResult GetCurrentStatus()
        {
            var result = _service.GetCurrentStatus();
            var response = new JsonResponse<StatusModelDto>(_mapper.Map<StatusModelDto>(result));
            return Ok(response);
        }
    }
}
