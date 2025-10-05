using ABCRetailers.Functions.Entities;
using ABCRetailers.Functions.Models;

namespace ABCRetailers.Functions.Helpers;

public static class Map
{
    public static CustomerDto ToCustomerDto(CustomerEntity entity) => new()
    {
        id = entity.RowKey,
        name = entity.Name,
        surname = entity.Surname,
        username = entity.Username,
        email = entity.Email,
        shippingAddress = entity.ShippingAddress
    };

    public static CustomerEntity ToCustomerEntity(CreateCustomerRequest request) => new()
    {
        RowKey = Guid.NewGuid().ToString(),
        Name = request.name,
        Surname = request.surname,
        Username = request.username,
        Email = request.email,
        ShippingAddress = request.shippingAddress
    };

    public static ProductDto ToProductDto(ProductEntity entity) => new()
    {
        id = entity.RowKey,
        productName = entity.ProductName,
        description = entity.Description,
        price = entity.Price,
        stockAvailable = entity.StockAvailable,
        imageUrl = entity.ImageUrl
    };

    public static OrderDto ToOrderDto(OrderEntity entity) => new()
    {
        id = entity.RowKey,
        customerId = entity.CustomerId,
        customerName = entity.CustomerName,
        productId = entity.ProductId,
        productName = entity.ProductName,
        quantity = entity.Quantity,
        unitPrice = entity.UnitPrice,
        totalPrice = entity.TotalPrice,
        status = entity.Status,
        orderDate = entity.OrderDate
    };
}