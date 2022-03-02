namespace LPA.Application.EnabledTables
{
    public interface IEnabledTablesManager
    {
        /// <summary>
        /// Gets the enable status of an <see cref="LPA.Application.AvailableTables.IAvailableTable"/>.
        /// </summary>
        /// <param name="tableGuid">
        /// The <see cref="LPA.Application.AvailableTables.IAvailableTable.Guid"/> of the table being queried.
        /// </param>
        /// <returns>
        /// An await-able <see cref="Task"/> that returns the enabled status of the given table..
        /// </returns>
        Task<bool> IsEnabledAsync(Guid tableGuid);

        /// <summary>
        /// <para>
        /// Toggles the enabled status of the table with the given <paramref name="tableGuid"/>.
        /// </para>
        /// <para>
        /// If the table with the given <paramref name="tableGuid"/> has not been toggled before,
        /// a call to this method will set its enabled status to <c>false</c>.
        /// </para>
        /// </summary>
        /// <param name="tableGuid">
        /// The <see cref="Guid"/> of the table to toggle enablement for.
        /// </param>
        /// <returns>
        /// An await-able <see cref="Task"/> that performs the toggle operation.
        /// </returns>
        Task ToggleEnabledAsync(Guid tableGuid);

        /// <summary>
        /// Sets every known table to disabled.
        /// </summary>
        /// <returns>
        /// An await-able <see cref="Task"/> that performs the disable operation.
        /// </returns>
        Task DisableAll();
    }
}