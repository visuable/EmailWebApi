using System;
using System.Text;
using AutoMapper;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Settings;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Services.Classes;
using EmailWebApi.Services.Interfaces;
using EmailWebApi.Tests.Fakes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EmailWebApi.Tests.Services
{
    public class StatusServiceTests
    {
        public StatusServiceTests()
        {
            var services = new ServiceCollection();

            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            services.AddLogging();
            services.AddOptions();

            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IThrottlingService, FakeThrottlingService>();
            services.AddScoped<IRepository<Email>, FakeEmailRepository>();

            services.Configure<SmtpSettings>(_configuration.GetSection("SmtpSettings"));
            services.Configure<ThrottlingSettings>(_configuration.GetSection("ThrottlingSettings"));

            services.AddAutoMapper(typeof(Startup));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.AddHostedService<QueryExecutorService>();

            _provider = services.BuildServiceProvider();
        }

        private readonly IServiceProvider _provider;
        private readonly IConfiguration _configuration;

        [Fact]
        public async void GetApplicationState()
        {
            //Arrange
            var service = _provider.GetRequiredService<IStatusService>();
            var repository = _provider.GetRequiredService<IRepository<Email>>();

            await repository.InsertAsync(new Email
            {
                State = new EmailState
                {
                    Status = EmailStatus.Error
                }
            });
            await repository.InsertAsync(new Email
            {
                State = new EmailState
                {
                    Status = EmailStatus.Sent
                }
            });
            await repository.InsertAsync(new Email
            {
                State = new EmailState
                {
                    Status = EmailStatus.Query
                }
            });

            //Act
            var result = await service.GetApplicationState();

            //Assert
            Assert.Equal(3, result.Total);
        }

        [Fact]
        public async void GetEmailState()
        {
            //Arrange
            var service = _provider.GetRequiredService<IStatusService>();
            var repository = _provider.GetRequiredService<IRepository<Email>>();

            await repository.InsertAsync(new Email
            {
                State = new EmailState
                {
                    Status = EmailStatus.Query
                },
                Info = new EmailInfo
                {
                    UniversalId = Guid.Empty,
                    Date = DateTime.Now
                }
            });

            //Act
            var result = await service.GetEmailState(new EmailInfo
            {
                UniversalId = Guid.Empty
            });

            //Assert
            Assert.Equal(EmailStatus.Query, result.Status);
        }
    }
}