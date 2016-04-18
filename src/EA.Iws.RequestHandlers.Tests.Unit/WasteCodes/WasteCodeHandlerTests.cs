namespace EA.Iws.RequestHandlers.Tests.Unit.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using Core.WasteCodes;
    using TestHelpers.DomainFakes;

    public class WasteCodeHandlerTests
    {
        protected static readonly TestableWasteCode FirstBaselCode = new TestableWasteCode
        {
            Code = "1",
            CodeType = CodeType.Basel,
            Id = new Guid("9449E8CC-16D6-410C-8652-8B9BCAB14A19")
        };

        protected static readonly TestableWasteCode SecondBaselCode = new TestableWasteCode
        {
            Code = "2",
            CodeType = CodeType.Basel,
            Id = new Guid("81204C70-E50A-46A4-A5F4-2DA27DB15E03")
        };

        protected static readonly TestableWasteCode FirstOecdCode = new TestableWasteCode
        {
            Code = "1",
            CodeType = CodeType.Oecd,
            Id = new Guid("42D47A34-386E-4699-B965-C752A36CBF29")
        };

        protected static readonly TestableWasteCode SecondOecdCode = new TestableWasteCode
        {
            Code = "2",
            CodeType = CodeType.Oecd,
            Id = new Guid("6B8F8B0F-6643-4C59-82F7-9716573D03BB")
        };

        protected List<TestableWasteCode> wasteCodes = new List<TestableWasteCode>
        {
                FirstBaselCode,
                SecondBaselCode,
                FirstOecdCode,
                SecondOecdCode,
                new TestableWasteCode
                {
                    Code = "1",
                    CodeType = CodeType.Y,
                    Id = new Guid("060033CD-7C75-4A6A-9C8C-ABAB14C35FC2")
                },
                new TestableWasteCode
                {
                    Code = "2",
                    CodeType = CodeType.Y,
                    Id = new Guid("AEC9B57A-9D19-4262-8069-4D887E6B2C8C")
                },
                new TestableWasteCode
                {
                    Code = "1",
                    CodeType = CodeType.Ewc,
                    Id = new Guid("3FF7A224-8CA0-4A1C-8C2E-C16D9599514F")
                },
                new TestableWasteCode
                {
                    Code = "2",
                    CodeType = CodeType.Ewc,
                    Id = new Guid("2BBFA83C-E33B-46DC-9BEC-92F13F78DE29")
                }
        };
    }
}
