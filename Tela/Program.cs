using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Configurar HttpClient
builder.Services.AddHttpClient("PacientesePlanos", client =>
{
    client.BaseAddress = new Uri("https://localhost:7294/"); // Definido corretamente
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

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
