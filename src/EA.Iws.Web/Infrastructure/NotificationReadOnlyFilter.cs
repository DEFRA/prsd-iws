namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    public class NotificationReadOnlyFilter : ActionFilterAttribute
    {
        public IMediator Mediator { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.DataTokens != null && (string)filterContext.RouteData.DataTokens["area"] == "NotificationApplication")
            {
                Guid notificationId;
                if (Guid.TryParse(filterContext.Controller.ValueProvider.GetValue("id").AttemptedValue,
                    out notificationId))
                {
                    bool canEdit = Mediator.SendAsync(new CanEditNotification(notificationId)).Result;

                    if (!canEdit)
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary
                            { 
                                { "controller", "ReadOnly"},
                                { "action", "Index" },
                                { "Area", "NotificationApplication" }
                            });
                    }
                }
                else
                {
                    filterContext.Result = new HttpNotFoundResult();
                }
            }
        }
    }
}