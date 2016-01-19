namespace EA.Iws.Web.Infrastructure
{
    using System.Web.Mvc;
    using Prsd.Core;

    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Displays todays date in the format 'dddd dd MMMM' including day suffix.
        /// 
        /// For example, 'Tuesday 19th January'.
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString DisplayTodaysDate(this HtmlHelper helper)
        {
            var today = SystemTime.Now.Date;
            string suffix;
            switch (today.Day)
            {
                case 1:
                case 21:
                case 31:
                    suffix = "st";
                    break;
                case 2:
                case 22:
                    suffix = "nd";
                    break;
                case 3:
                case 23:
                    suffix = "rd";
                    break;
                default:
                    suffix = "th";
                    break;
            }

            return new MvcHtmlString(string.Format("{0:dddd dd}{1} {0:MMMM}", today, suffix));
        }
    }
}