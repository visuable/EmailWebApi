using System;
using System.Text;
using AutoMapper;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Settings;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Services;
using EmailWebApi.Tests.Fakes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EmailWebApi.Tests.Services
{
    public class DatabaseManagerServiceTest
    {
        public DatabaseManagerServiceTest()
        {
            var services = new ServiceCollection();

            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            services.AddLogging();
            services.AddOptions();

            services.AddScoped<IStatusService, FakeStatusService>();
            services.AddScoped<IThrottlingService, FakeThrottlingService>();
            services.AddScoped<IEmailRepository, FakeEmailRepository>();

            services.AddScoped<IDatabaseManagerService, DatabaseManagerService>();

            services.Configure<SmtpSettings>(_configuration.GetSection("SmtpSettings"));
            services.Configure<ThrottlingSettings>(_configuration.GetSection("ThrottlingSettings"));

            services.AddAutoMapper(typeof(Startup));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _provider = services.BuildServiceProvider();
        }

        private readonly IServiceProvider _provider;
        private readonly IConfiguration _configuration;

        [Fact]
        public async void AddEmailAsync()
        {
            //Arrange
            var service = _provider.GetRequiredService<IDatabaseManagerService>();
            var repository = _provider.GetRequiredService<IEmailRepository>();
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
                },
                Info = new EmailInfo
                {
                    Date = DateTime.Now,
                    UniversalId = Guid.Empty
                }
            };

            //Act
            await service.AddEmailAsync(email);

            //Assert
            Assert.Equal(1, await repository.CountAsync());
        }

        [Fact]
        public async void GetAllCountAsync()
        {
            //Arrange
            var service = _provider.GetRequiredService<IDatabaseManagerService>();
            var repository = _provider.GetRequiredService<IEmailRepository>();

            for (var i = 0; i < 5; i++) await repository.AddAsync(new Email());

            //Act
            var result = await service.GetAllCountAsync();

            //Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public async void GetCountByStatusAsync()
        {
            //Arrange
            var service = _provider.GetRequiredService<IDatabaseManagerService>();
            var repository = _provider.GetRequiredService<IEmailRepository>();
            var emailError = new Email
            {
                State = new EmailState
                {
                    Status = EmailStatus.Error
                }
            };
            await repository.AddAsync(emailError);
            var emailQuery = new Email
            {
                State = new EmailState
                {
                    Status = EmailStatus.Query
                }
            };
            await repository.AddAsync(emailQuery);

            //Act
            var result = await service.GetCountByStatusAsync(EmailStatus.Error);

            //Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async void GetEmailByEmailInfoAsync()
        {
            //Arrange
            var service = _provider.GetRequiredService<IDatabaseManagerService>();
            var repository = _provider.GetRequiredService<IEmailRepository>();
            var emailInfo = new EmailInfo
            {
                UniversalId = Guid.Empty
            };
            var email = new Email
            {
                Info = emailInfo
            };
            await repository.AddAsync(email);

            //Act
            var result = await service.GetEmailByEmailInfoAsync(emailInfo);

            //Assert
            Assert.Equal(Guid.Empty, result.Info.UniversalId);
        }

        [Fact]
        public async void GetEmailsByStatusAsync()
        {
            //Arrange
            var service = _provider.GetRequiredService<IDatabaseManagerService>();
            var repository = _provider.GetRequiredService<IEmailRepository>();

            for (var i = 0; i < 5; i++)
                await repository.AddAsync(new Email
                {
                    Content = new EmailContent(),
                    Info = new EmailInfo(),
                    State = new EmailState
                    {
                        Status = EmailStatus.Error
                    }
                });

            for (var i = 0; i < 6; i++)
                await repository.AddAsync(new Email
                {
                    Content = new EmailContent(),
                    Info = new EmailInfo(),
                    State = new EmailState
                    {
                        Status = EmailStatus.Query
                    }
                });

            //Act
            var result = await service.GetEmailsByStatusAsync(EmailStatus.Query);

            //Assert
            Assert.Equal(6, result.Count);
        }

        [Fact]
        public async void GetThrottlingStateAsync()
        {
            //Arrange
            var service = _provider.GetRequiredService<IDatabaseManagerService>();
            var repository = _provider.GetRequiredService<IEmailRepository>();

            for (var i = 0; i < 5; i++)
                await repository.AddAsync(new Email
                {
                    Content = new EmailContent
                    {
                        Address = "test@test.test"
                    },
                    Info = new EmailInfo
                    {
                        Date = DateTime.Now,
                        UniversalId = Guid.NewGuid()
                    }
                });

            //Act
            var result = await service.GetThrottlingStateAsync();

            //Assert
            Assert.Equal("test@test.test", result.LastAddress);
        }

        [Fact]
        public async void UpdateEmailAsync()
        {
            //Arrange
            var service = _provider.GetRequiredService<IDatabaseManagerService>();
            var oldEmail = new Email
            {
                Info = new EmailInfo
                {
                    Date = DateTime.Now,
                    UniversalId = Guid.Empty
                }
            };
            var repository = _provider.GetRequiredService<IEmailRepository>();
            await repository.AddAsync(oldEmail);
            oldEmail.Info.UniversalId = Guid.NewGuid();

            //Act
            await service.UpdateEmailAsync(oldEmail);

            //Assert
            Assert.NotEqual(Guid.Empty, (await repository.GetFirstAsync()).Info.UniversalId);
        }
    }
}