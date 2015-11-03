namespace EA.Iws.Web.Areas.AddressBook
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

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
            context.MapLowercaseDashedRoute(
                name: "AddressBook_default",
                url: "Address-Book/{controller}/{action}/{id}",
                defaults: new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                namespaces: new[] { typeof(HomeController).Namespace });
        }
    }
}