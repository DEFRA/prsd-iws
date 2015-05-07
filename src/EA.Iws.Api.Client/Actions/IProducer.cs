namespace EA.Iws.Api.Client.Actions
{
    using System;
    using System.Threading.Tasks;
    using Entities;

    public interface IProducer
    {
        Task<Response<string>> CreateProducer(string accessToken, ProducerData producerData);
    }
}
