using LPA.Application.Progress;
using LPA.Application.SDK;
using LPA.Common;

namespace LPA.Application.PluginLoading
{
    internal class PluginLoader
        : IPluginLoader
    {
        private readonly List<string> loadedPlugins = new List<string>();

        private readonly IUserInput userInput;
        private readonly IProgressManager progressManager;
        private readonly ISdk sdk;

        public PluginLoader(ISdk sdk, IUserInput userInput, IProgressManager progressManager)
        {
            this.sdk = sdk;
            this.userInput = userInput;
            this.progressManager = progressManager;
        }

        public IEnumerable<string> LoadedPlugins
        {
            get
            {
                return this.loadedPlugins;
            }
        }

        public event EventHandler? PluginLoaded;

        public async Task LoadPluginAsync()
        {
            var path = await this.userInput.GetFolder();

            if (path == null)
            {
                return;
            }

            var progress = this.progressManager.CreateIndefiniteProgress();

            await progress.Start("Loading plugin...", "Cancel");

            // TODO: don't do this - for demo only
            await Task.Delay(2000);

            await LoadPluginAsync(path);
            await progress.Finish();
        }

        public async Task LoadPluginAsync(string path)
        {
            this.loadedPlugins.Add(path);

            await this.sdk.LoadPlugin(path);

            PluginLoaded?.Invoke(this, EventArgs.Empty);
        }
    }
}
