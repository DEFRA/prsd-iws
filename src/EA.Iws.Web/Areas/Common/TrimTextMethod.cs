namespace EA.Iws.Web.Areas.Common
{    
    public class TrimTextMethod : ITrimTextMethod
    {
        public string RemoveTextWhiteSpaces(string requestText) 
        {
            var responseText = string.Empty;
            if (requestText != null)
            {
                responseText = requestText.Trim();
            }
            return responseText;
        }
    }
}