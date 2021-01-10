using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Services.Classes;
using EmailWebApi.Services.Interfaces;
using EmailWebApi.Tests.Fakes;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EmailWebApi.Tests.Services
{
    public class EmailTransferServiceTests
    {
        public EmailTransferServiceTests()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddScoped<IRepository<Email>, FakeEmailRepository>();
            services.AddScoped<ISmtpSenderService, FakeSmtpSenderService>();
            services.AddScoped<IEmailTransferService, EmailTransferService>();

            _provider = services.BuildServiceProvider();
        }

        private readonly IServiceProvider _provider;

        [Theory]
        [ClassData(typeof(EmailGenerator))]
        public async Task Send(Email email)
        {
            //Arrange
            var service = _provider.GetRequiredService<IEmailTransferService>();
            var repository = _provider.GetRequiredService<IRepository<Email>>();

            //Act
            await service.Send(email);

            //Assert
            Assert.Equal(EmailStatus.Sent, (await repository.FirstAsync()).State?.Status);
        }

        public class EmailGenerator : IEnumerable<object[]>
        {
            private object[] _data;

            public EmailGenerator()
            {
                _data = new object[]
                {
                    new Email
                    {
                        Content = new EmailContent
                        {
                            Address = "test@test.test",
                            Body = new EmailBody
                            {
                                Body = "test",
                                Save = true
                            },
                            Title = "test",
                        },
                        Info = new EmailInfo()
                        {
                            Date = DateTime.Now,
                            UniversalId = Guid.NewGuid()
                        }
                    }
                };
            }
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return _data;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}