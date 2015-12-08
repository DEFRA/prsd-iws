namespace EA.Iws.Requests.ImportNotification.Validate
{
    using System.Collections.Generic;

    public class ValidationResults
    {
        public string EntityName { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}