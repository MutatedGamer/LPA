namespace LPA.UI.Tags
{
    /// <summary>
    /// Tag that represents the columns of a Session Table View.
    /// </summary>
    public class SessionTableViewColumnsTag
        : ITagWithId
    {
        public SessionTableViewColumnsTag(Guid viewId)
        {
            this.Id = viewId;
        }

        public string Type => "SessionTableViewColumns";

        public Guid Id
        {
            get;
            private set;
        }
    }
}
