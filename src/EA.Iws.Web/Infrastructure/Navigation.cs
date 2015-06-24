namespace EA.Iws.Web.Infrastructure
{
    using System.Linq;
    using System.Web.Mvc;

    public class Navigation
    {
        private readonly HtmlHelper htmlHelper;

        public Navigation(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        /// <summary>
        /// Returns a navigation link
        /// </summary>
        /// <param name="linkText">The text to display in the link</param>
        /// <param name="isComplete">Whether the section is complete. When true, a tick is shown next to the link.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="areaName">The name of the area.</param>
        /// <param name="additionalActiveActionNames">Additional action names that are considered to be the active page for the action.</param>
        /// <returns></returns>
        public MvcHtmlString Link(string linkText, bool isComplete, string actionName, string controllerName, string areaName, string[] additionalActiveActionNames = null)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            var link = new TagBuilder("a");
            link.GenerateId(linkText);
            link.Attributes.Add("href", urlHelper.Action(actionName, controllerName, new { area = areaName }));

            if (IsActiveItem(htmlHelper, controllerName, actionName, areaName, additionalActiveActionNames))
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

        private static bool IsActiveItem(HtmlHelper htmlHelper, string controller, string action, string area, string[] additionalActiveActionNames = null)
        {
            if (!TokenMatches(htmlHelper, area, "area"))
            {
                return false;
            }

            if (!ValueMatches(htmlHelper, controller, "controller"))
            {
                return false;
            }

            if (ValueMatches(htmlHelper, action, "action"))
            {
                return true;
            }

            return additionalActiveActionNames != null && additionalActiveActionNames.Any(additionalAction => ValueMatches(htmlHelper, additionalAction, "action"));
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