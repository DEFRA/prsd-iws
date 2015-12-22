namespace EA.Iws.RequestHandlers.Annexes
{
    using System.Threading.Tasks;
    using Core.Annexes;
    using Core.Annexes.ExportNotification;
    using Domain.NotificationApplication.Annexes;
    using Prsd.Core.Mediator;
    using Requests.Annexes;

    internal class GetAnnexesToBeProvidedHandler : IRequestHandler<GetAnnexesToBeProvided, ProvidedAnnexesData>
    {
        private readonly RequiredAnnexes requiredAnnexes;
        private readonly IAnnexCollectionRepository annexCollectionRepository;

        public GetAnnexesToBeProvidedHandler(RequiredAnnexes requiredAnnexes, 
            IAnnexCollectionRepository annexCollectionRepository)
        {
            this.requiredAnnexes = requiredAnnexes;
            this.annexCollectionRepository = annexCollectionRepository;
        }

        public async Task<ProvidedAnnexesData> HandleAsync(GetAnnexesToBeProvided message)
        {
            var annexRequirements = await requiredAnnexes.Get(message.NotificationId);
            var annexCollection = await annexCollectionRepository.GetByNotificationId(message.NotificationId);

            return new ProvidedAnnexesData
            {
                ProcessOfGeneration = new AnnexStatus
                {
                    IsRequired = annexRequirements.IsProcessOfGenerationRequired,
                    FileId = annexCollection.ProcessOfGeneration.FileId
                },
                TechnologyEmployed = new AnnexStatus
                {
                    IsRequired = annexRequirements.IsTechnologyEmployedRequired,
                    FileId = annexCollection.TechnologyEmployed.FileId
                },
                WasteComposition = new AnnexStatus
                {
                    IsRequired = annexRequirements.IsWasteCompositionRequired,
                    FileId = annexCollection.WasteComposition.FileId
                }
            };
        }
    }
}
