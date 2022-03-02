using ElectronNET.API;

namespace LPA.UI
{
    public interface ILpaWindowManager
    {
        BrowserWindow MainWindow { get; }

        void Initialize(BrowserWindow mainWindow);

        Task<BrowserWindow> CreateNewWindow(string path);

        Task<BrowserWindow> CreateModalWindow(string path);
    }
}