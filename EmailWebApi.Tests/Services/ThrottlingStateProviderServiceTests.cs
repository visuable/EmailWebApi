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
    public class ThrottlingStateProviderServiceTests
    {
        public ThrottlingStateProviderServiceTests()
        {
            var services = new ServiceCollection();
            services.AddTransient<IDateTimeService, SystemDateTimeService>();
            services.AddScoped<IRepository<Email>, FakeEmailRepository>();
            services.AddScoped<IThrottlingStateProviderService, ThrottlingStateProviderService>();
            _provider = services.BuildServiceProvider();
        }

        private readonly IServiceProvider _provider;

        [Theory]
        [ClassData(typeof(EmailTransferServiceTests.EmailGenerator))]
        public async Task GetAsync(Email email)
        {
            //Arrange
            var service = _provider.GetRequiredService<IThrottlingStateProviderService>();
            var repository = _provider.GetRequiredService<IRepository<Email>>();

            await repository.InsertAsync(email);

            //Act
            var result = await service.GetAsync();

            //Assert
            Assert.Equal(1, result.Counter);
            Assert.Equal(1, result.LastAddressCounter);
            Assert.Equal(email.Content.Address, result.LastAddress);
        }
    }
}