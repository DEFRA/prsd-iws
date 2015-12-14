namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Core.WasteCodes;
    using Domain.ImportNotification;
    using Domain.ImportNotification.WasteCodes;
    using Xunit;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Variables relate to waste codes")]
    public class WasteTypeTests
    {
        private static readonly Guid ImportNotificationId = new Guid("CEE1C400-8EA5-4D94-89AE-25E2D98B0675");

        [Fact]
        public void CanCreateWasteType()
        {
            var wasteType = new WasteType(ImportNotificationId,
                "WasteTypeName",
                BaselOecdCode.CreateFor(AnyWasteCode(CodeType.Basel)),
                EwcCode.CreateFor(new[] { AnyWasteCode(CodeType.Ewc) }),
                YCode.CreateFor(new[] { AnyWasteCode(CodeType.Y) }),
                HCode.CreateFor(new[] { AnyWasteCode(CodeType.H) }),
                UnClass.CreateFor(new[] { AnyWasteCode(CodeType.Un) }));

            Assert.IsType<WasteType>(wasteType);
        }

        [Fact]
        public void BaselCodeNotApplicable_NotListedTrue()
        {
            var wasteType = new WasteType(ImportNotificationId,
                "WasteTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[] { AnyWasteCode(CodeType.Ewc) }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable());

            Assert.True(wasteType.BaselOecdCodeNotListed);
        }

        [Fact]
        public void YCodesNotApplicable_NotApplicableTrue()
        {
            var wasteType = new WasteType(ImportNotificationId,
                "WasteTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[] { AnyWasteCode(CodeType.Ewc) }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable());

            Assert.True(wasteType.YCodeNotApplicable);
        }

        [Fact]
        public void HCodesNotApplicable_NotApplicableTrue()
        {
            var wasteType = new WasteType(ImportNotificationId,
                "WasteTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[] { AnyWasteCode(CodeType.Ewc) }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable());

            Assert.True(wasteType.HCodeNotApplicable);
        }

        [Fact]
        public void UnClassNotApplicable_NotApplicableTrue()
        {
            var wasteType = new WasteType(ImportNotificationId,
                "WasteTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[] { AnyWasteCode(CodeType.Ewc) }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable());

            Assert.True(wasteType.UnClassNotApplicable);
        }

        [Fact]
        public void BaselOecdCode_CreateFor_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => BaselOecdCode.CreateFor(null));
        }

        [Fact]
        public void EwcCode_CreateFor_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => EwcCode.CreateFor(null));
        }

        [Fact]
        public void YCode_CreateFor_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => YCode.CreateFor(null));
        }

        [Fact]
        public void HCode_CreateFor_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => HCode.CreateFor(null));
        }

        [Fact]
        public void UnClass_CreateFor_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnClass.CreateFor(null));
        }

        [Fact]
        public void Name_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new WasteType(ImportNotificationId,
                null,
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[]
                {
                    AnyWasteCode(CodeType.Ewc)
                }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable()));
        }

        [Fact]
        public void Name_Empty_Throws()
        {
            Assert.Throws<ArgumentException>(() => new WasteType(ImportNotificationId,
                string.Empty,
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[]
                {
                    AnyWasteCode(CodeType.Ewc)
                }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable()));
        }

        [Fact]
        public void ImportNotificationIdDefaultValue_Throws()
        {
            Assert.Throws<ArgumentException>(() => new WasteType(Guid.Empty,
                "WasteTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[]
                {
                                AnyWasteCode(CodeType.Ewc)
                }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable()));
        }

        [Fact]
        public void BaselOecdCode_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new WasteType(ImportNotificationId,
                "WasteTypeName",
                null,
                EwcCode.CreateFor(new[]
                {
                    AnyWasteCode(CodeType.Ewc)
                }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable()));
        }

        [Fact]
        public void EwcCodeNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new WasteType(ImportNotificationId,
                "WasteTypeName",
                BaselOecdCode.CreateNotListed(),
                null,
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable()));
        }

        [Fact]
        public void YCodeNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new WasteType(ImportNotificationId,
                "WasteTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[]
                {
                    AnyWasteCode(CodeType.Ewc)
                }),
                null,
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable()));
        }

        [Fact]
        public void HCodeNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new WasteType(ImportNotificationId,
                "WasteTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[]
                {
                    AnyWasteCode(CodeType.Ewc)
                }),
                YCode.CreateNotApplicable(),
                null,
                UnClass.CreateNotApplicable()));
        }

        [Fact]
        public void UnClassNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new WasteType(ImportNotificationId,
                "WasteTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[]
                {
                    AnyWasteCode(CodeType.Ewc)
                }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                null));
        }

        [Fact]
        public void BaselCodeIsSet()
        {
            var baselCodeId = new Guid("72669DEE-1126-47B3-8C45-2395F54C82D8");
            var baselCode = new WasteCode(baselCodeId, CodeType.Basel);

            var wasteType = new WasteType(ImportNotificationId,
                "WasteTypeName",
                BaselOecdCode.CreateFor(baselCode),
                EwcCode.CreateFor(new[] { AnyWasteCode(CodeType.Ewc) }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable());

            Assert.Equal(baselCodeId, wasteType.WasteCodes.Single(wc => wc.Type == CodeType.Basel).WasteCodeId);
        }

        [Fact]
        public void OecdCodeIsSet()
        {
            var oecdCodeId = new Guid("A58E7CE8-FA46-4E35-A3ED-26E395678CA0");
            var oecdCode = new WasteCode(oecdCodeId, CodeType.Oecd);

            var wasteType = new WasteType(ImportNotificationId,
                "WasteTypeName",
                BaselOecdCode.CreateFor(oecdCode),
                EwcCode.CreateFor(new[] { AnyWasteCode(CodeType.Ewc) }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable());

            Assert.Equal(oecdCodeId, wasteType.WasteCodes.Single(wc => wc.Type == CodeType.Oecd).WasteCodeId);
        }

        [Theory]
        [InlineData(CodeType.CustomsCode)]
        [InlineData(CodeType.Ewc)]
        [InlineData(CodeType.ExportCode)]
        [InlineData(CodeType.H)]
        [InlineData(CodeType.ImportCode)]
        [InlineData(CodeType.OtherCode)]
        [InlineData(CodeType.Un)]
        [InlineData(CodeType.UnNumber)]
        [InlineData(CodeType.Y)]
        public void BaselCodeTypeNotBaselOecd_Throws(CodeType codeType)
        {
            Assert.Throws<ArgumentException>(() => BaselOecdCode.CreateFor(AnyWasteCode(codeType)));
        }

        [Fact]
        public void EwcCodesAreSet()
        {
            var ewcCodeId = new Guid("A4DA3323-B6BD-4245-B144-B9E05661BEC6");
            var ewcCode = new WasteCode(ewcCodeId, CodeType.Ewc);

            var wasteType = new WasteType(ImportNotificationId,
                "WateTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[] { ewcCode }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable());

            Assert.Equal(ewcCodeId, wasteType.WasteCodes.Single(wc => wc.Type == CodeType.Ewc).WasteCodeId);
        }

        [Theory]
        [InlineData(CodeType.CustomsCode)]
        [InlineData(CodeType.Basel)]
        [InlineData(CodeType.Oecd)]
        [InlineData(CodeType.ExportCode)]
        [InlineData(CodeType.H)]
        [InlineData(CodeType.ImportCode)]
        [InlineData(CodeType.OtherCode)]
        [InlineData(CodeType.Un)]
        [InlineData(CodeType.UnNumber)]
        [InlineData(CodeType.Y)]
        public void AnyEwcCodesWrongType_Throws(CodeType codeType)
        {
            Assert.Throws<ArgumentException>(() => EwcCode.CreateFor(new[] { AnyWasteCode(codeType) }));
        }

        [Fact]
        public void YCodesAreSet()
        {
            var yCodeId = new Guid("5BACE9DF-5575-4AB9-BEF4-CE639AE1356B");
            var yCode = new WasteCode(yCodeId, CodeType.Y);

            var wasteType = new WasteType(ImportNotificationId,
                "WateTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[] { AnyWasteCode(CodeType.Ewc) }),
                YCode.CreateFor(new[] { yCode }),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable());

            Assert.Equal(yCodeId, wasteType.WasteCodes.Single(wc => wc.Type == CodeType.Y).WasteCodeId);
        }

        [Theory]
        [InlineData(CodeType.CustomsCode)]
        [InlineData(CodeType.Basel)]
        [InlineData(CodeType.Oecd)]
        [InlineData(CodeType.Ewc)]
        [InlineData(CodeType.ExportCode)]
        [InlineData(CodeType.H)]
        [InlineData(CodeType.ImportCode)]
        [InlineData(CodeType.OtherCode)]
        [InlineData(CodeType.Un)]
        [InlineData(CodeType.UnNumber)]
        public void AnyYCodesWrongType_Throws(CodeType codeType)
        {
            Assert.Throws<ArgumentException>(() => YCode.CreateFor(new[] { AnyWasteCode(codeType) }));
        }

        [Fact]
        public void HCodesAreSet()
        {
            var hCodeId = new Guid("A4DA3323-B6BD-4245-B144-B9E05661BEC6");
            var hCode = new WasteCode(hCodeId, CodeType.H);

            var wasteType = new WasteType(ImportNotificationId,
                "WateTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[] { AnyWasteCode(CodeType.Ewc) }),
                YCode.CreateNotApplicable(),
                HCode.CreateFor(new[] { hCode }),
                UnClass.CreateNotApplicable());

            Assert.Equal(hCodeId, wasteType.WasteCodes.Single(wc => wc.Type == CodeType.H).WasteCodeId);
        }

        [Theory]
        [InlineData(CodeType.CustomsCode)]
        [InlineData(CodeType.Basel)]
        [InlineData(CodeType.Oecd)]
        [InlineData(CodeType.Ewc)]
        [InlineData(CodeType.ExportCode)]
        [InlineData(CodeType.ImportCode)]
        [InlineData(CodeType.OtherCode)]
        [InlineData(CodeType.Un)]
        [InlineData(CodeType.UnNumber)]
        [InlineData(CodeType.Y)]
        public void AnyHCodesWrongType_Throws(CodeType codeType)
        {
            Assert.Throws<ArgumentException>(() => HCode.CreateFor(new[] { AnyWasteCode(codeType) }));
        }

        [Fact]
        public void UnClassesAreSet()
        {
            var unCodeId = new Guid("4B50E7AA-0136-444A-9185-FE31BE6CF98E");
            var unCode = new WasteCode(unCodeId, CodeType.Un);

            var wasteType = new WasteType(ImportNotificationId,
                "WateTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[] { AnyWasteCode(CodeType.Ewc) }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateFor(new[] { unCode }));

            Assert.Equal(unCodeId, wasteType.WasteCodes.Single(wc => wc.Type == CodeType.Un).WasteCodeId);
        }

        [Theory]
        [InlineData(CodeType.CustomsCode)]
        [InlineData(CodeType.Basel)]
        [InlineData(CodeType.Oecd)]
        [InlineData(CodeType.Ewc)]
        [InlineData(CodeType.ExportCode)]
        [InlineData(CodeType.H)]
        [InlineData(CodeType.ImportCode)]
        [InlineData(CodeType.OtherCode)]
        [InlineData(CodeType.UnNumber)]
        [InlineData(CodeType.Y)]
        public void AnyUnClassesWrongType_Throws(CodeType codeType)
        {
            Assert.Throws<ArgumentException>(() => UnClass.CreateFor(new[] { AnyWasteCode(codeType) }));
        }

        private WasteCode AnyWasteCode(CodeType codeType)
        {
            return new WasteCode(new Guid("2454B555-F9CF-43D7-B2CF-3230620E4C04"), codeType);
        }
    }
}