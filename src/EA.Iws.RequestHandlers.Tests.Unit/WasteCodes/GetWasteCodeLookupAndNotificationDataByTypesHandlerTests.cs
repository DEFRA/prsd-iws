namespace EA.Iws.RequestHandlers.Tests.Unit.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using DataAccess;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Mappings;
    using RequestHandlers.WasteCodes;
    using Requests.WasteCodes;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetWasteCodeLookupAndNotificationDataByTypesHandlerTests : WasteCodeHandlerTests
    {
        private readonly GetWasteCodeLookupAndNotificationDataByTypesHandler handler;
        private static readonly Guid NotificationWithWasteCodesId = new Guid("BA8A31E3-7F2F-4CB3-AD18-610269FF7787");
        private static readonly Guid NotificationWithoutWasteCodesId = new Guid("C1D93DA2-E2EB-4915-AB6D-FE857E3C95EA");

        public GetWasteCodeLookupAndNotificationDataByTypesHandlerTests()
        {
            var context = new TestIwsContext();

            handler = new GetWasteCodeLookupAndNotificationDataByTypesHandler(context, new WasteCodeMap());

            context.WasteCodes.AddRange(wasteCodes.Select(wc => wc as WasteCode).ToList());

            context.NotificationApplications.AddRange(new NotificationApplication[]
                {
                    new TestableNotificationApplication
                    {
                        Id = NotificationWithWasteCodesId, 
                        WasteCodes = new WasteCodeInfo[]
                        {
                            new TestableWasteCodeInfo(FirstBaselCode) 
                        }
                    },
                    new TestableNotificationApplication
                    {
                        Id = NotificationWithoutWasteCodesId
                    }
                });
        }

        [Fact]
        public async Task WithNullLookupTypesList_ReturnsAllCodes()
        {
            var result = await handler.HandleAsync(new GetWasteCodeLookupAndNotificationDataByTypes(NotificationWithoutWasteCodesId));

            Assert.Equal(wasteCodes.GroupBy(wc => wc.CodeType).Count(), result.LookupWasteCodeData.Count);
        }

        [Fact]
        public async Task WithSpecificLookupTypesList_ReturnsAllOfOneCodeType()
        {
            var result =
                await
                    handler.HandleAsync(new GetWasteCodeLookupAndNotificationDataByTypes(
                        NotificationWithoutWasteCodesId, 
                        new List<CodeType>
                        {
                            CodeType.Basel, 
                            CodeType.Oecd
                        }));

            var baselCodes = result.LookupWasteCodeData[CodeType.Basel];
            Assert.Equal(wasteCodes.Count(wc => wc.CodeType == CodeType.Basel), baselCodes.Length);
        }

        [Fact]
        public async Task WithSpecificLookupTypesList_ReturnsAllCodeTypesInDictionary()
        {
            var result =
                await
                    handler.HandleAsync(
                        new GetWasteCodeLookupAndNotificationDataByTypes(NotificationWithoutWasteCodesId,
                        new List<CodeType>
                        {
                            CodeType.Basel,
                            CodeType.Y
                        }));

            Assert.True(result.LookupWasteCodeData.ContainsKey(CodeType.Basel));
            Assert.True(result.LookupWasteCodeData.ContainsKey(CodeType.Y));
            Assert.False(result.LookupWasteCodeData.ContainsKey(CodeType.Oecd));
        }

        [Fact]
        public async Task WithSpecificNotificationTypes_ReturnsNotificationCodes()
        {
            var codes = new List<CodeType>
            {
                CodeType.Basel
            };

            var result =
                await
                    handler.HandleAsync(new GetWasteCodeLookupAndNotificationDataByTypes(NotificationWithWasteCodesId,
                        codes, codes));

            Assert.True(result.LookupWasteCodeData.ContainsKey(CodeType.Basel));
            Assert.True(result.NotificationWasteCodeData.ContainsKey(CodeType.Basel));
            Assert.Equal(1, result.NotificationWasteCodeData[CodeType.Basel].Length);
            Assert.Equal(wasteCodes.Count(wc => wc.CodeType == CodeType.Basel), result.LookupWasteCodeData[CodeType.Basel].Length);
        }

        [Fact]
        public async Task WithNotificationTypeDifferentFromLookupType_ReturnsCorrectResults()
        {
            var result =
                await handler.HandleAsync(new GetWasteCodeLookupAndNotificationDataByTypes(NotificationWithWasteCodesId,
                    new List<CodeType>
                    {
                        CodeType.Y
                    },
                    new List<CodeType>
                    {
                        CodeType.Basel
                    }));

            Assert.False(result.LookupWasteCodeData.ContainsKey(CodeType.Basel));
            Assert.False(result.NotificationWasteCodeData.ContainsKey(CodeType.Y));
            Assert.True(result.NotificationWasteCodeData.ContainsKey(CodeType.Basel));
        }

        [Fact]
        public async Task WithNotificationTypeNotContainedInNotification_ReturnsEmptyArray()
        {
            var result =
                await
                    handler.HandleAsync(new GetWasteCodeLookupAndNotificationDataByTypes(
                        NotificationWithoutWasteCodesId, new List<CodeType>
                        {
                            CodeType.Y
                        },
                        new List<CodeType>
                        {
                            CodeType.Y
                        }));

            Assert.True(result.NotificationWasteCodeData.ContainsKey(CodeType.Y));
            Assert.Empty(result.NotificationWasteCodeData[CodeType.Y]);
        }
    }
}
