namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using System.Threading.Tasks;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class StateOfExportMap : IMap<Domain.StateOfExport, Core.StateOfExport>
    {
        private readonly IEntryOrExitPointRepository pointRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;

        public StateOfExportMap(ICompetentAuthorityRepository competentAuthorityRepository,
            IEntryOrExitPointRepository pointRepository)
        {
            this.competentAuthorityRepository = competentAuthorityRepository;
            this.pointRepository = pointRepository;
        }

        public Core.StateOfExport Map(Domain.StateOfExport source)
        {
            var competentAuthority = Task.Run(() => competentAuthorityRepository.GetById(source.CompetentAuthorityId)).Result;
            var exitPoint = Task.Run(() => pointRepository.GetById(source.ExitPointId)).Result;

            return new Core.StateOfExport
            {
                CompetentAuthorityCode = competentAuthority.Code,
                CompetentAuthorityName = competentAuthority.Name,
                ExitPointName = exitPoint.Name,
                CountryName = exitPoint.Country.Name
            };
        }
    }
}