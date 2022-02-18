using LPA.Application.AvailableTables;
using Microsoft.Performance.SDK.Processing;

namespace LPA.Application.SDK
{
    internal record class SdkTableInfo
        : IAvailableTable
    {
        public Guid Guid { get; }

        public string PluginDirectory { get; }
        public string Name { get; }
        public string Description { get; }
        public string Category { get; }

        public SdkTableInfo(TableDescriptor tableDescriptor, string loadDirectory)
        {
            this.Guid = tableDescriptor.Guid;
            this.PluginDirectory = loadDirectory;
            this.Category = tableDescriptor.Category;
            this.Name = tableDescriptor.Name;
            this.Description = tableDescriptor.Description;
        }
    }
}
