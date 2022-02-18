using ElectronNET.API;
using ElectronNET.API.Entities;
using LPA.Application;
using LPA.Server;
using LPA.Server.Menus;

var builder = WebApplication.CreateBuilder(args);

await Task.Delay(10000);

// Add services to the container.
builder.Services.AddSingleton<MainMenu, MainMenu>();

builder.Services.AddMvc().AddControllersAsServices(ServiceLifetime.Singleton);
builder.Services.AddElectron();
LpaApplication.ConfigureServices(args, builder.Host);

builder.WebHost.UseElectron(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "localhost",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
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
    pattern: "{controller}/{action=Index}");

app.MapFallbackToFile("index.html");



Task.Run(async () =>
{
    await BootstrapElectron();
});

async Task BootstrapElectron()
{
    var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
    {
        Show = false,
        Width = 950,
    });

    await browserWindow.WebContents.Session.ClearCacheAsync();

    browserWindow.OnReadyToShow += () =>
    {
        app.Services.GetService<MainMenu>()!.SetMainMenu();
        browserWindow.Show();
    };

    browserWindow.OnShow += () =>
    {
        if (builder.Environment.IsDevelopment())
        {
            browserWindow.WebContents.OpenDevTools();
        }
    };
}

app.Run();
