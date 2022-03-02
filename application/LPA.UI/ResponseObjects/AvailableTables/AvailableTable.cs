namespace LPA.UI.ResponseObjects.AvailableTables
{
    public struct AvailableTable
    {
        public Guid Guid { get; set; }

        public string PluginDirectory { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }
    }
}