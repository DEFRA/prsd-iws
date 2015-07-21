namespace EA.Iws.Web.Areas.Admin.ViewModels.UserAdministration
{
    using Core.Admin;

    public class UserApprovalViewModel
    {
        public InternalUser User { get; set; }

        public ApprovalAction? Action { get; set; }

        public UserApprovalViewModel()
        {
        }

        public UserApprovalViewModel(InternalUser user)
        {
            User = user;
        }
    }
}