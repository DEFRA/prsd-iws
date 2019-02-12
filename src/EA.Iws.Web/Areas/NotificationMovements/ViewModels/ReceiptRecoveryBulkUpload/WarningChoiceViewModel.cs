namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.ReceiptRecoveryBulkUpload
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using Core.Shared;
    using Web.ViewModels.Shared;

    public class WarningChoiceViewModel
    {
        public WarningChoiceViewModel()
        {
        }

        public WarningChoiceViewModel(Guid notificationId, NotificationType type)
        {
            WarningChoices = RadioButtonStringCollectionViewModel.CreateFromEnum<WarningChoicesList>();
            this.NotificationId = notificationId;
            this.NotificationType = type;
        }

        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }

        public RadioButtonStringCollectionViewModel WarningChoices { get; set; }

        public string GetEnumDisplayValue(WarningChoicesList choice)
        {
            Type choiceType = choice.GetType();
            MemberInfo[] memberInfo = choiceType.GetMember(choice.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false);
                if ((attribs != null && attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DataAnnotations.DisplayAttribute)attribs.ElementAt(0)).Name;
                }
            }
            return choice.ToString();
        }
    }

    public enum WarningChoicesList
    {
        [Display(Name = "Return to upload the accompanying shipment movement documents")]
        Return = 0,
        [Display(Name = "Leave the process and return to notification options")]
        Leave = 1
    }
}