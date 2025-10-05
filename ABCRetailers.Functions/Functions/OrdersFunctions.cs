using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;
using Azure.Storage.Queues;
using System.Text.Json;
using ABCRetailers.Functions.Entities;
using ABCRetailers.Functions.Models;
using ABCRetailers.Functions.Helpers;

namespace ABCRetailers.Functions.Functions;

public class OrdersFunctions
{
    private readonly TableServiceClient _tableService;
    private readonly QueueServiceClient _queueService;
    private readonly ILogger<OrdersFunctions> _logger;

    public OrdersFunctions(TableServiceClient tableService, QueueServiceClient queueService, ILogger<OrdersFunctions> logger)
    {
        _tableService = tableService;
        _queueService = queueService;
        _logger = logger;
    }

    [Function("GetOrders")]
    public async Task<HttpResponseData> GetOrders(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders")] HttpRequestData req)
    {
        var tableClient = _tableService.GetTableClient("Orders");
        var orders = new List<OrderDto>();

        await foreach (var entity in tableClient.QueryAsync<OrderEntity>())
        {
            orders.Add(Map.ToOrderDto(entity));
        }

        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(orders);
        return response;
    }

    [Function("CreateOrder")]
    public async Task<HttpResponseData> CreateOrder(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders")] HttpRequestData req)
    {
        var request = await req.ReadFromJsonAsync<CreateOrderRequest>();

        // Get customer and product details
        var customersTable = _tableService.GetTableClient("Customers");
        var productsTable = _tableService.GetTableClient("Products");

        var customer = await customersTable.GetEntityAsync<CustomerEntity>("Customer", request.customerId);
        var product = await productsTable.GetEntityAsync<ProductEntity>("Product", request.productId);

        // Create order message for queue
        var orderMessage = new OrderMessage
        {
            OrderId = Guid.NewGuid().ToString(),
            CustomerId = request.customerId,
            CustomerName = $"{customer.Value.Name} {customer.Value.Surname}",
            ProductId = request.productId,
            ProductName = product.Value.ProductName,
            Quantity = request.quantity,
            UnitPrice = product.Value.Price,
            TotalPrice = product.Value.Price * request.quantity
        };

        // Send to queue for processing
        var queueClient = _queueService.GetQueueClient("order-processing");
        await queueClient.SendMessageAsync(JsonSerializer.Serialize(orderMessage));

        var response = req.CreateResponse(System.Net.HttpStatusCode.Accepted);
        await response.WriteAsJsonAsync(new
        {
            message = "Order submitted for processing",
            orderId = orderMessage.OrderId
        });
        return response;
    }

    [Function("UpdateOrderStatus")]
    public async Task<HttpResponseData> UpdateOrderStatus(
        [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "orders/{id}/status")] HttpRequestData req,
        string id)
    {
        var statusRequest = await req.ReadFromJsonAsync<UpdateOrderStatusRequest>();
        var tableClient = _tableService.GetTableClient("Orders");

        var entity = await tableClient.GetEntityAsync<OrderEntity>("Order", id);
        entity.Value.Status = statusRequest.status;

        await tableClient.UpdateEntityAsync(entity.Value, entity.Value.ETag);

        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(Map.ToOrderDto(entity));
        return response;
    }
}

public class OrderMessage
{
    public string OrderId { get; set; }
    public string CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class UpdateOrderStatusRequest
{
    public string status { get; set; }
}