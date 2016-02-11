namespace EA.Iws.Web.Tests.Unit.ViewModels.NotificationMovements
{
    using System;
    using System.Collections.Generic;
    using Areas.NotificationMovements.ViewModels.CancelMovement;
    using Xunit;

    public class CancelSuccessViewModelTests
    {
        private static readonly Guid AnyGuid = new Guid("08732B54-F73A-4718-83E7-85CFA63E8002");
        
        [Fact]
        public void OneShipment_CorrectHeading()
        {
            var viewModel = new SuccessViewModel(AnyGuid, new List<int> { 1 });

            Assert.Equal("You've successfully cancelled shipment 1", viewModel.HeadingText);
        }

        [Fact]
        public void TwoShipments_CorrectHeading()
        {
            var viewModel = new SuccessViewModel(AnyGuid, new List<int> { 1, 5 });

            Assert.Equal("You've successfully cancelled shipments 1 and 5", viewModel.HeadingText);
        }

        [Fact]
        public void MoreShipments_CorrectHeading()
        {
            var viewModel = new SuccessViewModel(AnyGuid, new List<int> { 1, 5, 9, 21 });

            Assert.Equal("You've successfully cancelled shipments 1, 5, 9 and 21", viewModel.HeadingText);
        }
    }
}
