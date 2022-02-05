using ElectronNET.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.WebHost.UseElectron(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "localhost",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000");
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors("localhost");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

Task.Run(async () => await Electron.WindowManager.CreateWindowAsync());
app.Run();
