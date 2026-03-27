namespace EA.Iws.Web.Areas.Reports.ViewModels.EADataReports
{
    using EA.Iws.Core.Reports;
    using EA.Iws.Web.ViewModels.Shared;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class IndexViewModel : IValidatableObject
    {
        public RequiredDateInputViewModel From { get; set; }

        public RequiredDateInputViewModel To { get; set; }

        public IList<KeyValuePairViewModel<EAReportList, bool>> EAReportLists { get; set; }

        public IList<EAReportList> SelectedValues
        {
            get
            {
                if (EAReportLists == null)
                {
                    return new List<EAReportList>(0);
                }

                return EAReportLists.Where(c => c.Value).Select(c => c.Key).ToList();
            }
        }

        public IndexViewModel()
        {
            From = new RequiredDateInputViewModel();
            To = new RequiredDateInputViewModel();

            EAReportLists = new List<KeyValuePairViewModel<EAReportList, bool>>
            {
                new KeyValuePairViewModel<EAReportList, bool>(EAReportList.ShipmentReport, false),
                new KeyValuePairViewModel<EAReportList, bool>(EAReportList.FinanceReport, false),
                new KeyValuePairViewModel<EAReportList, bool>(EAReportList.ProducerReport, false),
                new KeyValuePairViewModel<EAReportList, bool>(EAReportList.FOIReport, false)
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (From.AsDateTime() > To.AsDateTime())
            {
                yield return new ValidationResult(IndexViewModelResources.FromDateBeforeToDate, new[] { "From" });
            }

            if (EAReportLists != null && !EAReportLists.Any(c => c.Value))
            {
                yield return new ValidationResult(IndexViewModelResources.SelectionRequired, new[] { "EAReportLists" });
            }
        }
    }
}