namespace Local3DModelRepository.Models
{
    public sealed class ModelFactory : IModelFactory
    {
        public IModel Create(string fullPath)
            => new Model(fullPath);
    }
}
