namespace EA.Iws.Web.Infrastructure
{
    using System.Web.Mvc;

    public class Navigation
    {
        private readonly HtmlHelper htmlHelper;

        public Navigation(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        public MvcHtmlString Link(string linkText, bool isComplete, string actionName, string controllerName, string areaName)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            var link = new TagBuilder("a");
            link.GenerateId(linkText);
            link.Attributes.Add("href", urlHelper.Action(actionName, controllerName, new { area = areaName }));
            
            if (IsActiveItem(htmlHelper, controllerName, actionName, areaName))
            {
                link.AddCssClass("active");
            }

            link.Attributes.Add("title", linkText);
            link.Attributes.Add("role", "progressbar");
            link.Attributes.Add("aria-valuenow", "100");
            link.Attributes.Add("aria-valuemin", "0");
            link.Attributes.Add("aria-valuemax", "100");

            var icon = new TagBuilder("i");
            icon.AddCssClass("fa");

            if (isComplete)
            {
                icon.AddCssClass("fa-check");
            }

            link.InnerHtml = linkText + icon.ToString();

            return MvcHtmlString.Create(link.ToString());
        }

        private static bool IsActiveItem(HtmlHelper htmlHelper, string controller, string action, string area)
        {
            if (!TokenMatches(htmlHelper, area, "area"))
            {
                return false;
            }

            if (!ValueMatches(htmlHelper, controller, "controller"))
            {
                return false;
            }

            return ValueMatches(htmlHelper, action, "action");
        }

        private static bool ValueMatches(HtmlHelper htmlHelper, string item, string dataToken)
        {
            var routeData = (string)htmlHelper.ViewContext.ParentActionViewContext.RouteData.Values[dataToken];

            if (routeData == null)
            {
                return string.IsNullOrEmpty(item);
            }

            return routeData == item;
        }

        private static bool TokenMatches(HtmlHelper htmlHelper, string item, string dataToken)
        {
            var routeData = (string)htmlHelper.ViewContext.ParentActionViewContext.RouteData.DataTokens[dataToken];

            if (dataToken == "action" && item == "Index" && string.IsNullOrEmpty(routeData))
            {
                return true;
            }

            if (dataToken == "controller" && item == "Home" && string.IsNullOrEmpty(routeData))
            {
                return true;
            }

            if (routeData == null)
            {
                return string.IsNullOrEmpty(item);
            }

            return routeData == item;
        }
    }
}