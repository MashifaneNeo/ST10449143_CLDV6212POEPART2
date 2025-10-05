// Controllers/CustomerController.cs
using Microsoft.AspNetCore.Mvc;
using ST10449143_CLDV6212_POEPART1.Models;
using ST10449143_CLDV6212_POEPART1.Services;

namespace ST10449143_CLDV6212_POEPART1.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IFunctionsApi _functionsApi;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IFunctionsApi functionsApi, ILogger<CustomerController> logger)
        {
            _functionsApi = functionsApi;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            try
            {
                var customers = await _functionsApi.GetCustomersAsync();

                if (!string.IsNullOrEmpty(searchString))
                {
                    customers = customers.Where(c =>
                        c.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        c.Surname.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        c.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        c.Username.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                ViewBag.SearchString = searchString;
                return View(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading customers");
                TempData["Error"] = "Unable to load customers. Please try again.";
                return View(new List<CustomerDto>());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _functionsApi.CreateCustomerAsync(request);
                    TempData["Success"] = "Customer created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating customer");
                    ModelState.AddModelError("", $"Error creating customer: {ex.Message}");
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
                var customer = await _functionsApi.GetCustomerAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                var request = new UpdateCustomerRequest
                {
                    Name = customer.Name,
                    Surname = customer.Surname,
                    Username = customer.Username,
                    Email = customer.Email,
                    ShippingAddress = customer.ShippingAddress
                };

                ViewBag.CustomerId = customer.Id;
                return View(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading customer for edit");
                TempData["Error"] = "Unable to load customer. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UpdateCustomerRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _functionsApi.UpdateCustomerAsync(id, request);
                    TempData["Success"] = "Customer updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating customer");
                    ModelState.AddModelError("", $"Error updating customer: {ex.Message}");
                }
            }

            ViewBag.CustomerId = id;
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _functionsApi.DeleteCustomerAsync(id);
                TempData["Success"] = "Customer deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer");
                TempData["Error"] = $"Error deleting customer: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}