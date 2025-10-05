using Microsoft.AspNetCore.Mvc;
using ST10449143_CLDV6212_POEPART1.Models;
using ST10449143_CLDV6212_POEPART1.Services;
using System.Text.Json;

// Add these using statements
using ST10449143_CLDV6212_POEPART1.Models.ViewModels;
using OrderCreateViewModel = ST10449143_CLDV6212_POEPART1.Models.OrderCreateViewModel; // Alias for DTO version

namespace ST10449143_CLDV6212_POEPART1.Controllers
{
    public class OrderController : Controller
    {
        private readonly IAzureStorageService _storageService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IAzureStorageService storageService, ILogger<OrderController> logger)
        {
            _storageService = storageService;
            _logger = logger;
        }

        // GET: OrderController
        public async Task<IActionResult> Index(string searchString, string statusFilter)
        {
            try
            {
                var orders = await _storageService.GetAllEntitiesAsync<Order>();

                // Search functionality
                if (!string.IsNullOrEmpty(searchString))
                {
                    orders = orders.Where(o =>
                        o.Username.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        o.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        o.OrderId.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Status filter
                if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
                {
                    orders = orders.Where(o => o.Status == statusFilter).ToList();
                }

                ViewBag.SearchString = searchString;
                ViewBag.StatusFilter = statusFilter;
                ViewBag.StatusOptions = GetStatusOptions();

                return View(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading orders");
                TempData["Error"] = "Unable to load orders. Please try again.";
                return View(new List<Order>());
            }
        }

        // GET: OrderController/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var customers = await _storageService.GetAllEntitiesAsync<Customer>();
                var products = await _storageService.GetAllEntitiesAsync<Product>();

                // Convert to DTOs for the view model
                var customerDtos = customers.Select(c => new CustomerDto
                {
                    Id = c.RowKey,
                    Name = c.Name,
                    Surname = c.Surname,
                    Username = c.Username,
                    Email = c.Email,
                    ShippingAddress = c.ShippingAddress
                }).ToList();

                var productDtos = products.Select(p => new ProductDto
                {
                    Id = p.RowKey,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    StockAvailable = p.StockAvailable,
                    ImageUrl = p.ImageUrl
                }).ToList();

                var viewModel = new OrderCreateViewModel
                {
                    Customers = customerDtos,
                    Products = productDtos
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading order creation form");
                TempData["Error"] = "Unable to load order form. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var customer = await _storageService.GetEntityAsync<Customer>("Customer", model.CustomerId);
                    var product = await _storageService.GetEntityAsync<Product>("Product", model.ProductId);

                    if (customer == null || product == null)
                    {
                        ModelState.AddModelError("", "Invalid customer or product selected.");
                        await PopulateDropdowns(model);
                        return View(model);
                    }

                    if (product.StockAvailable < model.Quantity)
                    {
                        ModelState.AddModelError("Quantity", $"Insufficient stock. Available: {product.StockAvailable}");
                        await PopulateDropdowns(model);
                        return View(model);
                    }

                    // Create order
                    var order = new Order
                    {
                        CustomerId = model.CustomerId,
                        Username = customer.Username,
                        ProductId = model.ProductId,
                        ProductName = product.ProductName,
                        OrderDate = DateTime.SpecifyKind(model.OrderDate, DateTimeKind.Utc),
                        Quantity = model.Quantity,
                        UnitPrice = product.Price,
                        TotalPrice = product.Price * model.Quantity,
                        Status = "Submitted"
                    };

                    await _storageService.AddEntityAsync(order);

                    // Update product stock
                    product.StockAvailable -= model.Quantity;
                    await _storageService.UpdateEntityAsync(product);

                    // Send order notification message
                    var orderMessage = new
                    {
                        OrderId = order.OrderId,
                        CustomerId = order.CustomerId,
                        CustomerName = customer.Name + " " + customer.Surname,
                        ProductName = product.ProductName,
                        Quantity = order.Quantity,
                        TotalPrice = order.TotalPrice,
                        OrderDate = order.OrderDate,
                        Status = order.Status
                    };

                    await _storageService.SendMessageAsync("order-notifications", JsonSerializer.Serialize(orderMessage));

                    // Send stock update message
                    var stockMessage = new
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        PreviousStock = product.StockAvailable + model.Quantity,
                        NewStock = product.StockAvailable,
                        UpdatedBy = "Order System",
                        UpdateDate = DateTime.UtcNow
                    };

                    await _storageService.SendMessageAsync("stock-updates", JsonSerializer.Serialize(stockMessage));

                    TempData["Success"] = "Order created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating order");
                    ModelState.AddModelError("", $"Error creating order: {ex.Message}");
                }
            }

