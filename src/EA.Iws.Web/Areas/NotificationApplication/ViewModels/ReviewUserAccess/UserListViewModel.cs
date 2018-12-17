namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.ReviewUserAccess
{
using Core.Notification;
using System;
using System.Collections.Generic;
public class UserListViewModel
{
       public Guid NotificationId { get; set; }

       public string NotificationNumber { get; set; }

       public IList<NotificationSharedUser> SharedUsers { get; set; }
}
}