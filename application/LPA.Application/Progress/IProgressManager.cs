using LPA.UI.ResponseObjects.Progress;

namespace LPA.Application.Progress
{
    public interface IProgressManager
    {
        public Task<ProgressState?> GetProgressStateAsync(Guid id);

        public Task CancelAsync(Guid id);

        public IProgress CreateIndefiniteProgress();

        public IProgressWithValue CreateProgressWithValue();
    }
}