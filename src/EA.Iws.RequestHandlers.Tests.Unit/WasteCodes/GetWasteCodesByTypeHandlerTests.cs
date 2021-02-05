namespace EA.Iws.RequestHandlers.Tests.Unit.WasteCodes
{
    using Core.WasteCodes;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using RequestHandlers.Mappings;
    using RequestHandlers.WasteCodes;
    using Requests.WasteCodes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetWasteCodesByTypeHandlerTests : WasteCodeHandlerTests
    {
        private readonly GetWasteCodesByTypeHandler handler;
        private readonly IWasteCodeRepository wasteCodeRepository;

        public GetWasteCodesByTypeHandlerTests()
        {
            wasteCodeRepository = A.Fake<IWasteCodeRepository>();

            handler = new GetWasteCodesByTypeHandler(wasteCodeRepository, new WasteCodeMap());

            A.CallTo(() => wasteCodeRepository.GetAllWasteCodes()).Returns(wasteCodes);
        }

        [Fact]
        public async Task WithNullLookupType_ReturnsAllWasteCodes()
        {
            var result = await handler.HandleAsync(new GetWasteCodesByType(null));

            Assert.Equal(wasteCodes.Count, result.Length);
        }

        [Fact]
        public async Task WithEmptyLookupType_ReturnsAllWasteCodes()
        {
            var result = await handler.HandleAsync(new GetWasteCodesByType());

            Assert.Equal(wasteCodes.Count, result.Length);
        }

        [Fact]
        public async Task WithSpecificLookupType_ReturnsAllOfOneCodeType()
        {
            var result = await handler.HandleAsync(new GetWasteCodesByType(CodeType.Basel));

            Assert.Equal(wasteCodes.Count(wc => wc.CodeType == CodeType.Basel), result.Length);
        }
    }
}
