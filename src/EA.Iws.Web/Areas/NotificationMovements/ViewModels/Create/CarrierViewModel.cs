namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Carriers;

    public class CarrierViewModel : IValidatableObject
    {
        public IList<CarrierData> NotificationCarriers { get; set; }

        public Dictionary<int, SelectList> CarrierSelectLists { get; private set; }

        public List<Guid?> SelectedItems { get; set; }

        public MeansOfTransportViewModel MeansOfTransportViewModel { get; set; }

        public int MovementNumber { get; set; }

        public CarrierViewModel()
        {
        }

        public CarrierViewModel(IList<CarrierData> notificationCarriers, int numberOfCarriers, MeansOfTransportViewModel model, int movementNumber)
        {
            MovementNumber = movementNumber;
            CarrierSelectLists = new Dictionary<int, SelectList>();
            SelectedItems = new List<Guid?>();
            MeansOfTransportViewModel = model;

            for (int i = 0; i < numberOfCarriers; i++)
            {
                CarrierSelectLists.Add(i, new SelectList(notificationCarriers, "Id", "Business.Name"));
                SelectedItems.Add(null);
            }
        }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SelectedItems.First().HasValue)
            {
                yield return new ValidationResult("Please select a carrier", new[] { "SelectedItems[0]" });
            }

            if (SelectedItems.Count > 1)
            {
                for (int i = 1; i < SelectedItems.Count; i++)
                {
                    if (!SelectedItems[i].HasValue)
                    {
                        yield return new ValidationResult("Please select a carrier", new[] { string.Format("SelectedItems[{0}]", i) });
                    }
                    else if (SelectedItems[i - 1] == SelectedItems[i])
                    {
                        yield return new ValidationResult("Consecutive carriers cannot be the same", new[] { string.Format("SelectedItems[{0}]", i) });
                    }
                }
            }
        }
    }
}