namespace LPA.UI.Tags
{
    /// <summary>
    /// Tag that represents all of the views a Session Table currently has.
    /// </summary>
    public class SessionTableViewsTag
        : ITagWithId
    {
        public SessionTableViewsTag(Guid sessionId)
        {
            this.Id = sessionId;
        }

        public string Type => "SessionTableViews";

        public Guid Id
        {
            get;
            private set;
        }
    }
}
