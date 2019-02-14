namespace EA.Iws.Domain.TransportRoute
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.CustomsOffice;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class TransportRoute : Entity
    {
        // TODO - split customs offices to new aggregate, but for now use
        // the service in the domain object.
        private readonly Lazy<RequiredCustomsOffices> requiredCustomsOffices =
            new Lazy<RequiredCustomsOffices>(() => new RequiredCustomsOffices());

        protected TransportRoute()
        {
        }

        public TransportRoute(Guid notificationId)
        {
            NotificationId = notificationId;
            TransitStatesCollection = new List<TransitState>();
        }

        public virtual StateOfExport StateOfExport { get; private set; }

        public virtual StateOfImport StateOfImport { get; private set; }

        protected virtual ICollection<TransitState> TransitStatesCollection { get; set; }

        public IEnumerable<TransitState> TransitStates
        {
            get { return TransitStatesCollection.ToSafeIEnumerable(); }
        }

        public Guid NotificationId { get; private set; }

        public virtual ExitCustomsOffice ExitCustomsOffice { get; private set; }

        public virtual EntryCustomsOffice EntryCustomsOffice { get; private set; }
        public virtual EntryExitCustomsSelection EntryExitCustomsSelection { get; private set; }

        public void SetStateOfExportForNotification(StateOfExport stateOfExport)
        {
            Guard.ArgumentNotNull(() => stateOfExport, stateOfExport);

            if (StateOfImport != null && StateOfImport.Country.Id == stateOfExport.Country.Id)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Cannot add a State of Export in the same country as the State of Import for TransportRoute {0}. Country: {1}",
                        Id,
                        StateOfExport.Country.Name));
            }

            StateOfExport = stateOfExport;
        }

        public void SetStateOfImportForNotification(StateOfImport stateOfImport)
        {
            Guard.ArgumentNotNull(() => stateOfImport, stateOfImport);

            if (StateOfExport != null && StateOfExport.Country.Id == stateOfImport.Country.Id)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Cannot add a State of Import in the same country as the State of Export for TransportRoute {0}. Country: {1}",
                        Id,
                        StateOfExport.Country.Name));
            }

            StateOfImport = stateOfImport;
        }

        public void AddTransitStateToNotification(TransitState transitState)
        {
            Guard.ArgumentNotNull(() => transitState, transitState);

            if (TransitStatesCollection.Any(ts => ts.OrdinalPosition == transitState.OrdinalPosition))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Attempted to add a Transit State at position {0} for TransportRoute {1}. The TransportRoute already has a Transit State at this position.",
                        transitState.OrdinalPosition,
                        Id));
            }

            if (TransitStatesCollection.Count > 0)
            {
                var validPositions = GetAvailableTransitStatePositions();

                if (!validPositions.Contains(transitState.OrdinalPosition))
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "Attempted to add a Transit State at position {0} for TransportRoute {1}. This is not a valid position for a new Transit State.",
                            transitState.OrdinalPosition,
                            Id));
                }
            }

            TransitStatesCollection.Add(transitState);
        }

        public void UpdateTransitStateForNotification(Guid targetTransitStateId, Country country,
            CompetentAuthority competentAuthority,
            EntryOrExitPoint entryPoint,
            EntryOrExitPoint exitPoint,
            int? ordinalPosition)
        {
            var targetTransitState = TransitStatesCollection.Single(ts => ts.Id == targetTransitStateId);

            if (ordinalPosition.HasValue && ordinalPosition < 1)
            {
                ordinalPosition = null;
            }

            var allTransitStatesExceptTarget =
                TransitStatesCollection.Where(ts => ts.Id != targetTransitStateId).ToArray();

            if (ordinalPosition.HasValue &&
                allTransitStatesExceptTarget.Any(ts => ts.OrdinalPosition == ordinalPosition))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Attempted to edit a Transit State {0} to position {1} for TransportRoute {2}. The TransportRoute already has another Transit State at this position.",
                        targetTransitStateId,
                        ordinalPosition.Value,
                        Id));
            }

            targetTransitState.UpdateTransitState(country, competentAuthority, entryPoint, exitPoint, ordinalPosition);

            CheckAllTransitStatesInEU();
        }

        public int[] GetAvailableTransitStatePositions()
        {
            if (TransitStatesCollection.Count == 0)
            {
                return new[] { 1 };
            }

            var currentOrdinalPositionsSet = TransitStatesCollection.Select(ts => ts.OrdinalPosition)
                .OrderBy(i => i).ToArray();

            // We construct a list of positions available for this item by checking the positions already occupied on the notification.
            // For the series 1, 2 the valid position is 3.
            // For the series 1, 3 the valid positions are 2, 4
            var validPositions =
                Enumerable.Range(1, currentOrdinalPositionsSet[currentOrdinalPositionsSet.Length - 1] + 1)
                    .Where(i => !currentOrdinalPositionsSet.Contains(i))
                    .ToArray();

            return validPositions;
        }

        public void RemoveTransitState(Guid id)
        {
            var transitState = TransitStatesCollection.Single(ts => ts.Id == id);

            var removedPosition = transitState.OrdinalPosition;

            TransitStatesCollection.Remove(transitState);

            // Down-shift all transit states above the removed item.
            foreach (var state in TransitStatesCollection.Where(ts => ts.OrdinalPosition > removedPosition))
            {
                state.UpdateOrdinalPosition(state.OrdinalPosition - 1);
            }

            CheckAllTransitStatesInEU();
        }

        public void SetExitCustomsOffice(ExitCustomsOffice customsOffice)
        {
            var customsOfficeRequiredStatus = requiredCustomsOffices.Value.GetForTransportRoute(this);

            switch (customsOfficeRequiredStatus)
            {
                case CustomsOffices.EntryAndExit:
                case CustomsOffices.Exit:
                    this.ExitCustomsOffice = customsOffice;
                    break;
                default:
                    throw new InvalidOperationException("Cannot set an exit customs office for Notification " + this.Id
                        + ". The Notification only requires the following customs offices: " + customsOfficeRequiredStatus);
            }
        }

        public void SetEntryCustomsOffice(EntryCustomsOffice customsOffice)
        {
            var customsOfficeRequiredStatus = requiredCustomsOffices.Value.GetForTransportRoute(this);

            switch (customsOfficeRequiredStatus)
            {
                case CustomsOffices.EntryAndExit:
                case CustomsOffices.Entry:
                    this.EntryCustomsOffice = customsOffice;
                    break;
                default:
                    throw new InvalidOperationException("Cannot set an entry customs office for Notification " + this.Id
                        + ". The Notification only requires the following customs offices: " + customsOfficeRequiredStatus);
            }
        }

        public void SetEntryExitCustomsSelection(EntryExitCustomsSelection selection)
        {
            this.EntryExitCustomsSelection = selection;
        }

        private void CheckAllTransitStatesInEU()
        {
            if (TransitStates.All(ts => ts.Country.IsEuropeanUnionMember))
            {
                RaiseEvent(new AllTransitStatesInEUEvent(this));
            }
        }
    }
}