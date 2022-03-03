using System.Data.SqlTypes;
using LPA.Application.Progress;
using LPA.Application.Sessions.Provider;
using LPA.Application.Sessions.Tables.Columns;
using LPA.Application.Sessions.Tables.TableConfigs;
using LPA.Common;
using LPA.UI.Tags;

namespace LPA.Application.Sessions.Tables.View
{
    internal class SessionTableView
        : ISessionTableView
    {
        private readonly Guid sessionTableId;
        private readonly ISessionTableConfigurationsManager configManager;
        private readonly ISessionProvider provider;

        private readonly IProgressManager progressManager;
        private readonly IUserInput userInput;

        private ITableConfiguration? currentConfig;

        private SessionTableView(
            Guid id,
            Guid sessionTableId,
            ISessionTableConfigurationsManager configurationsManager,
            ISessionProvider provider,
            IUserInput userInput,
            IProgressManager progressManager)
        {
            this.tableDataTag = new SessionTableViewDataTag(id);
            this.columnsTag = new SessionTableViewColumnsTag(id);

            this.Id = id;
            this.sessionTableId = sessionTableId;
            this.configManager = configurationsManager;
            this.provider = provider;
            this.userInput = userInput;
            this.progressManager = progressManager;
        }

        private readonly SessionTableViewDataTag tableDataTag;
        private readonly SessionTableViewColumnsTag columnsTag;

        public Guid Id { get; private set; }

        public static async Task<SessionTableView> Create(
            Guid id,
            Guid sessionTableId,
            Guid configId,
            ISessionTableConfigurationsManager configurationsManager,
            ISessionProvider provider,
            IUserInput userInput,
            IProgressManager progressManager)
        {
            var view = new SessionTableView(id, sessionTableId, configurationsManager, provider, userInput, progressManager);
            await view.SetCurrentConfigurationAsync(configId);
            return view;
        }

        public Task<Guid> GetCurrentConfigurationIdAsync()
        {
            return Task.FromResult(this.currentConfig?.Id ?? Guid.Empty);
        }

        public async Task SetCurrentConfigurationAsync(Guid configId)
        {
            this.currentConfig = await this.provider.GetConfigurationAsync(this.sessionTableId, configId);

            TagInvalidator.InvalidateTag(this.tableDataTag);
            TagInvalidator.InvalidateTag(this.columnsTag);
        }

        public async Task<int> GetRowCountAsync()
        {
            return await this.provider.GetRowCountAsync(this.sessionTableId);
        }

        public Task<IEnumerable<IColumn>> GetColumnsAsync()
        {
            // TODO: null check?
            return Task.FromResult(GetVisibleColumns());
        }

        public async Task<string[][]> GetRowsAsync(int start, int count)
        {
            return await this.provider.GetRowsAsync(this.sessionTableId, GetVisibleColumns().Select(col => col.SessionTableColumnGuid).ToList(), start, count);
        }

        public async Task ExportCsvAsync()
        {
            var savePath = await this.userInput.GetSaveAsFile(new (string name, string[] filters)[] { ("csv", new string[] { "csv" }) });
            if (string.IsNullOrEmpty(savePath))
            {
                return;
            }


            var rowCount = await GetRowCountAsync();

            using (var cts = new CancellationTokenSource())
            {
                var progress = this.progressManager.CreateProgressWithValue();

                progress.Cancelled += (s, e) =>
                {
                    cts.Cancel();
                    progress.Finish();
                };

                await progress.Start("Exporting CSV", "Cancel");

                using (var stream = new StreamWriter(savePath))
                {
                    var headers = GetVisibleColumns().Select(col => col.Name).ToArray();
                    var headersCsv = string.Join(",", MakeValuesCsvFriendly(headers));

                    stream.WriteLine(headersCsv);

                    var rows = await GetRowsAsync(0, rowCount);

                    for (int i = 0; i < rows.Length; i++)
                    {
                        var row = rows[i];
                        if (cts.Token.IsCancellationRequested)
                        {
                            break;
                        }

                        stream.WriteLine(string.Join(",", MakeValuesCsvFriendly(row)));

                        progress.Value = (float)((double)i / (double)rowCount);
                    }

                    stream.Flush();
                }

                if (cts.Token.IsCancellationRequested)
                {
                    File.Delete(savePath);
                }
                else
                {
                    await progress.Finish();
                }
            }

            return;
        }

        private IEnumerable<IColumn> GetVisibleColumns()
        {
            return this.currentConfig?.Columns?.Where(col => col.IsVisible) ?? new IColumn[0];
        }

        private static string[] MakeValuesCsvFriendly(object[] values, string columnSeparator = ",")
        {
            return values.Select(v => MakeValueCsvFriendly(v, columnSeparator)).ToArray();
        }

        /// <summary>
        /// Converts a value to how it should output in a csv file
        /// If it has a comma, it needs surrounding with double quotes
        /// Eg Sydney, Australia -> "Sydney, Australia"
        /// Also if it contains any double quotes ("), then they need to be replaced with quad quotes[sic] ("")
        /// Eg "Dangerous Dan" McGrew -> """Dangerous Dan"" McGrew"
        /// </summary>
        /// <param name="columnSeparator">
        /// The string used to separate columns in the output.
        /// By default this is a comma so that the generated output is a CSV document.
        /// </param>
        private static string MakeValueCsvFriendly(object value, string columnSeparator = ",")
        {
            if (value == null) return "";
            if (value is INullable && ((INullable)value).IsNull) return "";

            string output;
            if (value is DateTime)
            {
                if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                {
                    output = ((DateTime)value).ToString("yyyy-MM-dd");
                }
                else
                {
                    output = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            else
            {
                output = value.ToString().Trim();
            }

            if (output.Length > 30000) //cropping value for stupid Excel
                output = output.Substring(0, 30000);

            if (output.Contains(columnSeparator) || output.Contains("\"") || output.Contains("\n") || output.Contains("\r"))
                output = '"' + output.Replace("\"", "\"\"") + '"';

            return output;
        }
    }
}
