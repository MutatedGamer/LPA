using ElectronNET.API;
using ElectronNET.API.Entities;

namespace LPA.UI
{
    public class LpaWindowManager
        : ILpaWindowManager
    {
        private bool isInitialized = false;
        private BrowserWindow? mainWindow = null;

        public BrowserWindow MainWindow
        {
            get
            {
                if (this.mainWindow == null)
                {
                    throw new InvalidOperationException("Not initialized.");
                }

                return this.mainWindow;
            }
        }

        public void Initialize(BrowserWindow mainWindow)
        {
            if (this.isInitialized)
            {
                throw new InvalidOperationException("Cannot initialize multiple times.");
            }

            this.isInitialized = true;
            this.mainWindow = mainWindow;
        }

        public async Task<BrowserWindow> CreateNewWindow(string path)
        {
            var window = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Show = false,
                Parent = this.MainWindow
            },
            loadUrl: await ConstructUrl(path));

            AddReadyToShowListener(window);

            return window;
        }

        public async Task<BrowserWindow> CreateModalWindow(string path)
        {
            var window = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Show = false,
                Modal = true,
                Parent = this.MainWindow,
                AutoHideMenuBar = true,
                Closable = false,
                Maximizable = false,
                Minimizable = false,
            },
            loadUrl: await ConstructUrl(path));

            window.SetMenuBarVisibility(false);

            AddReadyToShowListener(window);

            return window;
        }

        private void AddReadyToShowListener(BrowserWindow window)
        {
            async void Show()
            {
                window.OnReadyToShow -= Show;

                await CenterWindowIfNotMaximized(window);
                window.Show();
            }

            window.OnReadyToShow += Show;
        }

        private async Task CenterWindowIfNotMaximized(BrowserWindow window)
        {
            if (await window.IsMaximizedAsync())
            {
                return;
            }

            var mainPos = await this.MainWindow.GetPositionAsync();
            var mainSize = await this.MainWindow.GetSizeAsync();

            var windowSize = await window.GetSizeAsync();
            var width = windowSize[0];
            var height = windowSize[1];

            window.SetPosition(mainPos[0] + (mainSize[0] / 2) - (width / 2), mainPos[1] + (mainSize[1] / 2) - (height / 2));
        }

        private async Task<string> ConstructUrl(string path)
        {
            var url = await this.MainWindow.WebContents.GetUrl();
            return new Uri(url).GetLeftPart(UriPartial.Authority) + path;
        }
    }
}
