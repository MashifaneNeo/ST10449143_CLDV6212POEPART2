// Controllers/HomeController.cs
using ST10449143_CLDV6212_POEPART1.Models;
using ST10449143_CLDV6212_POEPART1.Models.ViewModels;
using ST10449143_CLDV6212_POEPART1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ST10449143_CLDV6212_POEPART1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFunctionsApi _functionsApi;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IFunctionsApi functionsApi, ILogger<HomeController> logger)
        {
            _functionsApi = functionsApi;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _functionsApi.GetProductsAsync();
                var customers = await _functionsApi.GetCustomersAsync();
                var orders = await _functionsApi.GetOrdersAsync();

                var viewModel = new HomeViewModel
                {
                    FeaturedProducts = products.Take(5).ToList(),
                    ProductCount = products.Count,
                    CustomerCount = customers.Count,
                    OrderCount = orders.Count,
                    PendingOrdersCount = orders.Count(o => o.Status == "Submitted" || o.Status == "Processing"),
                    TotalRevenue = orders.Where(o => o.Status == "Completed").Sum(o => o.TotalPrice)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard data");
                TempData["Error"] = "Unable to load dashboard data. Please try again.";
                return View(new HomeViewModel());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InitializeStorage()
        {
            try
            {
                // Test connectivity by making a simple API call
                await _functionsApi.GetCustomersAsync();
                TempData["Success"] = "System initialized successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize system");
                TempData["Error"] = $"Failed to initialize system: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}




