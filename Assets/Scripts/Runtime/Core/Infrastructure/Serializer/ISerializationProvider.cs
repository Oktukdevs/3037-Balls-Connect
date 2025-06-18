namespace Runtime.Core.Infrastructure.Serializer
{
    public interface ISerializationProvider
    {
        T Deserialize<T>(string text) where T : class;
        string Serialize<T>(T obj) where T : class;
    }
}