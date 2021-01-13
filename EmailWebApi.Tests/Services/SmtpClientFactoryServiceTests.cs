using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities.Settings;
using EmailWebApi.Services.Classes;
using EmailWebApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace EmailWebApi.Tests.Services
{
    public class SmtpClientFactoryServiceTests
    {
        private readonly IServiceProvider _provider;
        private readonly IConfiguration _configuration;

        public SmtpClientFactoryServiceTests()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var services = new ServiceCollection();
            services.AddOptions();
            services.AddScoped<ISmtpClientFactoryService, SmtpClientFactoryService>();
            services.Configure<SmtpSettings>(_configuration.GetSection("SmtpSettings"));
            _provider = services.BuildServiceProvider();
        }
        [Fact]
        public async Task CreateAsync()
        {
            //Arrange
            var service = _provider.GetRequiredService<ISmtpClientFactoryService>();

            //Act
            var client = await service.CreateAsync();

            //Assert
            Assert.NotNull(client);
        }
    }
}
