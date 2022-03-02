namespace LPA.Application.AvailableTables
{
    /// <summary>
    ///     A table that is provided by a loaded plugin. An <see cref="IAvailableTable"/> does NOT
    ///     represent a table with data that can be presented to a user. It ONLY represents a table
    ///     that can be created once a file is opened.
    /// </summary>
    public interface IAvailableTable
    {
        Guid Guid { get; }

        string PluginDirectory { get; }

        string Name { get; }

        string Description { get; }

        string Category { get; }
    }
}