using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Settings;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Services;
using EmailWebApi.Tests.Fakes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace EmailWebApi.Tests.Services
{
    public class QueryExecutorServiceTest
    {
        private IServiceProvider _provider;
        public QueryExecutorServiceTest()
        {
            var services = new ServiceCollection();

            services.AddLogging();

            services.AddScoped<BackgroundService, QueryExecutorService>();
            services.AddScoped<IEmailRepository, FakeEmailRepository>();
            services.AddScoped<IEmailTransferService, FakeEmailTransferService>();

            services.AddScoped<IDatabaseManagerService, DatabaseManagerService>();

            services.AddAutoMapper(typeof(Startup));

            _provider = services.BuildServiceProvider();
        }
        [Fact]
        public async Task ExecuteAsync()
        {
            //Arrange
            var service = _provider.GetRequiredService<BackgroundService>();
            var repository = _provider.GetRequiredService<IEmailRepository>();
            await repository.AddAsync(new Email()
            {
                Content = new EmailContent(),
                Id = 1,
                Info = new EmailInfo()
                {
                    Date = DateTime.Now,
                    Id = 1,
                    UniversalId = Guid.NewGuid()
                },
                State = new EmailState()
                {
                    Id = 1,
                    Status = EmailStatus.Query
                }
            });

            //Act
            var token = new CancellationToken();
            await service.StartAsync(token).ConfigureAwait(false);

            //Assert

            Assert.Equal(0, await repository.CountAsync(x => x.State.Status == EmailStatus.Query));
        }
    }
}
