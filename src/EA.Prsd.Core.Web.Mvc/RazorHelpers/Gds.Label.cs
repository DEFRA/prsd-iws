namespace EA.Prsd.Core.Web.Mvc.RazorHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Validation;

    public partial class Gds<TModel>
    {
        public MvcHtmlString LabelFor<TValue>(Expression<Func<TModel, TValue>> expression, string optionalMessage = "")
        {
            return LabelFor(expression, new RouteValueDictionary(), optionalMessage);
        }

        public MvcHtmlString LabelFor<TValue>(Expression<Func<TModel, TValue>> expression, object htmlAttributes, string optionalMessage = "")
        {
            return LabelFor(expression, new RouteValueDictionary(htmlAttributes), optionalMessage);
        }

        public MvcHtmlString LabelFor<TValue>(Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes, string optionalMessage = "")
        {
            return LabelFor(expression, htmlAttributes, "form-label", optionalMessage);
        }

        private MvcHtmlString LabelFor<TValue>(Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes, string cssClass, string optionalMessage = "")
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(expression, HtmlHelper.ViewData);

            var containsRequiredIfAttribute = ContainsAttribute<RequiredIfAttribute>(modelMetadata, modelMetadata.PropertyName);

            var appendOptional = modelMetadata.IsRequired || containsRequiredIfAttribute ? string.Empty : "(optional)";

            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            var labelText = modelMetadata.DisplayName
                            ?? modelMetadata.PropertyName
                            ?? htmlFieldName.Split('.').Last();

            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            var labelTag = new TagBuilder("label");

            labelTag.MergeAttributes(htmlAttributes);

            labelTag.Attributes.Add("for",
                HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

            labelTag.InnerHtml = labelText + " " + (string.IsNullOrEmpty(optionalMessage) ? appendOptional : optionalMessage);

            var nameToCheck = GetPropertyName(HtmlHelper, expression);
            var modelState = HtmlHelper.ViewData.ModelState[nameToCheck];

            if (modelState != null && modelState.Errors.Any())
            {
                cssClass += "-bold";
            }

            labelTag.AddCssClass(cssClass);

            return MvcHtmlString.Create(labelTag.ToString(TagRenderMode.Normal));
        }

        private bool ContainsAttribute<T>(ModelMetadata modelMetadata, string propertyName)
        {
            var property = modelMetadata.ContainerType.GetProperties().Single(p => p.Name.Equals(propertyName));

            var attribute = Attribute.GetCustomAttribute(property, typeof(T));
            return attribute != null;
        }
    }
}