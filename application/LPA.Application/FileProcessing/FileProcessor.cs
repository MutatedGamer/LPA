using LPA.Application.AvailableTables;
using LPA.Application.EnabledTables;
using LPA.Application.SDK;
using LPA.Common;

namespace LPA.Application.FileProcessing
{
    internal class FileProcessor
        : IFileProcessor
    {
        private readonly IUserInput userInput;
        private readonly ISdk sdk;
        private readonly IEnabledTablesManager enabledTablesManager;

        public FileProcessor(ISdk sdk, IEnabledTablesManager enabledTablesManager, IUserInput userInput)
        {
            this.sdk = sdk;
            this.enabledTablesManager = enabledTablesManager;
            this.userInput = userInput;
        }

        public async Task ProcessFile()
        {
            var file = await this.userInput.GetFile();
            if (string.IsNullOrEmpty(file))
            {
                return;
            }

            var availableTables = this.sdk.AvailableTables;
            List<IAvailableTable> enabledAvailableTables = new List<IAvailableTable>();

            await Parallel.ForEachAsync(availableTables, async (table, token) =>
            {
                var isEnabled = await this.enabledTablesManager.IsEnabledAsync(table.Guid);

                if (isEnabled)
                {
                    lock (enabledAvailableTables)
                    {
                        enabledAvailableTables.Add(table);
                    }
                }
            });

            await this.sdk.ProcessFile(file, enabledAvailableTables);
        }
    }
}
