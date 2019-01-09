namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    public class NotificationOwnerFilter : ActionFilterAttribute
    {
        public IMediator Mediator { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(SkipNotificationOwnerFilter), true).Any())
            {
                return;
            }

            Guid notificationId;
            if (Guid.TryParse(filterContext.Controller.ValueProvider.GetValue("id").AttemptedValue,
                out notificationId))
            {
                var isOwner = Mediator.SendAsync(new CheckIfNotificationOwner(notificationId)).Result;

                if (!isOwner)
                {
                    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }
            else
            {
                filterContext.Result = new HttpNotFoundResult();
            }
        }
    }
}