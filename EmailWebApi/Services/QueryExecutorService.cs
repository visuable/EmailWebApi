using System.Threading;
using System.Threading.Tasks;
using EmailWebApi.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmailWebApi.Services
{
    public class QueryExecutorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public QueryExecutorService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                try
                {
                    var emailTransferService = scope.ServiceProvider.GetRequiredService<IEmailTransferService>();
                    var databaseManagerService = scope.ServiceProvider.GetRequiredService<IDatabaseManagerService>();
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<QueryExecutorService>>();
                    var queryMessages = await databaseManagerService.GetEmailsByStatusAsync(EmailStatus.Query);
                    foreach (var message in queryMessages)
                    {
                        logger.LogDebug($"Отправка {message.Id} в фоновом режиме");
                        await emailTransferService.Send(message);
                        await Task.Delay(5000, stoppingToken);
                    }
                }
                catch
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<QueryExecutorService>>();
                    logger.LogError("Ошибка фоновой отправки сообщения");
                }
            }
        }
    }
}