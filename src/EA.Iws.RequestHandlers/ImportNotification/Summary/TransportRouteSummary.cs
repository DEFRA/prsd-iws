namespace EA.Iws.RequestHandlers.ImportNotification.Summary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotification.Summary;
    using DataAccess.Draft;
    using Draft = Core.ImportNotification.Draft;

    internal class TransportRouteSummary
    {
        private readonly Domain.ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly IDraftImportNotificationRepository draftRepository;
        private readonly Domain.TransportRoute.IEntryOrExitPointRepository entryOrExitPointRepository;

        public TransportRouteSummary(Domain.ICompetentAuthorityRepository competentAuthorityRepository,
            IDraftImportNotificationRepository draftRepository,
            Domain.TransportRoute.IEntryOrExitPointRepository entryOrExitPointRepository)
        {
            this.competentAuthorityRepository = competentAuthorityRepository;
            this.draftRepository = draftRepository;
            this.entryOrExitPointRepository = entryOrExitPointRepository;
        }

        public async Task<TransportRouteData> GetTransportRoute(Guid id,
            IList<Domain.Country> countries)
        {
            var idContainer = new TransportRouteIdContainer();

            var stateOfImport = await draftRepository.GetDraftData<Draft.StateOfImport>(id);
            var stateOfExport = await draftRepository.GetDraftData<Draft.StateOfExport>(id);
            var transitStateCollection = await draftRepository.GetDraftData<Draft.TransitStateCollection>(id);

            /* 
            * We must find the ids for the competent authorities and entry or exit points used by
            * the state of export, import and transit states.
            * Since these are all nullable we use a series of methods to gather the non-null ids we need
            * to load data for. This is so we can load the data in 2 calls rather than multiple calls.
            */
            AddTransitStateIds(transitStateCollection, idContainer);
            AddStateOfExportIds(stateOfExport, idContainer);
            AddStateOfImportIds(stateOfImport, idContainer);

            var competentAuthorities = await competentAuthorityRepository.GetByIds(idContainer.CompetentAuthorityIds);
            var entryOrExitPoints = await entryOrExitPointRepository.GetByIds(idContainer.EntryOfExitPointIds);
            var lookups = new TransportRouteLookups(countries, competentAuthorities, entryOrExitPoints);

            // Use the loaded competent authorities and entry or exit points to retrieve names for summary data.
            return new TransportRouteData(GenerateTransitStates(transitStateCollection, lookups),
                GenerateStateOfExport(stateOfExport, lookups),
                GenerateStateOfImport(stateOfImport, lookups),
                transitStateCollection.HasNoTransitStates);
        }

        private void AddTransitStateIds(Draft.TransitStateCollection transitStateCollection,
            TransportRouteIdContainer idContainer)
        {
            if (transitStateCollection != null && transitStateCollection.TransitStates != null)
            {
                idContainer.AddCompetentAuthorities(transitStateCollection.TransitStates.Select(ts => ts.CompetentAuthorityId));
                idContainer.AddEntryOrExitPoints(transitStateCollection.TransitStates.Select(ts => ts.EntryPointId));
                idContainer.AddEntryOrExitPoints(transitStateCollection.TransitStates.Select(ts => ts.ExitPointId));
            }
        }

        private void AddStateOfExportIds(Draft.StateOfExport stateOfExport, TransportRouteIdContainer idContainer)
        {
            if (stateOfExport != null)
            {
                idContainer.AddCompetentAuthority(stateOfExport.CompetentAuthorityId);
                idContainer.AddEntryOrExitPoint(stateOfExport.ExitPointId);
            }
        }

        private void AddStateOfImportIds(Draft.StateOfImport stateOfImport, TransportRouteIdContainer idContainer)
        {
            if (stateOfImport != null)
            {
                idContainer.AddCompetentAuthority(stateOfImport.CompetentAuthorityId);
                idContainer.AddEntryOrExitPoint(stateOfImport.EntryPointId);
            }
        }

        private StateOfExport GenerateStateOfExport(Draft.StateOfExport stateOfExport,
            TransportRouteLookups lookups)
        {
            var returnValue = new StateOfExport();

            if (stateOfExport.CompetentAuthorityId.HasValue)
            {
                var competentAuthority = lookups.GetCompetentAuthority(stateOfExport.CompetentAuthorityId);

                returnValue.CompetentAuthorityName = competentAuthority.Name;
                returnValue.CompetentAuthorityCode = competentAuthority.Code;
            }

            if (stateOfExport.CountryId.HasValue)
            {
                returnValue.CountryName = lookups.GetCountry(stateOfExport.CountryId).Name;
            }

            if (stateOfExport.ExitPointId.HasValue)
            {
                returnValue.ExitPointName = lookups.GetEntryOrExitPoint(stateOfExport.ExitPointId).Name;
            }

            return returnValue;
        }

        private StateOfImport GenerateStateOfImport(Draft.StateOfImport stateOfImport,
            TransportRouteLookups lookups)
        {
            var returnValue = new StateOfImport();

            if (stateOfImport.CompetentAuthorityId.HasValue)
            {
                var competentAuthority = lookups.GetCompetentAuthority(stateOfImport.CompetentAuthorityId);

                returnValue.CompetentAuthorityCode = competentAuthority.Code;
                returnValue.CompetentAuthorityName = competentAuthority.Name;
            }

            if (stateOfImport.EntryPointId.HasValue)
            {
                returnValue.EntryPointName = lookups.GetEntryOrExitPoint(stateOfImport.EntryPointId).Name;
            }

            return returnValue;
        }

        private IList<TransitState> GenerateTransitStates(Draft.TransitStateCollection transitStates,
            TransportRouteLookups lookups)
        {
            if (transitStates == null || transitStates.TransitStates == null || transitStates.HasNoTransitStates)
            {
                return new TransitState[0];
            }

            return transitStates.TransitStates.Select(ts => GenerateTransitState(ts, lookups)).ToArray();
        }

        private TransitState GenerateTransitState(Draft.TransitState transitState, TransportRouteLookups lookups)
        {
            var returnValue = new TransitState();

            if (transitState.CompetentAuthorityId.HasValue)
            {
                var competentAuthority = lookups.GetCompetentAuthority(transitState.CompetentAuthorityId);

                returnValue.CompetentAuthorityCode = competentAuthority.Code;
                returnValue.CompetentAuthorityName = competentAuthority.Name;
            }

            if (transitState.CountryId.HasValue)
            {
                returnValue.CountryName = lookups.GetCountry(transitState.CountryId).Name;
            }

            if (transitState.EntryPointId.HasValue)
            {
                returnValue.EntryPointName = lookups.GetEntryOrExitPoint(transitState.EntryPointId).Name;
            }

            if (transitState.ExitPointId.HasValue)
            {
                returnValue.ExitPointName = lookups.GetEntryOrExitPoint(transitState.ExitPointId).Name;
            }

            return returnValue;
        }
    }
}
