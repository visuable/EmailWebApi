using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmailWebApi.Objects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmailWebApi.Services
{
    public class QueryExecutorService : BackgroundService
    {
        private IServiceScopeFactory _scopeFactory;
        public QueryExecutorService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _email = scope.ServiceProvider.GetRequiredService<IEmailTransferService>();
                    var _manager = scope.ServiceProvider.GetRequiredService<IDatabaseManagerService>();
                    var queryMessages = await _manager.GetEmailsByStatus(EmailStatus.Query);
                    foreach (var message in queryMessages)
                    {
                        await _email.Send(message);
                        await Task.Delay(1000);
                    }
                } 
            }
        }
    }
}
