namespace Local3DModelRepository.Models
{
    public interface IModelFactory
    {
        /// <summary>
        /// Creates a new <seealso cref="IModel"/> object.
        /// </summary>
        /// <param name="fullPath">The full path to the file that contains the model.</param>
        /// <returns>A new <seealso cref="IModel"/> object.</returns>
        IModel Create(string fullPath);
    }
}
