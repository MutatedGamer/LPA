namespace LPA.UI.Tags
{
    /// <summary>
    /// Tag that represents the data of a Session Table View.
    /// </summary>
    public class SessionTableViewDataTag
        : ITagWithId
    {
        public SessionTableViewDataTag(Guid viewId)
        {
            this.Id = viewId;
        }

        public string Type => "SessionTableViewData";

        public Guid Id
        {
            get;
            private set;
        }
    }
}
