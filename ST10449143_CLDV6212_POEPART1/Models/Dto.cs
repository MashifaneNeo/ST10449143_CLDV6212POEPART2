using System.ComponentModel.DataAnnotations;

namespace ST10449143_CLDV6212_POEPART1.Models;

// ========== CUSTOMER DTOs ==========

// Request DTO for creating customer
public class CreateCustomerRequest
{
    [Required(ErrorMessage = "Name is required")]
    [Display(Name = "First Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Surname is required")]
    [Display(Name = "Last Name")]
    public string Surname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Username is required")]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Shipping address is required")]
    [Display(Name = "Shipping Address")]
    public string ShippingAddress { get; set; } = string.Empty;
}

// Request DTO for updating customer
public class UpdateCustomerRequest
{
    [Required(ErrorMessage = "Name is required")]
    [Display(Name = "First Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Surname is required")]
    [Display(Name = "Last Name")]
    public string Surname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Username is required")]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Shipping address is required")]
    [Display(Name = "Shipping Address")]
    public string ShippingAddress { get; set; } = string.Empty;
}

// Response DTO for customer
public class CustomerDto
{
    public string Id { get; set; } = string.Empty;

    [Display(Name = "First Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Last Name")]
    public string Surname { get; set; } = string.Empty;

    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Shipping Address")]
    public string ShippingAddress { get; set; } = string.Empty;
}

// ========== PRODUCT DTOs ==========

// Request DTO for creating product
public class CreateProductRequest
{
    [Required(ErrorMessage = "Product name is required")]
    [Display(Name = "Product Name")]
    public string ProductName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    [Display(Name = "Price")]
    public double Price { get; set; }

    [Required(ErrorMessage = "Stock quantity is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
    [Display(Name = "Stock Available")]
    public int StockAvailable { get; set; }

    [Display(Name = "Image URL")]
    public string ImageUrl { get; set; } = string.Empty;
}

// Request DTO for updating product
public class UpdateProductRequest
{
    [Required(ErrorMessage = "Product name is required")]
    [Display(Name = "Product Name")]
    public string ProductName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    [Display(Name = "Price")]
    public double Price { get; set; }

    [Required(ErrorMessage = "Stock quantity is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
    [Display(Name = "Stock Available")]
    public int StockAvailable { get; set; }

    [Display(Name = "Image URL")]
    public string ImageUrl { get; set; } = string.Empty;
}

// Response DTO for product
public class ProductDto
{
    public string Id { get; set; } = string.Empty;

    [Display(Name = "Product Name")]
    public string ProductName { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Price")]
    [DataType(DataType.Currency)]
    public double Price { get; set; }

    [Display(Name = "Stock Available")]
    public int StockAvailable { get; set; }

    [Display(Name = "Image URL")]
    public string ImageUrl { get; set; } = string.Empty;
}

// ========== ORDER DTOs ==========

// Request DTO for creating order
public class CreateOrderRequest
{
    [Required(ErrorMessage = "Customer is required")]
    [Display(Name = "Customer")]
    public string CustomerId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Product is required")]
    [Display(Name = "Product")]
    public string ProductId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    [Display(Name = "Quantity")]
    public int Quantity { get; set; }
}

// Request DTO for updating order status
public class UpdateOrderStatusRequest
{
    [Required(ErrorMessage = "Status is required")]
    [Display(Name = "Status")]
    public string Status { get; set; } = "Submitted";
}

// Response DTO for order
public class OrderDto
{
    public string Id { get; set; } = string.Empty;

    [Display(Name = "Customer ID")]
    public string CustomerId { get; set; } = string.Empty;

    [Display(Name = "Customer Name")]
    public string CustomerName { get; set; } = string.Empty;

    [Display(Name = "Product ID")]
    public string ProductId { get; set; } = string.Empty;

    [Display(Name = "Product Name")]
    public string ProductName { get; set; } = string.Empty;

    [Display(Name = "Quantity")]
    public int Quantity { get; set; }

    [Display(Name = "Unit Price")]
    [DataType(DataType.Currency)]
    public double UnitPrice { get; set; }

    [Display(Name = "Total Price")]
    [DataType(DataType.Currency)]
    public double TotalPrice { get; set; }

    [Display(Name = "Status")]
    public string Status { get; set; } = "Submitted";

    [Display(Name = "Order Date")]
    [DataType(DataType.DateTime)]
    public DateTime OrderDate { get; set; }
}

// ========== FILE UPLOAD DTOs ==========

// Request DTO for file upload
public class FileUploadRequest
{
    [Required(ErrorMessage = "File is required")]
    [Display(Name = "Proof of Payment")]
    public IFormFile? File { get; set; }

    [Display(Name = "Order ID")]
    public string? OrderId { get; set; }

    [Display(Name = "Customer Name")]
    public string? CustomerName { get; set; }
}

// Response DTO for file upload result
public class FileUploadResult
{
    public bool Success { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

// ========== SEARCH DTOs ==========

// Request DTO for search operations
public class SearchRequest
{
    public string SearchString { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

// Response DTO for search results
public class SearchResult<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

// ========== API RESPONSE DTOs ==========

// Generic API response wrapper
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}

// API response for operations that don't return data
public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new List<string>();
}

// ========== VIEW MODEL DTOs ==========

// ViewModel for order creation (extends the request DTO)
public class OrderCreateViewModel : CreateOrderRequest
{
    public List<CustomerDto> Customers { get; set; } = new List<CustomerDto>();
    public List<ProductDto> Products { get; set; } = new List<ProductDto>();

    // Additional properties for the view
    [Display(Name = "Order Date")]
    [DataType(DataType.Date)]
    public DateTime OrderDate { get; set; } = DateTime.Today;
}

// ViewModel for home dashboard
public class HomeViewModel
{
    public List<ProductDto> FeaturedProducts { get; set; } = new List<ProductDto>();
    public int CustomerCount { get; set; }
    public int ProductCount { get; set; }
    public int OrderCount { get; set; }
    public int PendingOrdersCount { get; set; }
    public double TotalRevenue { get; set; }
}

// ========== STATUS DTOs ==========

// DTO for order status updates with history
public class OrderStatusUpdateDto
{
    public string OrderId { get; set; } = string.Empty;
    public string PreviousStatus { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    public string UpdatedBy { get; set; } = "System";
}

// DTO for order status options
public class OrderStatusDto
{
    public string Value { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string BadgeClass { get; set; } = string.Empty;
}

// ========== INVENTORY DTOs ==========

// DTO for inventory updates
public class InventoryUpdateDto
{
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int PreviousStock { get; set; }
    public int NewStock { get; set; }
    public int ChangeAmount { get; set; }
    public string Reason { get; set; } = string.Empty; // "Order", "Restock", "Adjustment"
    public DateTime UpdateDate { get; set; } = DateTime.UtcNow;
}

// ========== ERROR DTOs ==========

// DTO for error responses
public class ErrorDto
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public string Instance { get; set; } = string.Empty;
    public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}