namespace LPA.UI.Tags
{
    public class AvailableTablesTag
        : IStandardTag
    {
        private static readonly AvailableTablesTag instance = new AvailableTablesTag();

        private AvailableTablesTag()
        {
        }

        public static AvailableTablesTag Instance
        {
            get
            {
                return instance;
            }
        }

        public string Name => "AvailableTables";
    }
}
