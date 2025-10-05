using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;
using ABCRetailers.Functions.Entities;
using ABCRetailers.Functions.Models;
using ABCRetailers.Functions.Helpers;

namespace ABCRetailers.Functions.Functions;

public class CustomersFunctions
{
    private readonly TableServiceClient _tableService;
    private readonly ILogger<CustomersFunctions> _logger;

    public CustomersFunctions(TableServiceClient tableService, ILogger<CustomersFunctions> logger)
    {
        _tableService = tableService;
        _logger = logger;
    }

    [Function("GetCustomers")]
    public async Task<HttpResponseData> GetCustomers(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "customers")] HttpRequestData req)
    {
        _logger.LogInformation("Getting all customers");

        var tableClient = _tableService.GetTableClient("Customers");
        var customers = new List<CustomerDto>();

        await foreach (var entity in tableClient.QueryAsync<CustomerEntity>())
        {
            customers.Add(Map.ToCustomerDto(entity));
        }

        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(customers);
        return response;
    }

    [Function("GetCustomer")]
    public async Task<HttpResponseData> GetCustomer(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "customers/{id}")] HttpRequestData req,
        string id)
    {
        var tableClient = _tableService.GetTableClient("Customers");
        var entity = await tableClient.GetEntityAsync<CustomerEntity>("Customer", id);

        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(Map.ToCustomerDto(entity));
        return response;
    }

    [Function("CreateCustomer")]
    public async Task<HttpResponseData> CreateCustomer(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "customers")] HttpRequestData req)
    {
        var request = await req.ReadFromJsonAsync<CreateCustomerRequest>();
        var tableClient = _tableService.GetTableClient("Customers");

        var entity = Map.ToCustomerEntity(request);
        await tableClient.AddEntityAsync(entity);

        var response = req.CreateResponse(System.Net.HttpStatusCode.Created);
        await response.WriteAsJsonAsync(Map.ToCustomerDto(entity));
        return response;
    }

    [Function("UpdateCustomer")]
    public async Task<HttpResponseData> UpdateCustomer(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "customers/{id}")] HttpRequestData req,
        string id)
    {
        var request = await req.ReadFromJsonAsync<CreateCustomerRequest>();
        var tableClient = _tableService.GetTableClient("Customers");

        var entity = await tableClient.GetEntityAsync<CustomerEntity>("Customer", id);
        entity.Value.Name = request.name;
        entity.Value.Surname = request.surname;
        entity.Value.Username = request.username;
        entity.Value.Email = request.email;
        entity.Value.ShippingAddress = request.shippingAddress;

        await tableClient.UpdateEntityAsync(entity.Value, entity.Value.ETag);

        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(Map.ToCustomerDto(entity));
        return response;
    }

    [Function("DeleteCustomer")]
    public async Task<HttpResponseData> DeleteCustomer(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "customers/{id}")] HttpRequestData req,
        string id)
    {
        var tableClient = _tableService.GetTableClient("Customers");
        await tableClient.DeleteEntityAsync("Customer", id);

        return req.CreateResponse(System.Net.HttpStatusCode.NoContent);
    }
}