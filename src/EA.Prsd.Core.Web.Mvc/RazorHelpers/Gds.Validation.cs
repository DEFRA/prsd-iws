namespace EA.Prsd.Core.Web.Mvc.RazorHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;

    public partial class Gds<TModel>
    {
        /// <summary>
        ///     This extension will provide a GDS compliant validation summary in the waste carriers style.
        /// </summary>
        /// <returns>A div containing the list of validation errors if applicable.</returns>
        public MvcHtmlString ValidationSummary()
        {
            var modelStateDictionary = HtmlHelper.ViewData.ModelState;

            var modelErrors = GetErrorsForModel(modelStateDictionary);

            if (modelStateDictionary == null || modelStateDictionary.Count == 0 || !modelErrors.Any())
            {
                return new MvcHtmlString(GetJavascriptEnabledBlankSummary());
            }

            // If the ModelState has errors it has been through an HTML post and javascript is disabled.
            return new MvcHtmlString(GetJavascriptDisabledErrorSummary(modelErrors));
        }

        /// <summary>
        ///     This extension will highlight any validation error input boxes.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public string FormGroupClass<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            var nameToCheck = GetPropertyName(HtmlHelper, expression);

            if (string.IsNullOrWhiteSpace(nameToCheck))
            {
                return string.Empty;
            }

            var modelState = HtmlHelper.ViewData.ModelState[nameToCheck];

            if (modelState == null)
            {
                return string.Empty;
            }

            var cssClass = (modelState.Errors.Count > 0) ? "error" : string.Empty;

            return cssClass;
        }

        public MvcHtmlString ValidationMessageFor<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            return HtmlHelper.ValidationMessageFor(expression, validationMessage: null, htmlAttributes: new { @class = "error-message" }, tag: "span");
        }

        public MvcHtmlString ValidationMessageFor<TValue>(Expression<Func<TModel, TValue>> expression, string validationMessage)
        {
            return HtmlHelper.ValidationMessageFor(expression, validationMessage, htmlAttributes: new { @class = "error-message" }, tag: "span");
        }

        private string GetJavascriptEnabledBlankSummary()
        {
            return @"<div class='error-summary-valid' data-valmsg-summary='true'>
                        <ul class='error-summary-list'>
                            <li style='display:none'></li>
                        </ul>
                    </div>";
        }

        private string GetJavascriptDisabledErrorSummary(IEnumerable<ModelErrorWithFieldId> modelErrors)
        {
            var startErrorRegion = @"<div class='error-summary' id='error_explanation'>";

            var errorTitle = GetErrorSummaryHeading(modelErrors);

            var errorList = GetErrorSummaryList(modelErrors);

            var endErrorRegion = "</div>";

            var summaryBuilder = new StringBuilder();

            summaryBuilder.Append(startErrorRegion)
                .Append(errorTitle)
                .Append(errorList)
                .Append(endErrorRegion);

            return summaryBuilder.ToString();
        }

        private string GetErrorSummaryList(IEnumerable<ModelErrorWithFieldId> modelErrors)
        {
            var errorListBuilder = new StringBuilder();

            errorListBuilder.AppendLine("<ul class='error-summary-list'>");

            foreach (var modelError in modelErrors)
            {
                errorListBuilder.AppendLine("<li>");
                errorListBuilder.AppendFormat("<a href=\"#{0}\">{1}</a>", modelError.FieldId,
                    modelError.ModelError.ErrorMessage);
                errorListBuilder.AppendLine("</li>");
            }

            errorListBuilder.AppendLine("</ul>");

            return errorListBuilder.ToString();
        }

        private string GetErrorSummaryHeading(IEnumerable<ModelErrorWithFieldId> modelErrors)
        {
            var modelErrorsCount = modelErrors.Count();

            var tagBuilder = new TagBuilder("h2");

            tagBuilder.AddCssClass("heading-medium");
            tagBuilder.AddCssClass("error-summary-heading");

            var modelErrorsCountString = string.Empty;
            if (modelErrorsCount > 1)
            {
                modelErrorsCountString = modelErrorsCount + " errors";
            }
            else
            {
                modelErrorsCountString = "1 error";
            }

            tagBuilder.SetInnerText(string.Format("You have {0} on this page", modelErrorsCountString));

            return tagBuilder.ToString();
        }

        private IEnumerable<ModelErrorWithFieldId> GetErrorsForModel(ModelStateDictionary modelStateDictionary)
        {
            // TODO: Our current method of finding the field ID is unlikely to work for arrays or complex types.
            foreach (var key in modelStateDictionary.Keys)
            {
                // Required for closure.
                var keyName = key;

                foreach (var error in modelStateDictionary[key].Errors)
                {
                    yield return new ModelErrorWithFieldId(error, key);
                }
            }
        }

        private class ModelErrorWithFieldId
        {
            public ModelErrorWithFieldId(ModelError modelError, string fieldId)
            {
                ModelError = modelError;
                FieldId = fieldId;
            }

            public ModelError ModelError { get; private set; }

            public string FieldId { get; private set; }
        }
    }
}