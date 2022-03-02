namespace LPA.Application.Sessions.Tables.Columns
{
    public interface IColumn
    {
        public Guid SessionTableColumnGuid { get; }

        public bool IsVisible { get; }

        public string Name { get; }

        public bool IsLeftFrozen { get; }

        public bool IsRightFrozen { get; }

        public bool IsPivot { get; }

        public bool IsGraph { get; }
    }
}
