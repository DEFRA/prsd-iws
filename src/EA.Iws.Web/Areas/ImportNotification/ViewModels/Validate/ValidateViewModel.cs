namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Validate
{
    using System;
    using System.Collections.Generic;
    using Requests.ImportNotification.Validate;

    public class ValidateViewModel
    {
        public ValidateViewModel()
        {
        }

        public ValidateViewModel(IEnumerable<ValidationResults> results)
        {
            ValidationResults = results;
        }

        public IEnumerable<ValidationResults> ValidationResults { get; set; }
    }
}