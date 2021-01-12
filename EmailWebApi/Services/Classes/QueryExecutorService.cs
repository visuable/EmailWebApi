using System.Threading;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Settings;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmailWebApi.Services.Classes
{
    /// <summary>
    ///     Фоновый сервис отправки очереди сообщений.
    /// </summary>
    public class QueryExecutorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<QueryExecutorService> _logger;
        private readonly IOptions<QueryExecutorSettings> _options;

        public QueryExecutorService(IServiceScopeFactory scopeFactory, ILogger<QueryExecutorService> logger, IOptions<QueryExecutorSettings> options)
        {
            _scopeFactory = scopeFactory;
            _options = options;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                try
                {
                    var emailTransferService = scope.ServiceProvider.GetRequiredService<IEmailTransferService>();
                    var emailRepository = scope.ServiceProvider.GetRequiredService<IRepository<Email>>();

                    var queryMessages = await emailRepository.GetAllAsync(x => x.State.Status == EmailStatus.Query);
                    foreach (var message in queryMessages)
                    {
                        _logger.LogDebug($"Отправка {message.Id} в фоновом режиме");
                        await emailTransferService.Send(message);
                        await Task.Delay(_options.Value.Delay, stoppingToken);
                    }
                }
                catch
                {
                    _logger.LogError("Ошибка фоновой отправки сообщения");
                }
            }
        }
    }
}