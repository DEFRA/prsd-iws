namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using System.Threading.Tasks;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class StateOfImportMap : IMap<Domain.StateOfImport, Core.StateOfImport>
    {
        private readonly IEntryOrExitPointRepository pointRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;

        public StateOfImportMap(ICompetentAuthorityRepository competentAuthorityRepository,
            IEntryOrExitPointRepository pointRepository)
        {
            this.competentAuthorityRepository = competentAuthorityRepository;
            this.pointRepository = pointRepository;
        }

        public Core.StateOfImport Map(Domain.StateOfImport source)
        {
            var competentAuthority = Task.Run(() => competentAuthorityRepository.GetById(source.CompetentAuthorityId)).Result;

            return new Core.StateOfImport
            {
                CompetentAuthorityCode = competentAuthority.Code,
                CompetentAuthorityName = competentAuthority.Name,
                EntryPointName = Task.Run(() => pointRepository.GetById(source.EntryPointId)).Result.Name
            };
        }
    }
}