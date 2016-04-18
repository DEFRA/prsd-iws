namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Validate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public bool HasErrors
        {
            get { return ValidationResults.Any(p => p.Errors.Any()); }
        }
    }
}