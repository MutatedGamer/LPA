namespace LPA.Common
{
    public interface IUserInput
    {
        Task<string?> GetFolder();

        Task<string?> GetFile();

        Task<string?> GetFile((string name, string[] filters)[]? fileTypeFilters);

        Task<string?> GetSaveAsFile((string name, string[] filters)[]? fileTypeFilters);
    }
}
