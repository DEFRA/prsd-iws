namespace EA.Iws.Web.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Areas.NotificationApplication.ViewModels.TransitState;
    using Prsd.Core.Mapper;
    using Requests.TransitState;
    using ViewModels.Shared;

    public class TransitStateWithTransportRouteDataMap : IMap<TransitStateWithTransportRouteData, TransitStateViewModel>
    {
        public TransitStateViewModel Map(TransitStateWithTransportRouteData source)
        {
            var model = new TransitStateViewModel
            {
                ShowNextSection = source.TransitState != null,
                TransitStateCountryIds = source.TransitStates.Select(ts => ts.Country.Id).ToArray()
            };

            if (model.ShowNextSection)
            {
                model.CountryId = source.TransitState.Country.Id;
                model.Countries = new SelectList(source.Countries, "Id", "Name", model.CountryId);

                model.EntryPointId = source.TransitState.EntryPoint.Id;
                model.ExitPointId = source.TransitState.ExitPoint.Id;

                model.EntryOrExitPoints = new SelectList(source.EntryOrExitPoints, "Id", "Name");

                model.CompetentAuthorities = new StringGuidRadioButtons(source.CompetentAuthorities
                    .Select(ca => new KeyValuePair<string, Guid>(ca.Name, ca.Id)));

                model.CompetentAuthorities.SelectedValue = source.TransitState.CompetentAuthority.Id;
            }
            else
            {
                model.Countries = new SelectList(source.Countries, "Id", "Name");
            }

            if (source.StateOfExport != null)
            {
                model.StateOfExportCountryId = source.StateOfExport.Country.Id;
            }

            if (source.StateOfImport != null)
            {
                model.StateOfImportCountryId = source.StateOfImport.Country.Id;
            }

            return model;
        }
    }
}