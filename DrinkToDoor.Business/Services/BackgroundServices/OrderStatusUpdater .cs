
using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Business.Services.BackgroundServices
{
    public class OrderStatusUpdater : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OrderStatusUpdater> _logger;

        public OrderStatusUpdater(IServiceScopeFactory scopeFactory, ILogger<OrderStatusUpdater> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateOrdersAsync();
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Kiểm tra mỗi 5 giây
            }
        }

        private async Task UpdateOrdersAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DrinkToDoorDbContext>();

            var now = DateTime.UtcNow;

            var orders = await dbContext.Orders
                .Where(o => o.Status == EnumOrderStatus.WAITING_DELIVER || o.Status == EnumOrderStatus.DELIVERING || o.Status == EnumOrderStatus.DELIVERED)
                .ToListAsync();

            foreach (var order in orders)
            {
                if (order.Status == EnumOrderStatus.WAITING_DELIVER && (now - order.StatusChangedAt).TotalSeconds >= 30)
                {
                    _logger.LogInformation($"Order {order.Id} | Status: {order.Status} | UpdatedAt: {order.StatusChangedAt} | Now: {now}");
                    order.Status = EnumOrderStatus.DELIVERING;
                    order.StatusChangedAt = now;
                }
                else if (order.Status == EnumOrderStatus.DELIVERING && (now - order.StatusChangedAt).TotalSeconds >= 30)
                {
                    _logger.LogInformation($"Order {order.Id} | Status: {order.Status} | UpdatedAt: {order.StatusChangedAt} | Now: {now}");
                    order.Status = EnumOrderStatus.DELIVERED;
                    order.StatusChangedAt = now;
                }
                else if (order.Status == EnumOrderStatus.DELIVERED && (now - order.StatusChangedAt).TotalSeconds >= 30)
                {
                    _logger.LogInformation($"Order {order.Id} | Status: {order.Status} | UpdatedAt: {order.StatusChangedAt} | Now: {now}");
                    order.Status = EnumOrderStatus.SUCCESS;
                    order.StatusChangedAt = now;
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}