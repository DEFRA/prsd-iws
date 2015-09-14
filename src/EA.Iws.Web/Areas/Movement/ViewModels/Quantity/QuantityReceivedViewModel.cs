namespace EA.Iws.Web.Areas.Movement.ViewModels.Quantity
{
    using System.ComponentModel.DataAnnotations;
    using Core.Shared;

    public class QuantityReceivedViewModel
    {
        public ShipmentQuantityUnits MovementUnits { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal? Quantity { get; set; }
    }
}