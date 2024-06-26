﻿namespace EA.Iws.Web.RazorHelpers
{
    using Prsd.Core.Web.Mvc.RazorHelpers;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;

    public partial class IwsGds<TModel>
    {
        public readonly HtmlHelper<TModel> HtmlHelper;
        public readonly UrlHelper UrlHelper;

        private readonly Gds<TModel> gdsHelper;
        public IwsGds(WebViewPage<TModel> webViewPage)
        {
            HtmlHelper = webViewPage.Html;
            UrlHelper = webViewPage.Url;

            gdsHelper = new Gds<TModel>(webViewPage.Html);
        }

        public ProgressiveDisclosure<TModel> ProgressiveDisclosure(string linkText)
        {
            return new ProgressiveDisclosure<TModel>(this, linkText);
        }

        public TableBuilder<TModel, TDataset> TableBuilder<TDataset>(IEnumerable<TDataset> dataset, string caption, object htmlAttributes = null)
        {
            return new TableBuilder<TModel, TDataset>(this, dataset, caption, htmlAttributes);
        }

        public string EventTrackingFunction(string eventCategory, string eventAction, string eventLabel = null)
        {
            string result;

            if (string.IsNullOrEmpty(eventLabel))
            {
                result = string.Format("gtag('send', 'event', '{0}', '{1}');", HttpUtility.JavaScriptStringEncode(eventCategory), HttpUtility.JavaScriptStringEncode(eventAction));
            }
            else
            {
                result = string.Format("gtag('send', 'event', '{0}', '{1}', '{2}');", HttpUtility.JavaScriptStringEncode(eventCategory), HttpUtility.JavaScriptStringEncode(eventAction), HttpUtility.JavaScriptStringEncode(eventLabel));
            }

            return result;
        }
    }
}