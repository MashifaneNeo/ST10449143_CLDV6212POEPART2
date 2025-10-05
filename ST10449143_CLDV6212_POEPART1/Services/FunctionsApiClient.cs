using System.Text.Json;
using ST10449143_CLDV6212_POEPART1.Models;

namespace ST10449143_CLDV6212_POEPART1.Services
{
    public class FunctionsApiClient : IFunctionsApi
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FunctionsApiClient> _logger;

        public FunctionsApiClient(HttpClient httpClient, ILogger<FunctionsApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Customer operations
        public async Task<List<CustomerDto>> GetCustomersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/customers");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<CustomerDto>>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }) ?? new List<CustomerDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customers");
                throw;
            }
        }

        public async Task<CustomerDto> GetCustomerAsync(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/customers/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<CustomerDto>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer {Id}", id);
                throw;
            }
        }

        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/customers", request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<CustomerDto>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer");
                throw;
            }
        }

        public async Task<CustomerDto> UpdateCustomerAsync(string id, UpdateCustomerRequest request)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/customers/{id}", request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<CustomerDto>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer {Id}", id);
                throw;
            }
        }

        public async Task DeleteCustomerAsync(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/customers/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer {Id}", id);
                throw;
            }
        }

        // Product operations
        public async Task<List<ProductDto>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/products");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ProductDto>>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }) ?? new List<ProductDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products");
                throw;
            }
        }

        public async Task<ProductDto> GetProductAsync(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/products/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product {Id}", id);
                throw;
            }
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/products", request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                throw;
            }
        }

        public async Task<ProductDto> UpdateProductAsync(string id, UpdateProductRequest request)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/products/{id}", request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {Id}", id);
                throw;
            }
        }

        public async Task DeleteProductAsync(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/products/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {Id}", id);
                throw;
            }
        }

        // Order operations
        public async Task<List<OrderDto>> GetOrdersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/orders");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<OrderDto>>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }) ?? new List<OrderDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders");
                throw;
            }
        }

        public async Task<OrderDto> GetOrderAsync(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/orders/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderDto>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order {Id}", id);
                throw;
            }
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/orders", request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderDto>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                throw;
            }
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(string id, string status)
        {
            try
            {
                var updateRequest = new { status };
                var response = await _httpClient.PatchAsJsonAsync($"api/orders/{id}/status", updateRequest);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderDto>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order status {Id}", id);
                throw;
            }
        }

        public async Task DeleteOrderAsync(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/orders/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order {Id}", id);
                throw;
            }
        }

        // File operations (placeholder implementation)
        public async Task<FileUploadResult> UploadFileAsync(FileUploadRequest request)
        {
            // TODO: Implement file upload functionality
            await Task.Delay(100); // Simulate API call
            return new FileUploadResult
            {
                Success = true,
                FileName = "placeholder.jpg",
                FileUrl = "/images/placeholder.jpg",
                Message = "File upload functionality to be implemented"
            };
        }
    }
}