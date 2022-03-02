namespace LPA.UI.Tags
{
    public interface ITagWithId
        : ITag
    {
        public Guid Id { get; }
    }
}
