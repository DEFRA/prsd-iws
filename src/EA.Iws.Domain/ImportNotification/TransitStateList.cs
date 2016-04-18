namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;

    public class TransitStateList : IEnumerable<TransitState>
    {
        private readonly List<TransitState> transitStates;

        public TransitStateList(IEnumerable<TransitState> transitStates)
        {
            var states = transitStates as TransitState[] ?? transitStates.ToArray();

            var ordinals = states.Select(s => s.OrdinalPosition).ToArray();

            if (!ordinals.IsUnique())
            {
                throw new ArgumentException("Transit states must have unique ordinal positions", "transitStates");
            }

            if (!ordinals.IsConsecutive())
            {
                throw new ArgumentException("Transit states must have consecutive ordinal positions", "transitStates");
            }

            this.transitStates = new List<TransitState>(states.OrderBy(s => s.OrdinalPosition));
        }

        public IEnumerator<TransitState> GetEnumerator()
        {
            return transitStates.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}