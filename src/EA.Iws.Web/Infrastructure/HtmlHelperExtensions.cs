namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Core.Notification;
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

            return new MvcHtmlString(string.Format("{0:dddd d}{1} {0:MMMM}", today, suffix));
        }

        /// <summary>
        /// Returns a link to the feedback page
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString FeedbackLink(this HtmlHelper helper)
        {
            return helper.ActionLink(FeedbackLinkResources.LinkText, "Index", "Feedback",
                new { area = string.Empty }, null);
        }

        /// <summary>
        /// Returns the helpline number for the specified CA
        /// </summary>
        public static MvcHtmlString CompetentAuthorityHelpline(this HtmlHelper helper, UKCompetentAuthority competentAuthority)
        {
            return UKCompetentAuthorityDetails.ForCompetentAuthority(competentAuthority).HelplineNumber;
        }

        /// <summary>
        /// Returns a mailto link to askshipments@environment-agency.gov.uk
        /// </summary>
        public static MvcHtmlString IwsContactEmail(this HtmlHelper helper)
        {
            return helper.MailtoLink("askshipments@environment-agency.gov.uk");
        }

        /// <summary>
        /// Returns a mailto link to the specified email address
        /// </summary>
        public static MvcHtmlString MailtoLink(this HtmlHelper helper, string emailAddress)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", "mailto:" + emailAddress);
            builder.SetInnerText(emailAddress);

            return MvcHtmlString.Create(builder.ToString());
        }

        /// <summary>
        /// Returns HTML markup with whitespace replaced with non-breaking space for each property 
        /// in the object that is represented by the 
        /// <see cref="T:System.Linq.Expressions.Expression"/> expression.
        /// </summary>
        /// 
        /// <returns>
        /// The HTML markup with whitespace replaced with non-breaking space for each property in the 
        /// object that is represented by the expression.
        /// </returns>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the properties to display.</param>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        public static MvcHtmlString DisplayNonBreakingFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression)
        {
            var display = helper.DisplayFor(expression);
            return new MvcHtmlString(display.ToString().ToNonBreakingString());
        }
    }
}