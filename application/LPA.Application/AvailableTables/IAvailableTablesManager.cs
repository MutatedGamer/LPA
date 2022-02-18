namespace LPA.Application.AvailableTables
{
    public interface IAvailableTablesManager
    {
        /// <summary>
        /// Gets all tables that have been discovered by loaded plugins.
        /// </summary>
        /// <returns>
        /// An await-able <see cref="Task"/> whose result is all available tables.
        /// </returns>
        Task<IEnumerable<IAvailableTable>> GetAvailableTablesAsync();
    }
}
