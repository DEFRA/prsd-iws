namespace EA.Iws.Web.Mappings
{
    using System.Linq;
    using System.Web.Mvc;
    using Areas.NotificationApplication.ViewModels.StateOfExport;
    using Core.StateOfExport;
    using Prsd.Core.Mapper;

    public class StateOfExportWithTransportRouteDataMap : IMap<StateOfExportWithTransportRouteData, StateOfExportViewModel>
    {
        public StateOfExportViewModel Map(StateOfExportWithTransportRouteData source)
        {
            var model = new StateOfExportViewModel
            {
                TransitStateCountryIds = source.TransitStates.Select(ts => ts.Country.Id).ToArray()
            };

            model.CountryId = source.StateOfExport.Country.Id;
            model.CompetentAuthorityName = source.StateOfExport.CompetentAuthority.Code + " - " + source.StateOfExport.CompetentAuthority.Name;

            if (source.StateOfExport.ExitPoint != null)
            {
                model.EntryOrExitPointId = source.StateOfExport.ExitPoint.Id;
            }
            
            model.ExitPoints = new SelectList(source.ExitPoints, "Id", "Name");

            if (source.StateOfImport != null)
            {
                model.StateOfImportCountryId = source.StateOfImport.Country.Id;
            }

            return model;
        }
    }
}