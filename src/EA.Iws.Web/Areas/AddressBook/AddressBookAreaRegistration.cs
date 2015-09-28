namespace EA.Iws.Web.Areas.AddressBook
{
    using System.Web.Mvc;

    public class AddressBookAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AddressBook";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AddressBook_default",
                "Address-Book/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional });
        }
    }
}