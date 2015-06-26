namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Linq;
    using Prsd.Core;
    using TransportRoute;

    public partial class NotificationApplication
    {
        public void AddStateOfExportToNotification(StateOfExport stateOfExport)
        {
            Guard.ArgumentNotNull(() => stateOfExport, stateOfExport);

            if (this.StateOfExport != null)
            {
                throw new InvalidOperationException(string.Format("Cannot add a State of Export to Notification {0}. This Notification already has a State of Export {1}.",
                    this.Id,
                    this.StateOfExport.Id));
            }

            this.StateOfExport = stateOfExport;
        }

        public void SetStateOfImportForNotification(StateOfImport stateOfImport)
        {
            Guard.ArgumentNotNull(() => stateOfImport, stateOfImport);

            if (this.StateOfExport != null && this.StateOfExport.Country.Id == stateOfImport.Country.Id)
            {
                throw new InvalidOperationException(string.Format("Cannot add a State of Import in the same country as the State of Export for Notification {0}. Country: {1}",
                    this.Id,
                    this.StateOfExport.Country.Name));
            }

            this.StateOfImport = stateOfImport;
        }

        public void AddTransitStateToNotification(TransitState transitState)
        {
            Guard.ArgumentNotNull(() => transitState, transitState);

            if (TransitStatesCollection.Any(ts => ts.CompetentAuthority == transitState.CompetentAuthority))
            {
                throw new InvalidOperationException(string.Format("Cannot add a Transit State in the same Competent Authority as another in the collection. Notification {0}. Competent authority {1}",
                    this.Id,
                    transitState.CompetentAuthority.Name));
            }

            if (TransitStatesCollection.Any(ts => ts.EntryPoint.Id == transitState.EntryPoint.Id))
            {
                throw new InvalidOperationException(string.Format("Cannot add a Transit State with the same entry point as another in the collection. Notification {0}. Entry point {1}.",
                    this.Id,
                    transitState.EntryPoint.Name));
            }

            if (TransitStatesCollection.Any(ts => ts.ExitPoint.Id == transitState.ExitPoint.Id))
            {
                throw new InvalidOperationException(string.Format("Cannot add a Transit State with the same exit point as another in the collection. Notification {0}. Exit point {1}.",
                    this.Id,
                    transitState.ExitPoint.Name));
            }

            if (TransitStatesCollection.Any(ts => ts.OrdinalPosition == transitState.OrdinalPosition))
            {
                throw new InvalidOperationException(string.Format("Attempted to add a Transit State at position {0} for Notification {1}. The Notification already has a Transit State at this position.",
                    transitState.OrdinalPosition,
                    this.Id));
            }

            if (TransitStatesCollection.Count > 0)
            {
                var validPositions = GetAvailableTransitStatePositions();

                if (!validPositions.Contains(transitState.OrdinalPosition))
                {
                    throw new InvalidOperationException(string.Format("Attempted to add a Transit State at position {0} for Notification {1}. This is not a valid position for a new Transit State.",
                        transitState.OrdinalPosition,
                        this.Id));
                }
            }

            this.TransitStatesCollection.Add(transitState);
        }

        public void UpdateTransitStateForNotification(Guid targetTransitStateId, Country country, 
            CompetentAuthority competentAuthority, 
            EntryOrExitPoint entryPoint,
            EntryOrExitPoint exitPoint, 
            int? ordinalPosition)
        {
            var targetTransitState = this.TransitStatesCollection.Single(ts => ts.Id == targetTransitStateId);

            if (ordinalPosition.HasValue && ordinalPosition < 1)
            {
                ordinalPosition = null;
            }

            var allTransitStatesExceptTarget = this.TransitStatesCollection.Where(ts => ts.Id != targetTransitStateId).ToArray();

            if (allTransitStatesExceptTarget.Any(ts => ts.Country.Id == country.Id))
            {
                throw new InvalidOperationException(string.Format("Cannot edit a Transit State to put it in the same Country as another in the collection. Notification {0}. Country {1}",
                    this.Id,
                    country.Name));
            }

            if (ordinalPosition.HasValue && allTransitStatesExceptTarget.Any(ts => ts.OrdinalPosition == ordinalPosition))
            {
                throw new InvalidOperationException(string.Format("Attempted to edit a Transit State {0} to position {1} for Notification {2}. The Notification already has another Transit State at this position.",
                    targetTransitStateId,
                    ordinalPosition.Value,
                    this.Id));
            }

            targetTransitState.UpdateTransitState(country, competentAuthority, entryPoint, exitPoint, ordinalPosition);
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

        public void UpdateStateOfExport(Country country, CompetentAuthority competentAuthority,
            EntryOrExitPoint exitPoint)
        {
            Guard.ArgumentNotNull(() => country, country);
            Guard.ArgumentNotNull(() => competentAuthority, competentAuthority);
            Guard.ArgumentNotNull(() => exitPoint, exitPoint);

            if (this.StateOfExport == null)
            {
                this.AddStateOfExportToNotification(new StateOfExport(country, competentAuthority, exitPoint));
                return;
            }

            if (this.StateOfImport != null && this.StateOfImport.Country.Id == country.Id)
            {
                throw new InvalidOperationException(string.Format("Attempted to edit the State of Export for Notification {0}. Cannot have a State of Export in the same country as the State of Import: {1}",
                    this.Id,
                    country.Name));
            }

            if (this.TransitStates.Any(ts => ts.Country.Id == country.Id))
            {
                throw new InvalidOperationException(string.Format("Attempted to edit the State of Export for Notification {0}. Cannot have a State of Export in the same country as a Transit State: {1}",
                    this.Id,
                    country.Name));
            }

            this.StateOfExport.Update(country, competentAuthority, exitPoint);
        }
    }
}
