using Microsoft.AspNetCore.Mvc;
using ST10449143_CLDV6212_POEPART1.Models;
using ST10449143_CLDV6212_POEPART1.Services;

namespace ST10449143_CLDV6212_POEPART1.Controllers
{
    public class ProductController : Controller
    {
        private readonly IFunctionsApi _functionsApi;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IFunctionsApi functionsApi, ILogger<ProductController> logger)
        {
            _functionsApi = functionsApi;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchString, string categoryFilter)
        {
            try
            {
                var products = await _functionsApi.GetProductsAsync();

                if (!string.IsNullOrEmpty(searchString))
                {
                    products = products.Where(p =>
                        p.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        p.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                ViewBag.SearchString = searchString;
                ViewBag.CategoryFilter = categoryFilter;
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading products");
                TempData["Error"] = "Unable to load products. Please try again.";
                return View(new List<ProductDto>());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductRequest request, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (request.Price <= 0)
                    {
                        ModelState.AddModelError("Price", "Price must be greater than zero.");
                        return View(request);
                    }

                    // TODO: Handle image file upload through Functions API
                    // For now, we'll proceed without image
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // This would be handled by a separate file upload function
                        request.ImageUrl = $"/images/placeholder.jpg"; // Placeholder
                    }

                    await _functionsApi.CreateProductAsync(request);
                    TempData["Success"] = $"Product '{request.ProductName}' created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating product");
                    ModelState.AddModelError("", $"Error creating product: {ex.Message}");
                }
            }
            return View(request);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var product = await _functionsApi.GetProductAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                var request = new UpdateProductRequest
                {
                    ProductName = product.ProductName,
                    Description = product.Description,
                    Price = product.Price,
                    StockAvailable = product.StockAvailable,
                    ImageUrl = product.ImageUrl
                };

                ViewBag.ProductId = product.Id;
                return View(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product for edit");
                TempData["Error"] = "Unable to load product. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UpdateProductRequest request, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Handle image file upload through Functions API
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // This would be handled by a separate file upload function
                        request.ImageUrl = $"/images/updated-{id}.jpg"; // Placeholder
                    }

                    await _functionsApi.UpdateProductAsync(id, request);
                    TempData["Success"] = "Product updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating product");
                    ModelState.AddModelError("", $"Error updating product: {ex.Message}");
                }
            }

            ViewBag.ProductId = id;
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _functionsApi.DeleteProductAsync(id);
                TempData["Success"] = "Product deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                TempData["Error"] = $"Error deleting product: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
