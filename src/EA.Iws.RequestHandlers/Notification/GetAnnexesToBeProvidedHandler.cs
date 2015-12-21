namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using Core.Notification;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetAnnexesToBeProvidedHandler : IRequestHandler<GetAnnexesToBeProvided, ProvidedAnnexesData>
    { 
        public Task<ProvidedAnnexesData> HandleAsync(GetAnnexesToBeProvided message)
        {
            throw new System.NotImplementedException();
        }
    }
}
