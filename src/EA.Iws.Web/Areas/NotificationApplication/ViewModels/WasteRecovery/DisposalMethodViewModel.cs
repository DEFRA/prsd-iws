namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using System;
    using Core.Shared;

    public class DisposalMethodViewModel
    {
        public Guid NotificationId { get; set; }

        public string DisposalMethod { get; set; }

        public string Amount { get; set; }

        public ValuePerWeightUnits Units { get; set; }

        public DisposalMethodViewModel()
        {
        }

        public DisposalMethodViewModel(Guid id, string disposalMethod)
        {
            NotificationId = id;
            DisposalMethod = disposalMethod;
        }
    }
}