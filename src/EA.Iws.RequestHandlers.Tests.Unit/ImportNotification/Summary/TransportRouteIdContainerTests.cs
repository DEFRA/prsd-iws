namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Summary
{
    using System;
    using RequestHandlers.ImportNotification.Summary;
    using Xunit;

    public class TransportRouteIdContainerTests
    {
        private static readonly Guid FirstGuid = new Guid("39DB89EA-013B-41F1-8ADA-47952FFDE925");
        private static readonly Guid SecondGuid = new Guid("C59B0179-2531-4BA3-BE55-C817245CC3E9");

        private readonly TransportRouteIdContainer container = new TransportRouteIdContainer();
        
        [Fact]
        public void AddNullGuid_EntryOrExitPoint_DoesNotAdd()
        {
            container.AddEntryOrExitPoint(null);

            Assert.Empty(container.EntryOfExitPointIds);
        }

        [Fact]
        public void EntryOrExitPoint_AddsToList()
        {
            container.AddEntryOrExitPoint(FirstGuid);

            Assert.Contains(FirstGuid, container.EntryOfExitPointIds);
        }

        [Fact]
        public void EntryOrExitPoint_DoesNotChangeCompetentAuthorities()
        {
            container.AddEntryOrExitPoint(FirstGuid);

            Assert.Empty(container.CompetentAuthorityIds);
        }

        [Fact]
        public void CompetentAuthority_AddsToList()
        {
            container.AddCompetentAuthority(FirstGuid);

            Assert.Contains(FirstGuid, container.CompetentAuthorityIds);
        }

        [Fact]
        public void CompetentAuthority_DoesNotChangeEntryOrExitPoint()
        {
            container.AddCompetentAuthority(FirstGuid);

            Assert.Empty(container.EntryOfExitPointIds);
        }

        [Fact]
        public void EntryOrExitPoints_WithNullGuid_AddsToList()
        {
            container.AddEntryOrExitPoints(new Guid?[]
            {
                FirstGuid,
                null,
                SecondGuid
            });

            Assert.Equal(new[]
            {
                FirstGuid,
                SecondGuid
            }, container.EntryOfExitPointIds);
        }

        [Fact]
        public void CompetentAuthorities_AddsToList()
        {
            container.AddCompetentAuthorities(new Guid?[]
            {
                FirstGuid,
                SecondGuid
            });

            Assert.Equal(new[]
            {
                FirstGuid,
                SecondGuid
            }, container.CompetentAuthorityIds);
        }
    }
}
