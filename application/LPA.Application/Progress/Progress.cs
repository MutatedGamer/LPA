using ElectronNET.API;
using LPA.UI;
using LPA.UI.Tags;

namespace LPA.Application.Progress
{
    internal class Progress
        : IProgress
    {
        private const string address = "/progressBar";

        private readonly ProgressTag dataTag;

        const int width = 500;
        const int height = 75;

        private readonly ILpaWindowManager lpaWindowManager;
        private BrowserWindow? window;

        public Guid Id
        {
            get;
            private set;
        }

        public event EventHandler? Cancelled;

        public bool HasCancelledHandler => Cancelled != null;

        public string? Label { get; private set; }

        public string? CancelText { get; private set; }

        public event EventHandler? Finished;

        public Progress(ILpaWindowManager lpaWindowManager)
        {
            this.lpaWindowManager = lpaWindowManager;

            this.Id = Guid.NewGuid();
            this.dataTag = new ProgressTag(this.Id);
        }

        public async Task Start(string label, string cancelText)
        {
            this.Label = label;
            this.CancelText = cancelText;
            await CreateProgressWindow();
        }

        public Task Finish()
        {
            CloseProgressWindow();
            Finished?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        public Task Cancel()
        {
            Cancelled?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        protected void InvalidateData()
        {
            TagInvalidator.InvalidateTag(this.dataTag);
        }

        private async Task CreateProgressWindow()
        {
            this.window = await this.lpaWindowManager.CreateModalWindow($"{address}/{this.Id}");

            this.window.SetContentSize(width, height);
            this.window.SetResizable(false);
            this.window.SetMovable(false);
            this.window.SetTitle("Loading...");
        }

        private void CloseProgressWindow()
        {
            if (this.window == null)
            {
                throw new InvalidOperationException("Cannot call Finish before Start");
            }

            this.window.Destroy();
        }
    }
}
