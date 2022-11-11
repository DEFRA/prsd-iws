namespace EA.Iws.Web.RazorHelpers
{
    using System.Web.Mvc;

    public static class IwsGdsExtensions
    {
        public static IwsGds<TModel> IwsGds<TModel>(this WebViewPage<TModel> webViewPage)
        {
            return new IwsGds<TModel>(webViewPage);
        }
    }
}