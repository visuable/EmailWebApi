using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWebApi.Extensions;
using EmailWebApi.Objects;
using EmailWebApi.Objects.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmailWebApi.Services
{
    public class ThrottlingService : IThrottlingService
    {
        private readonly IDatabaseManagerService _databaseService;
        private readonly IEmailTransferService _emailService;
        private readonly ILogger<ThrottlingService> _logger;
        private readonly IOptions<ThrottlingSettings> _options;

        private ThrottlingState latestState;

        public ThrottlingService(ILogger<ThrottlingService> logger, IEmailTransferService emailService,
            IOptions<ThrottlingSettings> options, IDatabaseManagerService databaseService)
        {
            _logger = logger;
            _emailService = emailService;
            _options = options;
            _databaseService = databaseService;
        }

        public async Task Invoke(Email email)
        {
            latestState = await _databaseService.GetLastThrottlingState();
            if (ConsumeTime() && ConsumeCounter())
            {
                if (latestState.LastAddress != email.Content.Address)
                {
                    latestState.RefreshLastAddress(email.Content.Address);
                    await _emailService.Send(email);
                    latestState.IncrementCounter();
                    await SaveThrottlingState();
                }
                else if(ConsumeAddressCounter())
                {
                    await _emailService.Send(email);
                    latestState.IncrementLastAddressCounter();
                    await SaveThrottlingState();
                }
                else
                {
                    email.SetState(EmailStatus.Query);
                    await _databaseService.AddEmail(email);
                }
            }
            else if (ConsumeTime() && !ConsumeCounter())
            {
                email.SetState(EmailStatus.Query);
                await _databaseService.AddEmail(email);
            }
            else if (!ConsumeTime())
            {
                latestState.RefreshEndPoint();
                latestState.RefreshCounter();
                latestState.RefreshLastAddressCounter();
                await SaveThrottlingState();
                await Invoke(email);
            }
        }

        private async Task SaveThrottlingState()
        {
            await _databaseService.AddThrottlingState(new ThrottlingState()
            {
                Counter = latestState.Counter,
                EndPoint = latestState.EndPoint,
                LastAddress = latestState.LastAddress,
                LastAddressCounter = latestState.LastAddressCounter
            });
        }
        private bool ConsumeTime()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds() <= new DateTimeOffset(latestState.EndPoint).ToUnixTimeSeconds();
        }

        private bool ConsumeCounter()
        {
            return latestState.Counter < _options.Value.Limit;
        }

        private bool ConsumeAddressCounter()
        {
            return latestState.LastAddressCounter < _options.Value.AddressLimit;
        }
    }
}