            await PopulateDropdowns(model);
            return View(model);
        }

        // GET: OrderController/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var order = await _storageService.GetEntityAsync<Order>("Order", id);
                if (order == null)
                {
                    return NotFound();
                }

                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading order details");
                TempData["Error"] = "Unable to load order details. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: OrderController/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var order = await _storageService.GetEntityAsync<Order>("Order", id);
                if (order == null)
                {
                    return NotFound();
                }

                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading order for edit");
                TempData["Error"] = "Unable to load order. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure OrderDate is UTC
                    order.OrderDate = DateTime.SpecifyKind(order.OrderDate, DateTimeKind.Utc);

                    await _storageService.UpdateEntityAsync(order);
                    TempData["Success"] = "Order updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating order");
                    ModelState.AddModelError("", $"Error updating order: {ex.Message}");
                }
            }
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _storageService.DeleteEntityAsync<Order>("Order", id);
                TempData["Success"] = "Order deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order");
                TempData["Error"] = $"Error deleting order: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> GetProductPrice(string productId)
        {
            try
            {
                var product = await _storageService.GetEntityAsync<Product>("Product", productId);
                if (product != null)
                {
                    return Json(new
                    {
                        success = true,
                        price = product.Price,
                        stock = product.StockAvailable,
                        productName = product.ProductName
                    });
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product price");
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(string id, string newStatus)
        {
            try
            {
                var order = await _storageService.GetEntityAsync<Order>("Order", id);
                if (order == null)
                {
                    return Json(new { success = false, message = "Order not found" });
                }

                var previousStatus = order.Status;
                order.Status = newStatus;
                await _storageService.UpdateEntityAsync(order);

                // Send queue message for status update
                var statusMessage = new
                {
                    OrderId = order.OrderId,
                    CustomerId = order.CustomerId,
                    CustomerName = order.Username,
                    ProductName = order.ProductName,
                    PreviousStatus = previousStatus,
                    NewStatus = newStatus,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = "System"
                };

                await _storageService.SendMessageAsync("order-notifications", JsonSerializer.Serialize(statusMessage));

                return Json(new { success = true, message = $"Order status updated to {newStatus}" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order status");
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task PopulateDropdowns(OrderCreateViewModel model)
        {
            try
            {
                var customers = await _storageService.GetAllEntitiesAsync<Customer>();
                var products = await _storageService.GetAllEntitiesAsync<Product>();

                model.Customers = customers.Select(c => new CustomerDto
                {
                    Id = c.RowKey,
                    Name = c.Name,
                    Surname = c.Surname,
                    Username = c.Username,
                    Email = c.Email,
                    ShippingAddress = c.ShippingAddress
                }).ToList();

                model.Products = products.Select(p => new ProductDto
                {
                    Id = p.RowKey,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    StockAvailable = p.StockAvailable,
                    ImageUrl = p.ImageUrl
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error populating dropdowns");
                model.Customers = new List<CustomerDto>();
                model.Products = new List<ProductDto>();
            }
        }

        private List<OrderStatusDto> GetStatusOptions()
        {
            return new List<OrderStatusDto>
            {
                new() { Value = "All", DisplayName = "All Statuses", BadgeClass = "secondary" },
                new() { Value = "Submitted", DisplayName = "Submitted", BadgeClass = "primary" },
                new() { Value = "Processing", DisplayName = "Processing", BadgeClass = "warning" },
                new() { Value = "Completed", DisplayName = "Completed", BadgeClass = "success" },
                new() { Value = "Cancelled", DisplayName = "Cancelled", BadgeClass = "danger" }
            };
        }
    }
}