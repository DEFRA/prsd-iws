namespace EA.Iws.Web.Areas.Admin.ViewModels.UserAdministration
{
    using Core.Admin;

    public class ChangeUserStatusViewModel
    {
        public string UserId { get; set; }

        public InternalUserStatus Status { get; set; }
    }
}