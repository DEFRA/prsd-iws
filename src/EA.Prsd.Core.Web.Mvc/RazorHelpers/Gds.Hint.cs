namespace EA.Prsd.Core.Web.Mvc.RazorHelpers
{
    using System.Web.Mvc;

    public partial class Gds<TModel>
    {
        public MvcHtmlString Hint(string hintText)
        {
            var hint = CreateHintTag(hintText);
            return MvcHtmlString.Create(hint.ToString());
        }

        public MvcHtmlString Hint(string hintText, string id)
        {
            var hint = CreateHintTag(hintText);
            hint.GenerateId(id);
            return MvcHtmlString.Create(hint.ToString());
        }

        private static TagBuilder CreateHintTag(string hintText)
        {
            var hint = new TagBuilder("span");
            hint.AddCssClass("form-hint");
            hint.SetInnerText(hintText);
            return hint;
        }
    }
}