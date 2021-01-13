using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities.Settings;
using EmailWebApi.Services.Interfaces;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace EmailWebApi.Services.Classes
{
    public class SmtpClientFactoryService : ISmtpClientFactoryService
    {
        private readonly IOptions<SmtpSettings> _options;
        public SmtpClientFactoryService(IOptions<SmtpSettings> options)
        {
            _options = options;
        }
        public async Task<ISmtpClient> CreateAsync()
        {
            var client = new SmtpClient(new ProtocolLogger(_options.Value.LogFileName));
            return client;
        }
    }
}
