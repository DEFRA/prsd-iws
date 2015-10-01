namespace EA.Iws.Web.Areas.NotificationAssessment.ViewModels
{
    using System;
    using Core.Admin;

    public class DecisionRecordViewModel
    {
        public DecisionType Decision { get; set; }

        public DateTime Date { get; set; }
    }
}