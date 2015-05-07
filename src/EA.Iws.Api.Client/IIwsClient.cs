namespace EA.Iws.Api.Client
{
    using Actions;
    using System;

    public interface IIwsClient : IDisposable
    {
        IRegistration Registration { get; }

        INotification Notification { get; }

        IProducer Producer { get; }
    }
}