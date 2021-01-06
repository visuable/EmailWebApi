using System;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Services.Classes;
using EmailWebApi.Services.Interfaces;
using EmailWebApi.Tests.Fakes;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EmailWebApi.Tests.Services
{
    public class EmailTransferServiceTest
    {
        public EmailTransferServiceTest()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddScoped<IRepository<Email>, FakeEmailRepository>();
            services.AddScoped<ISmtpSenderService, FakeSmtpSenderService>();
            services.AddScoped<IEmailTransferService, EmailTransferService>();

            _provider = services.BuildServiceProvider();
        }

        private readonly IServiceProvider _provider;

        [Fact]
        public async Task Send()
        {
            //Arrange
            var service = _provider.GetRequiredService<IEmailTransferService>();
            var email = new Email
            {
                Content = new EmailContent
                {
                    Address = "test@test.test",
                    Body = new EmailBody
                    {
                        Body = "test",
                        Save = false
                    },
                    Title = "test"
                }
            };
            var repository = _provider.GetRequiredService<IRepository<Email>>();

            //Act
            await service.Send(email);

            //Assert
            Assert.Equal(EmailStatus.Sent, (await repository.FirstAsync()).State?.Status);
        }
    }
}