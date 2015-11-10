namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.StateOfExport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Core.Shared;
    using Core.TransportRoute;
    using Web.ViewModels.Shared;

    public class StateOfExportViewModel
    {
        [Display(Name = "CountryId", ResourceType = typeof(StateOfExportViewModelResources))]
        public Guid? CountryId { get; set; }

        [Display(Name = "CompetentAuthorityId", ResourceType = typeof(StateOfExportViewModelResources))]
        public Guid? CompetentAuthorityId { get; set; }

        [Display(Name = "ExitPointId", ResourceType = typeof(StateOfExportViewModelResources))]
        public Guid? ExitPointId { get; set; }

        public IList<CountryData> Countries { get; set; }

        public bool IsCountrySelected
        {
            get { return CountryId.HasValue; }
        }

        public SelectList CountryList
        {
            get
            {
                return new SelectList(Countries, "Id", "Name");
            }
        }

        public IList<CompetentAuthorityData> CompetentAuthorities { get; set; }

        public StringGuidRadioButtons CompetentAuthorityList
        {
            get
            {
                return new StringGuidRadioButtons(CompetentAuthorities.Select(
                    ca => new KeyValuePair<string, Guid>(string.Format("{0} - {1}", ca.Code, ca.Name), ca.Id)));
            }
        }

        public IList<EntryOrExitPointData> ExitPoints { get; set; }

        public SelectList ExitPointList
        {
            get
            {
                return new SelectList(ExitPoints.OrderBy(eep => eep.Name), "Id", "Name");
            }
        }

        public StateOfExportViewModel()
        {
        }

        public StateOfExportViewModel(StateOfExport stateOfExport)
        {
            CountryId = stateOfExport.CountryId;
            CompetentAuthorityId = stateOfExport.CompetentAuthorityId;
            ExitPointId = stateOfExport.ExitPointId;
        }
    }
}