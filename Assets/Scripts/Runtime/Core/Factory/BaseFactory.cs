namespace Runtime.Core.Factory
{
    public abstract class BaseFactory<T>
    {
        public abstract T Create();
    }
}