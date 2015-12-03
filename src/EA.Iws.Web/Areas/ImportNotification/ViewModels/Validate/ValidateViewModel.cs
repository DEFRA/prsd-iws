namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Validate
{
    using System.Collections.Generic;

    public class ValidateViewModel
    {
        public ValidateViewModel()
        {
        }

        public ValidateViewModel(IEnumerable<string> results)
        {
            Errors = results;
        }

        public IEnumerable<string> Errors { get; set; }
    }
}