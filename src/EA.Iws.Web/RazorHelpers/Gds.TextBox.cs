namespace EA.Iws.Web.RazorHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;

    public partial class Gds<TModel>
    {
        public MvcHtmlString TextBoxFor<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            return TextBoxFor(expression, new RouteValueDictionary());
        }

        public MvcHtmlString TextBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            var routeValueDictionary = System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return TextBoxFor(expression, routeValueDictionary);
        }

        public MvcHtmlString TextBoxFor<TValue>(Expression<Func<TModel, TValue>> expression,
            IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes.ContainsKey("class"))
            {
                if (!htmlAttributes["class"].ToString().Contains("form-control"))
                {
                    htmlAttributes["class"] += " form-control";
                }
            }
            else
            {
                htmlAttributes.Add("class", "form-control");
            }
            return HtmlHelper.TextBoxFor(expression, htmlAttributes);
        }
    }
}