namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels
{
    using System;
    using Core.Admin;

    public class DecisionRecordViewModel
    {
        public DecisionType Decision { get; set; }

        public DateTime Date { get; set; }
    }
}