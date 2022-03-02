namespace LPA.UI.ResponseObjects.Tables
{
    public struct Column
    {
        public Guid SessionTableColumnGuid { get; set; }

        public string Name { get; set; }

        public bool IsLeftFrozen { get; set; }

        public bool IsRightFrozen { get; set; }

        public bool IsPivot { get; set; }

        public bool IsGraph { get; set; }
    }
}
