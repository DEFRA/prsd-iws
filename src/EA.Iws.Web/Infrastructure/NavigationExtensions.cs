namespace EA.Iws.Web.Infrastructure
{
    using System.Web.Mvc;

    public static class NavigationExtensions
    {
        public static Navigation Navigation(this HtmlHelper htmlHelper)
        {
            return new Navigation(htmlHelper);
        }
    }
}