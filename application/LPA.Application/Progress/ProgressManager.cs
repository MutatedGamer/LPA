using System.Collections.Concurrent;
using LPA.UI;
using LPA.UI.ResponseObjects.Progress;

namespace LPA.Application.Progress
{
    internal class ProgressManager
        : IProgressManager
    {
        private readonly ILpaWindowManager lpaWindowManager;
        private readonly ConcurrentDictionary<Guid, IProgress> activeProgresses = new();

        public ProgressManager(ILpaWindowManager lpaWindowManager)
        {
            this.lpaWindowManager = lpaWindowManager;
        }

        public IProgress CreateIndefiniteProgress()
        {
            var progress = new Progress(this.lpaWindowManager);
            SetupProgress(progress);
            return progress;
        }

        public IProgressWithValue CreateProgressWithValue()
        {
            var progress = new ProgressWithValue(this.lpaWindowManager);
            SetupProgress(progress);
            return progress;
        }

        private void SetupProgress(IProgress progress)
        {
            this.activeProgresses.TryAdd(progress.Id, progress);
            progress.Finished += OnProgressFinished;
        }

        private void OnProgressFinished(object? sender, EventArgs e)
        {
            if (!(sender is IProgress progress))
            {
                throw new InvalidOperationException();
            }

            this.activeProgresses.Remove(progress.Id, out var _);
            progress.Finished -= OnProgressFinished;
        }

        public Task<ProgressState?> GetProgressStateAsync(Guid id)
        {
            if (this.activeProgresses.TryGetValue(id, out var progress))
            {
                return Task.FromResult<ProgressState?>(
                    new ProgressState()
                    {
                        Value = (progress is ProgressWithValue progressWithValue) ? progressWithValue.Value : null,
                        CancelText = progress.CancelText ?? "Cancel",
                        Label = progress.Label ?? "Loading...",
                        CanCancel = progress.HasCancelledHandler
                    });
            }

            return Task.FromResult<ProgressState?>(null);
        }

        public Task CancelAsync(Guid id)
        {
            if (this.activeProgresses.TryGetValue(id, out var progress))
            {
                progress.Cancel();
            }

            return Task.CompletedTask;
        }
    }
}
