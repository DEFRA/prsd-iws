﻿namespace EA.Iws.Web.RazorHelpers
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Prsd.Core.Web.Mvc.RazorHelpers;

    public partial class IwsGds<TModel>
    {
        private const string CssTextClass = "govuk-input";
        private const string CssWeeeIconClass = "weee-icon";
        private const string CssFontAwesomeClass = "fa";
        private const string CssFontAwesomeSizeClass = "fa-2x";

        public MvcHtmlString TextBoxWithIconFor<TValue>(Expression<Func<TModel, TValue>> expression,
            object htmlAttributes, string icon)
        {
            var routeValueDictionary = System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            var divTagBuilder = new TagBuilder("div");
            var iconTagBuilder = new TagBuilder("i");
            iconTagBuilder.AddCssClass(CssWeeeIconClass);
            iconTagBuilder.AddCssClass(CssFontAwesomeSizeClass);
            iconTagBuilder.AddCssClass(icon);
            iconTagBuilder.AddCssClass(CssFontAwesomeClass);

            divTagBuilder.InnerHtml += iconTagBuilder.ToString();

            GdsExtensions.AddClass(routeValueDictionary, CssTextClass);

            var textBox = HtmlHelper.TextBoxFor(expression, routeValueDictionary);
            divTagBuilder.InnerHtml += textBox.ToHtmlString();
            return new MvcHtmlString(divTagBuilder.InnerHtml);
        }
    }
}