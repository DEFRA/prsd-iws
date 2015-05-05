namespace EA.Iws.Web.RazorHelpers
{
    using System.Web.Mvc;

    public partial class Gds<TModel>
    {
        protected readonly HtmlHelper<TModel> HtmlHelper;

        public Gds(HtmlHelper<TModel> htmlHelper)
        {
            HtmlHelper = htmlHelper;
        }
    }
}