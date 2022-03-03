using LPA.Application.AvailableTables;
using LPA.Application.EnabledTables;
using LPA.Application.Progress;
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
        private readonly IProgressManager progressManager;

        public FileProcessor(ISdk sdk, IEnabledTablesManager enabledTablesManager, IUserInput userInput, IProgressManager progressManager)
        {
            this.sdk = sdk;
            this.enabledTablesManager = enabledTablesManager;
            this.userInput = userInput;
            this.progressManager = progressManager;
        }

        public async Task ProcessFile()
        {
            var file = await this.userInput.GetFile();
            if (string.IsNullOrEmpty(file))
            {
                return;
            }

            var progress = this.progressManager.CreateIndefiniteProgress();
            await progress.Start("Processing...", "Cancel");

            // TODO: don't do this - for demo only
            await Task.Delay(2000);

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

            // TODO: do somethng with this result
            await this.sdk.ProcessFile(file, enabledAvailableTables);

            await progress.Finish();
        }
    }
}
