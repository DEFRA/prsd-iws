namespace EA.Iws.Api.Client.Serlializer
{
    public interface IJsonSerializer
    {
        T Deserialize<T>(string json);

        string Serialize(object item);
    }
}
