using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ThrottlingStateProviderServiceTest
    {
        private IServiceProvider _provider;

        public ThrottlingStateProviderServiceTest()
        {
            var services = new ServiceCollection();
            services.AddTransient<IDateTimeService, SystemDateTimeService>();
            services.AddScoped<IRepository<Email>, FakeEmailRepository>();
            services.AddScoped<IThrottlingStateProviderService, ThrottlingStateProviderService>();
            _provider = services.BuildServiceProvider();
        }
        [Fact]
        public async Task GetAsync()
        {
            //Arrange
            var service = _provider.GetRequiredService<IThrottlingStateProviderService>();
            var repository = _provider.GetRequiredService<IRepository<Email>>();
            var time = _provider.GetRequiredService<IDateTimeService>();

            await repository.InsertAsync(new Email()
            {
                Content = new EmailContent()
                {
                    Address = "test@test.test",
                    Body = new EmailBody()
                    {
                        Body = String.Empty,
                        Save = false
                    },
                    Title = "test"
                },
                Info = new EmailInfo()
                {
                    Date = time.Now.DateTime
                }
            });

            //Act
            var result = await service.GetAsync();

            //Assert
            Assert.Equal(1, result.Counter);
            Assert.Equal(1, result.LastAddressCounter);
            Assert.Equal("test@test.test", result.LastAddress);

            //Arrange
            await repository.InsertAsync(new Email()
            {
                Content = new EmailContent()
                {
                    Address = "test@test",
                    Body = new EmailBody()
                    {
                        Body = String.Empty,
                        Save = false
                    },
                    Title = "test"
                },
                Info = new EmailInfo()
                {
                    Date = time.Now.DateTime
                }
            });

            //Act
            result = await service.GetAsync();

            //Assert
            Assert.Equal(2, result.Counter);
            Assert.Equal(1, result.LastAddressCounter);
            Assert.Equal("test@test", result.LastAddress);
        }
    }
}
