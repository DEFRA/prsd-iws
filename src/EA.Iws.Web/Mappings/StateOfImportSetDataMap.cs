namespace EA.Iws.Web.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Areas.NotificationApplication.ViewModels.StateOfImport;
    using Prsd.Core.Mapper;
    using Requests.StateOfImport;
    using ViewModels.Shared;

    public class StateOfImportSetDataMap : IMap<StateOfImportSetData, StateOfImportViewModel>
    {
        public StateOfImportViewModel Map(StateOfImportSetData source)
        {
            var model = new StateOfImportViewModel
            {
                ShowNextSection = source.StateOfImport != null,
                TransitStateCountryIds = source.TransitStates.Select(ts => ts.Country.Id).ToArray()
            };

            if (model.ShowNextSection)
            {
                model.CountryId = source.StateOfImport.Country.Id;

                model.Countries = new SelectList(source.Countries, "Id", "Name", model.CountryId);

                model.EntryOrExitPointId = source.StateOfImport.EntryPoint.Id;

                model.CompetentAuthorities = new StringGuidRadioButtons(source.CompetentAuthorities
                    .Select(ca => new KeyValuePair<string, Guid>(ca.Name, ca.Id)));

                model.CompetentAuthorities.SelectedValue = source.StateOfImport.CompetentAuthority.Id;

                model.EntryPoints = new SelectList(source.EntryPoints, "Id", "Name", model.EntryOrExitPointId);
            }
            else
            {
                model.Countries = new SelectList(source.Countries, "Id", "Name");
            }

            if (source.StateOfExport != null)
            {
                model.StateOfExportCountryId = source.StateOfExport.Country.Id;
            }

            return model;
        }
    }
}