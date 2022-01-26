namespace Local3DModelRepository.Models
{
    internal class TagFactory : ITagFactory
    {
        public ITag Create(string value) => new Tag(value);
    }
}