namespace EA.Prsd.Core.Web.Mvc.RazorHelpers
{
    using System.Web.Mvc;

    public partial class Gds<TModel>
    {
        public MvcHtmlString HintSpan(string hintText, string id = null, string displayStyle = "")
        {
            var hint = CreateHintTag(hintText, id, "span", displayStyle);
            return MvcHtmlString.Create(hint.ToString());
        }

        public MvcHtmlString HintParagraph(string hintText, string id = null)
        {
            var hint = CreateHintTag(hintText, id, "p");
            return MvcHtmlString.Create(hint.ToString());
        }

        private static TagBuilder CreateHintTag(string hintText, string id, string tagName, string displayStyle = "")
        {
            var hint = new TagBuilder(tagName);
            hint.AddCssClass("form-hint");
            if (!string.IsNullOrWhiteSpace(displayStyle))
            {
                hint.MergeAttribute("style", "display:" + displayStyle + ";");
            }
            hint.SetInnerText(hintText);
            if (!string.IsNullOrWhiteSpace(id))
            {
                hint.GenerateId(id);
            }
            return hint;
        }
    }
}