namespace LPA.Application.Progress
{
    public interface IProgress
    {
        event EventHandler Finished;

        /// <summary>
        /// Raised when the action this <see cref="IProgress"/> represents is requested
        /// to be cancelled. Handlers MUST call <see cref="Finish"/> to acknowledge the cancellation
        /// request.
        /// </summary>
        event EventHandler Cancelled;

        bool HasCancelledHandler { get; }

        Guid Id { get; }

        string? Label { get; }

        string? CancelText { get; }

        Task Start(string label, string cancelText);

        Task Finish();

        Task Cancel();
    }
}
