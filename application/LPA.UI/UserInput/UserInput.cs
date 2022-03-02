using ElectronNET.API;
using ElectronNET.API.Entities;
using LPA.Common;

namespace LPA.UI.UserInput
{
    public class UserInput
        : IUserInput
    {
        public Task<string?> GetFile()
        {
            return GetFile(null);
        }

        public async Task<string?> GetFile((string name, string[] filters)[]? fileTypeFilters)
        {
            var result = await Electron.Dialog.ShowOpenDialogAsync(Electron.WindowManager.BrowserWindows.First(), new OpenDialogOptions()
            {
                Properties = new OpenDialogProperty[]
                {
                    OpenDialogProperty.openFile
                },
                Filters = fileTypeFilters?.Select(filter => new FileFilter() { Name = filter.name, Extensions = filter.filters }).ToArray(),
            });

            return result.Any() ? result.First() : null;
        }

        public async Task<string?> GetFolder()
        {
            var result = await Electron.Dialog.ShowOpenDialogAsync(Electron.WindowManager.BrowserWindows.First(), new OpenDialogOptions()
            {
                Properties = new OpenDialogProperty[]
                {
                    OpenDialogProperty.openDirectory
                }
            });

            return result.Any() ? result.First() : null;
        }
    }
}
