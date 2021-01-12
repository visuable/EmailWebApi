using System;
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
        public SmtpClientFactoryServiceTests()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var services = new ServiceCollection();

            services.AddOptions();
            services.AddLogging();
            services.Configure<SmtpSettings>(_configuration.GetSection("SmtpSettings"));
            services.AddScoped<ISmtpClientFactoryService, SmtpClientFactoryService>();

            _provider = services.BuildServiceProvider();
        }

        private readonly IServiceProvider _provider;
        private readonly IConfiguration _configuration;

        [Fact]
        public void Create()
        {
            //Arrange
            var service = _provider.GetRequiredService<ISmtpClientFactoryService>();
            var options = _provider.GetRequiredService<IOptions<SmtpSettings>>().Value;

            //Act
            var client = service.Create();

            //Assert
            Assert.Equal(client.Host, options.Host);
            Assert.Equal(client.Port, options.Port);
            Assert.True(client.EnableSsl);
        }
    }
}