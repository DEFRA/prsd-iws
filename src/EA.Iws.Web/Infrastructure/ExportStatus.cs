namespace EA.Iws.Web.Infrastructure
{
    using System.Web.Mvc;
    using Core.NotificationAssessment;

    public class ExportStatus
    {
        private readonly HtmlHelper htmlHelper;

        public ExportStatus(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        public MvcHtmlString DisplayColorClass(NotificationStatus status)
        {
            string statusColor;
            switch (status)
            {
                case NotificationStatus.InAssessment:
                case NotificationStatus.ReadyToTransmit:
                case NotificationStatus.Transmitted:
                case NotificationStatus.DecisionRequiredBy:
                case NotificationStatus.Unlocked:
                case NotificationStatus.Reassessment:
                    statusColor = "s-orange";
                    break;

                case NotificationStatus.Consented:
                    statusColor = "s-green";
                    break;

                case NotificationStatus.Objected:
                case NotificationStatus.Withdrawn:
                case NotificationStatus.ConsentWithdrawn:
                    statusColor = "s-red";
                    break;

                default:
                    statusColor = "s-blue";
                    break;
            }

            return new MvcHtmlString(statusColor);
        }
    }
}