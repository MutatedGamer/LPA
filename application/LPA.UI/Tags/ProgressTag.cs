namespace LPA.UI.Tags
{
    public class ProgressTag
        : ITagWithId
    {
        public ProgressTag(Guid id)
        {
            this.Id = id;
        }

        public string Type => "Progress";

        public Guid Id
        {
            get;
            private set;
        }
    }
}
