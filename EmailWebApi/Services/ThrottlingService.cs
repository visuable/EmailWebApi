using EmailWebApi.Objects;
using EmailWebApi.Objects.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

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
            if(state == null)
            {
                state = new ThrottlingState()
                {
                    Counter = 0,
                    EndPoint = DateTime.Now.AddSeconds(60),
                    LastAddress = string.Empty,
                    LastAddressCounter = 0
                };
            }
            var options = _options.Value;


            if (state.Counter < options.Limit && DateTimeOffset.Now.ToUnixTimeSeconds() < new DateTimeOffset(state.EndPoint).ToUnixTimeSeconds())
            {
                if (state.LastAddress == email.Content.Address)
                {
                    if (state.LastAddressCounter < options.AddressLimit)
                    {
                        outputs.Add(_emailService.Send(email));
                        state.LastAddressCounter++;
                        _databaseService.AddThrottlingState(state);
                        _logger.LogDebug("Отправлено сообщение. Записано состояние");
                        return outputs;
                    }
                    else
                    {
                        email.State = new EmailState() { Status = EmailStatus.Query };
                        outputs.Add(_emailService.Send(email));
                        return outputs;
                    }
                }
                else
                {
                    var count = _databaseService.GetCountByStatus(EmailStatus.Query);
                    if (count == 0)
                    {
                        email.State = new EmailState() { Status = EmailStatus.None };
                        //outputs.Add(_emailService.Send(email));
                        state.Counter++;
                        state.LastAddress = email.Content.Address;
                        _databaseService.AddThrottlingState(state);
                        _logger.LogDebug("Отправлено сообщение. Записано состояние");
                        return outputs;
                    }
                    else
                    {
                        var emails = _databaseService.GetEmailsByStatus(EmailStatus.Query);
                        foreach (var queryEmail in emails)
                        {
                            if (_databaseService.GetCountByStatus(EmailStatus.Query) == 0)
                            {
                                email.State.Status = EmailStatus.None;
                                outputs.Add(_emailService.Send(email));
                                state.Counter++;
                                state.LastAddress = email.Content.Address;
                                _databaseService.AddThrottlingState(state);
                                _logger.LogDebug("Отправлено сообщение. Записано состояние");
                                return outputs;
                            }
                            else
                            {
                                queryEmail.State.Status = EmailStatus.None;
                                outputs.Add(_emailService.Send(email));
                                state.Counter++;
                                state.LastAddress = email.Content.Address;
                                _databaseService.AddThrottlingState(state);
                                _logger.LogDebug("Отправлено сообщение. Записано состояние");
                                return outputs;
                            }
                        }
                    }
                }
            }
            else if (state.Counter >= options.Limit && DateTimeOffset.Now.ToUnixTimeSeconds() <= new DateTimeOffset(state.EndPoint).ToUnixTimeSeconds())
            {
                email.State.Status = EmailStatus.Query;
                outputs.Add(_emailService.Send(email));
                return outputs;
            }
            else
            {
                state.Counter = 0;
                state.EndPoint = DateTime.Now.AddSeconds(60);
                state.LastAddressCounter = 0;
                _databaseService.AddThrottlingState(state);
                _logger.LogDebug("Cброшено и записано состояние.");
                return outputs;
            }
            return outputs;
        }
    }
}
