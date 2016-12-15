namespace EA.Iws.Web.Areas.Admin.ViewModels.UserAdministration
{
    using Core.Authorization;

    public class ChangeUserRoleViewModel
    {
        public string UserId { get; set; }

        public UserRole Role { get; set; }
    }
}