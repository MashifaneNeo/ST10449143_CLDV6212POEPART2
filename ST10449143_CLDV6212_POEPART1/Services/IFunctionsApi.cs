// Services/IFunctionsApi.cs
using ST10449143_CLDV6212_POEPART1.Models;

namespace ST10449143_CLDV6212_POEPART1.Services
{
    public interface IFunctionsApi
    {
        // Customer operations
        Task<List<CustomerDto>> GetCustomersAsync();
        Task<CustomerDto> GetCustomerAsync(string id);
        Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest request);
        Task<CustomerDto> UpdateCustomerAsync(string id, UpdateCustomerRequest request); 
        Task DeleteCustomerAsync(string id);

        // Product operations  
        Task<List<ProductDto>> GetProductsAsync();
        Task<ProductDto> GetProductAsync(string id);
        Task<ProductDto> CreateProductAsync(CreateProductRequest request);
        Task<ProductDto> UpdateProductAsync(string id, UpdateProductRequest request); 
        Task DeleteProductAsync(string id);

        // Order operations
        Task<List<OrderDto>> GetOrdersAsync();
        Task<OrderDto> GetOrderAsync(string id);
        Task<OrderDto> CreateOrderAsync(CreateOrderRequest request);
        Task<OrderDto> UpdateOrderStatusAsync(string id, string status);
        Task DeleteOrderAsync(string id);

        // File operations
        Task<FileUploadResult> UploadFileAsync(FileUploadRequest request);
    }
}