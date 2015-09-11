namespace EA.Iws.Web.Areas.Movement.ViewModels.Carrier
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Carriers;

    public class CarrierViewModel : IValidatableObject
    {
        public Guid MovementId { get; set; }

        public List<CarrierData> NotificationCarriers { get; set; }

        public Dictionary<int, SelectList> CarrierSelectLists { get; private set; }

        public List<Guid?> SelectedItems { get; set; }

        public MeansOfTransportViewModel MeansOfTransportViewModel { get; set; }

        public CarrierViewModel()
        {
            CarrierSelectLists = new Dictionary<int, SelectList>();
            SelectedItems = new List<Guid?>();
        }

        public void SetCarrierSelectLists(Dictionary<int, CarrierData> selectedCarriers, int numberOfCarriers)
        {
            for (int i = 0; i < numberOfCarriers; i++)
            {
                if (i < selectedCarriers.Count)
                {
                    var selectList = new SelectList(NotificationCarriers, "Id", "Business.Name", selectedCarriers[i].Id);

                    CarrierSelectLists.Add(i, selectList);
                    SelectedItems.Add(selectedCarriers[i].Id);
                }
                else
                {
                    var selectList = new SelectList(NotificationCarriers, "Id", "Business.Name");

                    CarrierSelectLists.Add(i, selectList);
                    SelectedItems.Add(null);
                }
            }
        }

        private Dictionary<int, CarrierData> MapSelectedCarriers(IEnumerable<Guid?> selected, IEnumerable<CarrierData> actualCarriers)
        {
            var selectedCarriers = new Dictionary<int, CarrierData>();
            int order = 0;

            foreach (var item in selected)
            {
                if (item.HasValue)
                {
                    selectedCarriers.Add(order, actualCarriers.Single(c => c.Id == item));
                    order++;
                }
            }

            return selectedCarriers;
        }

        public void SetCarrierSelectListsFromSelectedValues()
        {
            SetCarrierSelectLists(MapSelectedCarriers(SelectedItems, NotificationCarriers), SelectedItems.Count);
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