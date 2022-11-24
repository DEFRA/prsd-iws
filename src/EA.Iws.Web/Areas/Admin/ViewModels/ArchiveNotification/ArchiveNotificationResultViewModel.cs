namespace EA.Iws.Web.Areas.Admin.ViewModels.ArchiveNotification
{
    using EA.Iws.Core.Admin.ArchiveNotification;    

    public class ArchiveNotificationResultViewModel
    {
        public ArchiveNotificationResult[] ArchiveNotificationResults { get; set; }

        public bool HasAnyResults
        {
            get 
            {
                return false;
                //return ArchiveNotificationResults.Any(); 
            }
        }
    }
}