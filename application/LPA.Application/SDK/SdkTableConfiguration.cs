using LPA.Application.Sessions.Tables.Columns;
using LPA.Application.Sessions.Tables.TableConfigs;
using Microsoft.Performance.SDK.Processing;

namespace LPA.Application.SDK
{
    internal class SdkTableConfiguration
        : ITableConfiguration
    {
        private readonly TableConfiguration tableConfiguration;

        public SdkTableConfiguration(Guid id, TableConfiguration tableConfiguration)
        {
            this.tableConfiguration = tableConfiguration;

            this.Name = tableConfiguration.Name;
            this.Id = id;

            this.LeftFreezeColumnIndex = -1;
            this.RightFreezeColumnIndex = -1;
            this.PivotColumnIndex = -1;
            this.GraphColumnIndex = -1;

            var columns = new List<SdkColumn>();

            var i = -1;
            foreach (var column in tableConfiguration.Columns.Where(col => col.DisplayHints.IsVisible))
            {
                i++;

                switch (column.Metadata.Guid)
                {
                    case var guid when (guid == TableConfiguration.LeftFreezeColumn.Metadata.Guid):
                        this.LeftFreezeColumnIndex = i;
                        foreach (var priorColumm in columns)
                        {
                            priorColumm.IsLeftFrozen = true;
                        }
                        break;
                    case var guid when (guid == TableConfiguration.RightFreezeColumn.Metadata.Guid):
                        this.RightFreezeColumnIndex = i;
                        break;
                    case var guid when (guid == TableConfiguration.PivotColumn.Metadata.Guid):
                        this.PivotColumnIndex = i;
                        foreach (var priorColumm in columns)
                        {
                            priorColumm.IsGraph = true;
                        }
                        break;
                    case var guid when (guid == TableConfiguration.GraphColumn.Metadata.Guid):
                        this.GraphColumnIndex = i;
                        break;
                    default:
                        var sdkColumn = new SdkColumn()
                        {
                            SessionTableColumnGuid = column.Metadata.Guid,
                            Name = column.Metadata.Name,
                            IsGraph = this.GraphColumnIndex != -1,
                            IsRightFrozen = this.RightFreezeColumnIndex != -1,
                            IsVisible = column.DisplayHints.IsVisible,
                        };
                        columns.Add(sdkColumn);
                        break;
                }
            }

            this.Columns = columns.ToArray();
        }

        public Guid Id { get; }

        public int LeftFreezeColumnIndex { get; }

        public int RightFreezeColumnIndex { get; }

        public int PivotColumnIndex { get; }

        public int GraphColumnIndex { get; }

        public IColumn[] Columns { get; }

        public string Name { get; }
    }
}