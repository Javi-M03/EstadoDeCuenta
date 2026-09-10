using CardsFrontend.Services;
using QuestPDF.Infrastructure;

// QuestPDF is free under the Community license for organizations with
// less than $1M USD annual revenue. See https://www.questpdf.com/license/
QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages
builder.Services.AddRazorPages();

// Typed HttpClient pointing at the Cards API.
// Update "ApiSettings:BaseUrl" in appsettings.json (or appsettings.Development.json)
// to match the port your API is running on in Swagger.
builder.Services.AddHttpClient("CardsApi", client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"]
        ?? "https://localhost:7263/";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<IApiClient, ApiClient>();
builder.Services.AddSingleton<IStatementExportService, StatementExportService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
