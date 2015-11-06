namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.StateOfImport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Core.Shared;
    using Core.TransportRoute;

    public class StateOfImportViewModel
    {
        [Display(Name = "CompetentAuthorityId", ResourceType = typeof(StateOfImportViewModelResources))]
        public Guid? CompetentAuthorityId { get; set; }

        [Display(Name = "EntryPointId", ResourceType = typeof(StateOfImportViewModelResources))]
        public Guid? EntryPointId { get; set; }

        public IList<CompetentAuthorityData> CompetentAuthorities { get; set; }

        public IList<EntryOrExitPointData> EntryPoints { get; set; }

        public SelectList EntryPointList
        {
            get
            {
                return new SelectList(EntryPoints.OrderBy(ep => ep.Name), "Id", "Name");
            }
        }

        public StateOfImportViewModel()
        {
        }

        public StateOfImportViewModel(StateOfImport stateOfImport)
        {
            CompetentAuthorityId = stateOfImport.CompetentAuthorityId;
            EntryPointId = stateOfImport.EntryPointId;
        }
    }
}