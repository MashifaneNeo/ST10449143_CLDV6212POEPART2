using ST10449143_CLDV6212_POEPART1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register Azure Functions API service BEFORE builder.Build()
builder.Services.AddHttpClient<IFunctionsApi, FunctionsApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Functions:BaseUrl"] ?? "http://localhost:7071");
});

// Add Logging
builder.Services.AddLogging();

// Set the culture for decimal handling
var culture = new System.Globalization.CultureInfo("en-US");
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;

var app = builder.Build(); // This makes the service collection read-only

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();