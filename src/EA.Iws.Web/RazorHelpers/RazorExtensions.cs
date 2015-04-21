namespace EA.Iws.Web.RazorHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class LabelExtensions
    {
        public static MvcHtmlString LabelForWithOptional<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression)
        {
            return LabelForWithOptional(htmlHelper: htmlHelper, expression: expression, htmlAttributes: new RouteValueDictionary());
        }

        public static MvcHtmlString LabelForWithOptional<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression,
            object htmlAttributes)
        {
            return LabelForWithOptional(htmlHelper: htmlHelper, expression: expression, htmlAttributes: new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString LabelForWithOptional<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression,
            IDictionary<string, object> htmlAttributes)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            string appendOptional = modelMetadata.IsRequired ? string.Empty : " (optional)";

            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            string labelText = modelMetadata.DisplayName 
                ?? modelMetadata.PropertyName 
                ?? htmlFieldName.Split('.').Last();

            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            TagBuilder labelTag = new TagBuilder("label");

            labelTag.MergeAttributes(htmlAttributes);
            
            labelTag.Attributes.Add("for", htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

            labelTag.InnerHtml = labelText + appendOptional;

            return MvcHtmlString.Create(labelTag.ToString(TagRenderMode.Normal));
        }

        public static Gds<TModel> Gds<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new Gds<TModel>(htmlHelper);
        }
    }
}