namespace EA.Iws.Web.Areas.Admin.ViewModels.UserAdministration
{
    using Core.Admin;

    public class UserApprovalViewModel
    {
        public InternalUserData UserData { get; set; }

        public ApprovalAction? Action { get; set; }

        public UserApprovalViewModel()
        {
        }

        public UserApprovalViewModel(InternalUserData userData)
        {
            UserData = userData;
        }
    }
}