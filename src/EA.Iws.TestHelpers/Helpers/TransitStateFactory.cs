namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using Domain;
    using Domain.TransportRoute;

    public class TransitStateFactory
    {
        public static TransitState Create(Guid id, Country country, int ordinalPosition)
        {
            // Note that the use of Guid.NewGuid() is non-deterministic and may cause random test failures.
            var transitState = new TransitState(country,
                CompetentAuthorityFactory.Create(Guid.NewGuid(), country),
                EntryOrExitPointFactory.Create(Guid.NewGuid(), country),
                EntryOrExitPointFactory.Create(Guid.NewGuid(), country),
                ordinalPosition);

            EntityHelper.SetEntityId(transitState, id);

            return transitState;
        }
    }
}
