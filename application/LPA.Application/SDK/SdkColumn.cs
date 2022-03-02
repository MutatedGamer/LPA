using LPA.Application.Sessions.Tables.Columns;

namespace LPA.Application.SDK
{
    internal class SdkColumn
        : IColumn
    {
        public Guid SessionTableColumnGuid { get; set; }

        public bool IsVisible { get; set; }

        public string Name { get; set; }

        public bool IsLeftFrozen { get; set; }

        public bool IsRightFrozen { get; set; }

        public bool IsPivot { get; set; }

        public bool IsGraph { get; set; }
    }
}
