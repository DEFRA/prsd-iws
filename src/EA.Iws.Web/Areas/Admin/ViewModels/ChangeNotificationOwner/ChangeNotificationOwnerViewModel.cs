namespace EA.Iws.Web.Areas.Admin.ViewModels.ChangeNotificationOwner
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Admin;

    public class ChangeNotificationOwnerViewModel
    {
        public ChangeNotificationOwnerViewModel()
        {
        }

        public ChangeNotificationOwnerViewModel(ChangeUserData data)
        {
            CurrentUser = data;
        }

        public Guid NotificationId { get; set; }

        public ChangeUserData CurrentUser { get; set; }

        [Required(ErrorMessageResourceType = typeof(ChangeNotificationOwnerViewModelResources), ErrorMessageResourceName = "EmailRequired")]
        [Display(Name = "Email", ResourceType = typeof(ChangeNotificationOwnerViewModelResources))]
        public string SelectedUser { get; set; }

        public SelectList NewUserSelectList
        {
            get
            {
                if (AllUsers != null)
                {
                    var emails = AllUsers.Select(u => new
                    {
                        Key = u.Email,
                        Value = u.UserId
                    });

                    return new SelectList(emails, "Value", "Key", null); 
                }

                return null;
            }
        }

        public IList<ChangeUserData> AllUsers { get; set; } 
    }
}