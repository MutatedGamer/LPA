namespace LPA.Application.AvailableTables
{
    public interface IAvailableTable
    {
        Guid Guid { get; }

        string PluginDirectory { get; }

        string Name { get; }

        string Description { get; }

        string Category { get; }
    }
}