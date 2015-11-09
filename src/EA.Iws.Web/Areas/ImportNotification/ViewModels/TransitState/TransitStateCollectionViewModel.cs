namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.TransitState
{
    using System.Collections.Generic;
    using Core.TransitState;

    public class TransitStateCollectionViewModel
    {
        public bool HasNoTransitStates { get; set; }

        public List<TransitStateData> TransitStates { get; set; }

        public TransitStateCollectionViewModel()
        {
            TransitStates = new List<TransitStateData>();
        }
    }
}