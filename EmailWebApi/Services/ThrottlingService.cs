using EmailWebApi.Objects;
using EmailWebApi.Objects.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using EmailWebApi.Extensions;

namespace EmailWebApi.Services
{
    public class ThrottlingService : IThrottlingService
    {
        private ILogger<ThrottlingService> _logger;
        private IEmailTransferService _emailService;
        private IOptions<ThrottlingSettings> _options;
        private IDatabaseManagerService _databaseService;
        public ThrottlingService(ILogger<ThrottlingService> logger, IEmailTransferService emailService, IOptions<ThrottlingSettings> options, IDatabaseManagerService databaseService)
        {
            _logger = logger;
            _emailService = emailService;
            _options = options;
            _databaseService = databaseService;
        }
        public List<Email> Invoke(Email email)
        {
            var outputs = new List<Email>();
            _logger.LogDebug("Состояние получено");
            var state = _databaseService.GetLastThrottlingState();
            var options = _options.Value;


            if (state.Counter < options.Limit && CheckTime(state))
            {
                if (state.LastAddress == email.Content.Address)
                {
                    if (state.LastAddressCounter < options.AddressLimit)
                    {
                        email.SetState(EmailStatus.None);
                        SendEmail(email, outputs);
                        state.IncrementLastAddressCounter();
                        SaveState(state);
                        _logger.LogDebug("Записано состояние");
                        return outputs;
                    }
                    else
                    {
                        email.SetState(EmailStatus.Query);
                        SendEmail(email, outputs);
                        return outputs;
                    }
                }
                else
                {
                    var count = _databaseService.GetCountByStatus(EmailStatus.Query);
                    if (count == 0)
                    {
                        email.SetState(EmailStatus.None);
                        SendEmail(email, outputs);
                        state.IncrementCounter();
                        state.RefreshLastAddress(email.Content.Address);
                        SaveState(state);
                        _logger.LogDebug("Записано состояние");
                        return outputs;
                    }
                    else
                    {
                        var emails = _databaseService.GetEmailsByStatus(EmailStatus.Query);
                        foreach (var queryEmail in emails)
                        {
                            if (_databaseService.GetCountByStatus(EmailStatus.Query) == 0)
                            {
                                email.SetState(EmailStatus.None);
                                SendEmail(email, outputs);
                                state.IncrementCounter();
                                state.RefreshLastAddress(email.Content.Address);
                                SaveState(state);
                                _logger.LogDebug("Записано состояние");
                                return outputs;
                            }
                            else
                            {
                                queryEmail.SetState(EmailStatus.None);
                                SendEmail(queryEmail, outputs);
                                SaveState(state);
                                _logger.LogDebug("Записано состояние");
                                return outputs;
                            }
                        }
                    }
                }
            }
            else if (state.Counter >= options.Limit && CheckTime(state))
            {
                email.SetState(EmailStatus.None);
                outputs.Add(_emailService.Send(email));
                return outputs;
            }
            else
            {
                state.RefreshCounter();
                state.RefreshLastAddressCounter();
                state.RefreshEndPoint();
                SaveState(state);
                _logger.LogDebug("Cброшено и записано состояние.");
                return outputs;
            }
            return outputs;
        }

        private static bool CheckTime(ThrottlingState state)
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds() <= new DateTimeOffset(state.EndPoint).ToUnixTimeSeconds();
        }

        private void SendEmail(Email email, List<Email> outputs)
        {
            outputs.Add(_emailService.Send(email));
        }

        private void SaveState(ThrottlingState state)
        {
            _databaseService.AddThrottlingState(new ThrottlingState()
            {
                Counter = state.Counter,
                EndPoint = state.EndPoint,
                LastAddress = state.LastAddress,
                LastAddressCounter = state.LastAddressCounter
            });
        }
    }
}
