namespace ABCRetailers.Functions.Models;

// Request DTOs
public class CreateCustomerRequest
{
    public string name { get; set; } = string.Empty;
    public string surname { get; set; } = string.Empty;
    public string username { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string shippingAddress { get; set; } = string.Empty;
}

public class CreateProductRequest
{
    public string productName { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public decimal price { get; set; }
    public int stockAvailable { get; set; }
}

public class CreateOrderRequest
{
    public string customerId { get; set; } = string.Empty;
    public string productId { get; set; } = string.Empty;
    public int quantity { get; set; }
}

// Response DTOs
public class CustomerDto
{
    public string id { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public string surname { get; set; } = string.Empty;
    public string username { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string shippingAddress { get; set; } = string.Empty;
}

public class ProductDto
{
    public string id { get; set; } = string.Empty;
    public string productName { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public decimal price { get; set; }
    public int stockAvailable { get; set; }
    public string imageUrl { get; set; } = string.Empty;
}

public class OrderDto
{
    public string id { get; set; } = string.Empty;
    public string customerId { get; set; } = string.Empty;
    public string customerName { get; set; } = string.Empty;
    public string productId { get; set; } = string.Empty;
    public string productName { get; set; } = string.Empty;
    public int quantity { get; set; }
    public decimal unitPrice { get; set; }
    public decimal totalPrice { get; set; }
    public string status { get; set; } = "Submitted";
    public DateTime orderDate { get; set; }
}