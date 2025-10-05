using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;
using Azure.Storage.Queues;
using System.Text.Json;
using ABCRetailers.Functions.Entities;

namespace ABCRetailers.Functions.Functions;

public class QueueProcessorFunctions
{
    private readonly TableServiceClient _tableService;
    private readonly ILogger<QueueProcessorFunctions> _logger;

    public QueueProcessorFunctions(TableServiceClient tableService, ILogger<QueueProcessorFunctions> logger)
    {
        _tableService = tableService;
        _logger = logger;
    }

    [Function("ProcessOrderQueue")]
    public async Task ProcessOrderQueue(
        [QueueTrigger("order-processing", Connection = "AzureWebJobsStorage")] string queueItem)
    {
        try
        {
            _logger.LogInformation($"Processing order from queue: {queueItem}");

            var orderData = JsonSerializer.Deserialize<OrderMessage>(queueItem);

            var tableClient = _tableService.GetTableClient("Orders");
            await tableClient.CreateIfNotExistsAsync();

            var orderEntity = new OrderEntity
            {
                PartitionKey = "Order",
                RowKey = orderData.OrderId,
                CustomerId = orderData.CustomerId,
                CustomerName = orderData.CustomerName,
                ProductId = orderData.ProductId,
                ProductName = orderData.ProductName,
                Quantity = orderData.Quantity,
                UnitPrice = orderData.UnitPrice,
                TotalPrice = orderData.TotalPrice,
                Status = "Processing",
                OrderDate = DateTime.UtcNow
            };

            await tableClient.UpsertEntityAsync(orderEntity);

            _logger.LogInformation($"Order {orderData.OrderId} processed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing order: {ex.Message}");
            throw;
        }
    }
}