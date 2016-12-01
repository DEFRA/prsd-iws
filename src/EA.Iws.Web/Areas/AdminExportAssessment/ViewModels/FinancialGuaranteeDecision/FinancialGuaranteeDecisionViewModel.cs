namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.FinancialGuaranteeDecision
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.FinancialGuarantee;
    using Prsd.Core.Helpers;

    public class FinancialGuaranteeDecisionViewModel
    {
        public Guid FinancialGuaranteeId { get; set; }

        [Required(ErrorMessage = "Please select a decision")]
        public FinancialGuaranteeDecision? Decision { get; set; }

        public SelectList PossibleDecisions { get; set; }

        public FinancialGuaranteeDecisionViewModel()
        {
            var values = Enum.GetValues(typeof(FinancialGuaranteeDecision))
                .Cast<FinancialGuaranteeDecision>()
                .Where(d => d != FinancialGuaranteeDecision.None)
                .Select(e => new KeyValuePair<int, string>(
                    (int)e,
                    EnumHelper.GetShortName(e)))
                .OrderBy(k => k.Value);

            PossibleDecisions = new SelectList(values, "Key", "Value");
        }
    }
}