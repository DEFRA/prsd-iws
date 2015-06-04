namespace EA.Prsd.Core.Web.Mvc.RazorHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Web.Mvc;

    public partial class Gds<TModel>
    {
        protected readonly HtmlHelper<TModel> HtmlHelper;

        public Gds(HtmlHelper<TModel> htmlHelper)
        {
            HtmlHelper = htmlHelper;
        }

        protected static void AddFormControlCssClass(IDictionary<string, object> htmlAttributes)
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
        }

        protected static string GetPropertyName<TProperty>(HtmlHelper htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            string nameToCheck;

            if (memberExpression == null)
            {
                return string.Empty;
            }

            // We are accessing a sub property, the model state records the fully qualified name.
            // For the expression m.Class.Property:
            // Class.Property rather than Property
            if (memberExpression.ToString().Count(me => me == '.') == 2)
            {
                var memberExpressionAsString = memberExpression.ToString();
                nameToCheck = memberExpressionAsString.Substring(memberExpressionAsString.IndexOf('.') + 1);
            }
            else
            {
                var property = memberExpression.Member as PropertyInfo;

                if (property == null)
                {
                    return string.Empty;
                }

                nameToCheck = property.Name;
            }

            var propertyName = htmlHelper.ViewData.ModelMetadata.PropertyName;
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                nameToCheck = string.Format("{0}.{1}", propertyName, nameToCheck);
            }

            return nameToCheck;
        }
    }
}