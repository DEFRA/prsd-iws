namespace EA.Iws.Web.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Areas.NotificationApplication.ViewModels.StateOfExport;
    using Prsd.Core.Mapper;
    using Requests.StateOfExport;
    using ViewModels.Shared;

    public class StateOfExportWithTransportRouteDataMap : IMap<StateOfExportWithTransportRouteData, StateOfExportViewModel>
    {
        public StateOfExportViewModel Map(StateOfExportWithTransportRouteData source)
        {
            var model = new StateOfExportViewModel
            {
                ShowNextSection = source.StateOfExport != null,
                TransitStateCountryIds = source.TransitStates.Select(ts => ts.Country.Id).ToArray()
            };

            if (model.ShowNextSection)
            {
                model.CountryId = source.StateOfExport.Country.Id;

                model.Countries = new SelectList(source.Countries, "Id", "Name", model.CountryId);

                model.EntryOrExitPointId = source.StateOfExport.ExitPoint.Id;

                model.CompetentAuthorities = new StringGuidRadioButtons(source.CompetentAuthorities
                    .Select(ca => new KeyValuePair<string, Guid>(ca.Name, ca.Id)));

                model.ExitPoints = new SelectList(source.ExitPoints, "Id", "Name");

                model.CompetentAuthorities.SelectedValue = source.StateOfExport.CompetentAuthority.Id;
            }

            model.Countries = new SelectList(source.Countries, "Id", "Name", source.Countries.Single(c => c.Name == "United Kingdom").Id);

            if (source.StateOfImport != null)
            {
                model.StateOfImportCountryId = source.StateOfImport.Country.Id;
            }

            return model;
        }
    }
}