using System;
using System.Text;
using System.Threading.Tasks;
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
    public class ThrottlingServiceTest
    {
        public ThrottlingServiceTest()
        {
            var services = new ServiceCollection();

            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            services.AddLogging();
            services.AddOptions();

            services.AddScoped<IThrottlingService, ThrottlingService>();
            services.AddScoped<IRepository<Email>, FakeEmailRepository>();
            services.AddTransient<IDateTimeService, SystemDateTimeService>();
            services.AddScoped<IThrottlingStateProviderService, ThrottlingStateProviderService>();
            services.AddScoped<IEmailTransferService, FakeEmailTransferService>();

            services.Configure<SmtpSettings>(_configuration.GetSection("SmtpSettings"));
            services.Configure<ThrottlingSettings>(_configuration.GetSection("ThrottlingSettings"));

            services.AddAutoMapper(typeof(Startup));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _provider = services.BuildServiceProvider();
        }

        private readonly IServiceProvider _provider;
        private readonly IConfiguration _configuration;

        [Fact]
        public async Task Invoke()
        {
            //Arrange
            var service = _provider.GetRequiredService<IThrottlingService>();
            var repository = _provider.GetRequiredService<IRepository<Email>>();
            await repository.InsertAsync(new Email
            {
                Content = new EmailContent
                {
                    Address = "test@test.test"
                },
                Id = 1,
                Info = new EmailInfo
                {
                    Date = DateTime.Now,
                    UniversalId = Guid.NewGuid(),
                    Id = 1
                },
                State = new EmailState
                {
                    Id = 1,
                    Status = EmailStatus.Sent
                }
            });

            //Act
            for (var i = 0; i < 3; i++)
            {
                var email = new Email
                {
                    Content = new EmailContent
                    {
                        Address = "test@test.test"
                    }
                };
                await service.Invoke(email);
            }

            //Assert
            Assert.Equal(1, await repository.GetCountAsync(x => x.State.Status == EmailStatus.Query));
        }
    }
}