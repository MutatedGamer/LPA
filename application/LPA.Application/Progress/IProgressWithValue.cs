namespace LPA.Application.Progress
{
    public interface IProgressWithValue
        : IProgress
    {
        void SetValue(int newValue);
    }
}
