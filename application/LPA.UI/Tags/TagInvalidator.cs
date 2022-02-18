using ElectronNET.API;

namespace LPA.UI.Tags
{
    public static class TagInvalidator
    {
        public static bool InvalidateTag(ITag tag)
        {
            switch (tag)
            {
                case IStandardTag standardTag:
                    return InvalidateStandardTag(standardTag);
                default:
                    return false;
            }
        }

        private static bool InvalidateStandardTag(IStandardTag standardTag)
        {
            if (!Electron.WindowManager.BrowserWindows.Any())
            {
                return false;
            }

            var window = Electron.WindowManager.BrowserWindows.First();

            Electron.IpcMain.Send(
                    window,
                    "invalidate",
                    standardTag.Name);

            return true;
        }
    }
}
