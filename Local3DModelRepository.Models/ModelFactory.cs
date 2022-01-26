namespace Local3DModelRepository.Models
{
    internal sealed class ModelFactory : IModelFactory
    {
        public IModel Create(string fullPath)
            => new Model(fullPath);
    }
}