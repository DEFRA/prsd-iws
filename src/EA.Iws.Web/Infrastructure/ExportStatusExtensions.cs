namespace EA.Iws.Web.Infrastructure
{
    using System.Web.Mvc;

    public static class ExportStatusExtensions
    {
        public static ExportStatus ExportStatus(this HtmlHelper htmlHelper)
        {
            return new ExportStatus(htmlHelper);
        }
    }
}