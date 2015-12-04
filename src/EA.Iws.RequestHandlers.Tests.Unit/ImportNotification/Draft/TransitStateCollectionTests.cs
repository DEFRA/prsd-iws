namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Draft
{
    using System;
    using System.Linq;
    using Core.ImportNotification.Draft;
    using Xunit;

    public class TransitStateCollectionTests
    {
        private readonly TransitStateCollection collection = new TransitStateCollection();

        [Theory]
        [InlineData(new[] { 1 }, 1, new int[] { })]
        [InlineData(new[] { 1, 2 }, 2, new[] { 1 })]
        [InlineData(new[] { 1, 2 }, 1, new[] { 1 })]
        [InlineData(new[] { 1, 2, 3 }, 1, new[] { 1, 2 })]
        [InlineData(new[] { 1, 2, 3 }, 2, new[] { 1, 2 })]
        [InlineData(new[] { 1, 2, 3, 4 }, 3, new[] { 1, 2, 3 })]
        [InlineData(new[] { 1, 4, 3, 2 }, 3, new[] { 1, 2, 3 })]
        [InlineData(new[] { 1, 2, 3, 3, 4 }, 3, new[] { 1, 2, 3 })]
        public void Delete(int[] positions, int remove, int[] expected)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                collection.Add(new TransitState
                {
                    Id = GetIdForTransitStateAtPosition(positions[i]),
                    OrdinalPosition = positions[i]
                });
            }

            collection.Delete(GetIdForTransitStateAtPosition(remove));

            Assert.Equal(expected, collection.TransitStates.Select(i => i.OrdinalPosition));
        }

        [Theory]
        [InlineData(new int[0], new[] { 1 })]
        [InlineData(new[] { 1 }, new[] { 1, 2 })]
        [InlineData(new[] { 1, 2 }, new[] { 1, 2, 3 })]
        [InlineData(new[] { 1, 3 }, new[] { 1, 2, 3 })]
        public void Add(int[] positions, int[] expected)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                collection.Add(new TransitState
                {
                    Id = GetIdForTransitStateAtPosition(positions[i]),
                    OrdinalPosition = positions[i]
                });
            }

            collection.Add(new TransitState
            {
                Id = new Guid("E653C2F9-A52D-45F0-B1BD-01A44FB9DC25")
            });

            Assert.Equal(expected, collection.TransitStates.Select(ts => ts.OrdinalPosition));
        }

        private Guid GetIdForTransitStateAtPosition(int n)
        {
            if (n > 9 || n < 0)
            {
                throw new ArgumentException("n must be positive and less than 10");
            }

            return new Guid("8A6C766E-7D7D-4F11-8365-E6A27BB6786" + n);
        }
    }
}
