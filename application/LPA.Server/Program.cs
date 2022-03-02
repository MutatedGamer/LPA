using ElectronNET.API;
using ElectronNET.API.Entities;
using LPA.Application;
using LPA.Server;
using LPA.Server.Menus;
using LPA.UI;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<MainMenu, MainMenu>();
builder.Services.AddSingleton<ILpaWindowManager, LpaWindowManager>();

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
    var mainWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
    {
        Show = false,
        Width = 950,
    });

    app.Services.GetService<ILpaWindowManager>()!.Initialize(mainWindow);

    await mainWindow.WebContents.Session.ClearCacheAsync();

    mainWindow.OnReadyToShow += () =>
    {
        app.Services.GetService<MainMenu>()!.SetMainMenu();
        mainWindow.Show();
    };

    mainWindow.OnShow += async () =>
    {
        if (builder.Environment.IsDevelopment())
        {
            mainWindow.WebContents.OpenDevTools();
        }
    };
}

app.Run();
