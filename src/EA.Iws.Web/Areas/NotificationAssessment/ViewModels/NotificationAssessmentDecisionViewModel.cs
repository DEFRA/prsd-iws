namespace EA.Iws.Web.Areas.NotificationAssessment.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.NotificationAssessment;
    using Web.ViewModels.Shared;

    public class NotificationAssessmentDecisionViewModel
    {
        public Guid NotificationId { get; set; }

        public NotificationStatus Status { get; set; }

        public IList<DecisionRecordViewModel> PreviousDecisions { get; set; }

        public SelectList PossibleDecisions
        {
            get
            {
                return new SelectList(DecisionTypes);
            }
        }

        public IList<DecisionType> DecisionTypes { get; set; }

        [Required]
        [Display(Name = "Decision")]
        public DecisionType? SelectedDecision { get; set; }
        
        public OptionalDateInputViewModel ConsentValidFromDate { get; set; }

        public OptionalDateInputViewModel ConsentValidToDate { get; set; }

        public string ConsentConditions { get; set; }

        public string ReasonForObjection { get; set; }

        public string ReasonWithdrawn { get; set; }

        public NotificationAssessmentDecisionViewModel()
        {
            ConsentValidFromDate = new OptionalDateInputViewModel();
            ConsentValidToDate = new OptionalDateInputViewModel();
            PreviousDecisions = new List<DecisionRecordViewModel>();
            DecisionTypes = new List<DecisionType>();
        }
    }
}