namespace LPA.Application.Progress
{
    public interface IProgressWithValue
        : IProgress
    {
        float Value { get; set; }
    }
}
