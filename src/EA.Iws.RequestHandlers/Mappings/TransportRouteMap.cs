﻿namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Shared;
    using Core.StateOfExport;
    using Core.StateOfImport;
    using Core.TransitState;
    using Core.TransportRoute;
    using DataAccess;
    using Domain;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;

    internal class TransportRouteMap : IMapWithParameter<TransportRoute, Guid, StateOfExportWithTransportRouteData>
    {
        private readonly IwsContext context;
        private readonly IMap<StateOfImport, StateOfImportData> stateOfImportMapper;
        private readonly IMap<StateOfExport, StateOfExportData> stateOfExportMapper;
        private readonly IMap<IEnumerable<TransitState>, IList<TransitStateData>> transitStateMapper;
        private readonly IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper;
        private readonly IMap<Country, CountryData> countryMapper;
        private readonly IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper;
        private readonly IIntraCountryExportAllowedRepository intraCountryExportAllowedRepository;
        private readonly IUnitedKingdomCompetentAuthorityRepository unitedKingdomCompetentAuthorityRepository;

        public TransportRouteMap(IwsContext context,
            IMap<StateOfImport, StateOfImportData> stateOfImportMapper,
            IMap<StateOfExport, StateOfExportData> stateOfExportMapper,
            IMap<IEnumerable<TransitState>, IList<TransitStateData>> transitStateMapper,
            IMap<EntryOrExitPoint, EntryOrExitPointData> entryOrExitPointMapper,
            IMap<Country, CountryData> countryMapper,
            IMap<CompetentAuthority, CompetentAuthorityData> competentAuthorityMapper,
             IIntraCountryExportAllowedRepository intraCountryExportAllowedRepository,
             IUnitedKingdomCompetentAuthorityRepository unitedKingdomCompetentAuthorityRepository)
        {
            this.context = context;
            this.stateOfImportMapper = stateOfImportMapper;
            this.stateOfExportMapper = stateOfExportMapper;
            this.transitStateMapper = transitStateMapper;
            this.entryOrExitPointMapper = entryOrExitPointMapper;
            this.countryMapper = countryMapper;
            this.competentAuthorityMapper = competentAuthorityMapper;
            this.intraCountryExportAllowedRepository = intraCountryExportAllowedRepository;
            this.unitedKingdomCompetentAuthorityRepository = unitedKingdomCompetentAuthorityRepository;
        }

        public StateOfExportWithTransportRouteData Map(TransportRoute source, Guid parameter)
        {
            var data = new StateOfExportWithTransportRouteData();

            var countries = context.Countries.OrderBy(c => c.Name).ToArray();
            var notification = context.NotificationApplications.Single(n => n.Id == parameter);

            data.Countries = countries.Select(countryMapper.Map).ToArray();

            if (source != null)
            {
                data.StateOfImport = stateOfImportMapper.Map(source.StateOfImport);
                data.StateOfExport = stateOfExportMapper.Map(source.StateOfExport);
                data.TransitStates = transitStateMapper.Map(source.TransitStates);
            }

            if (data.StateOfExport == null)
            {
                data.StateOfExport = new StateOfExportData();

                var ukcompAuth = this.unitedKingdomCompetentAuthorityRepository.GetAll().Single(ca => ca.Id == (int)notification.CompetentAuthority);

                data.StateOfExport.Country = countryMapper.Map(countries.Single(c => c.Name == UnitedKingdomCompetentAuthority.CountryName));

                data.StateOfExport.CompetentAuthority = competentAuthorityMapper.Map(context.CompetentAuthorities.Single(ca => ca.Id == ukcompAuth.CompetentAuthority.Id));
            }

            var entryPoints = context.EntryOrExitPoints.Where(ep => ep.Country.Id == data.StateOfExport.Country.Id).ToArray();
            data.ExitPoints = entryPoints.Select(entryOrExitPointMapper.Map).ToArray();

            return data;
        }
    }
}