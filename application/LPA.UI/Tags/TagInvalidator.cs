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
                case ITagWithId tagWithId:
                    return InvalidateTagWithId(tagWithId);
                default:
                    return false;
            }
        }

        private static bool InvalidateStandardTag(IStandardTag standardTag)
        {
            SendToAllWindows("invalidate", standardTag);
            return true;
        }

        private static bool InvalidateTagWithId(ITagWithId tagWithId)
        {
            SendToAllWindows("invalidate", tagWithId);
            return true;
        }

        private static void SendToAllWindows(string channel, params object[] data)
        {
            foreach (var window in Electron.WindowManager.BrowserWindows)
            {
                Electron.IpcMain.Send(
                    window,
                    channel,
                    data);
            }
        }
    }
}
