using LPA.Application.SDK;
using LPA.Common;

namespace LPA.Application.PluginLoading
{
    internal class PluginLoader
        : IPluginLoader
    {
        private readonly List<string> loadedPlugins = new List<string>();

        private readonly IUserInput userInput;
        private readonly ISdk sdk;

        public PluginLoader(ISdk sdk, IUserInput userInput)
        {
            this.sdk = sdk;
            this.userInput = userInput;
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
            await LoadPluginAsync(path);
        }

        public async Task LoadPluginAsync(string path)
        {
            this.loadedPlugins.Add(path);

            await this.sdk.LoadPlugin(path);

            PluginLoaded?.Invoke(this, EventArgs.Empty);
        }
    }
}
