﻿namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Core.WasteCodes;
    using Core.WasteType;
    using Domain.ImportNotification.WasteCodes;
    using Domain.NotificationApplication;
    using TestHelpers.Helpers;
    using Xunit;
    using WasteType = Domain.ImportNotification.WasteType;

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
                UnClass.CreateFor(new[] { AnyWasteCode(CodeType.Un) }),
                ChemicalComposition.Other, WasteCategoryType.Batteries);

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
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries);

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
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries);

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
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries);

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
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries);

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
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries));
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
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries));
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
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries));
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
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries));
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
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries));
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
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries));
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
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries));
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
                null,
                ChemicalComposition.Other, WasteCategoryType.Batteries));
        }

        [Fact]
        public void BaselCodeIsSet()
        {
            var baselCode = AnyWasteCode(CodeType.Basel);
            var baselCodeId = new Guid("72669DEE-1126-47B3-8C45-2395F54C82D8");
            ObjectInstantiator<WasteCode>.SetProperty(x => x.Id, baselCodeId, baselCode);

            var wasteType = new WasteType(ImportNotificationId,
                "WasteTypeName",
                BaselOecdCode.CreateFor(baselCode),
                EwcCode.CreateFor(new[] { AnyWasteCode(CodeType.Ewc) }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries);

            Assert.Equal(1, wasteType.WasteCodes.Count(wc => wc.WasteCodeId == baselCodeId));
        }

        [Fact]
        public void OecdCodeIsSet()
        {
            var oecdCode = AnyWasteCode(CodeType.Oecd);
            var oecdCodeId = new Guid("A58E7CE8-FA46-4E35-A3ED-26E395678CA0");
            ObjectInstantiator<WasteCode>.SetProperty(x => x.Id, oecdCodeId, oecdCode);

            var wasteType = new WasteType(ImportNotificationId,
                "WasteTypeName",
                BaselOecdCode.CreateFor(oecdCode),
                EwcCode.CreateFor(new[] { AnyWasteCode(CodeType.Ewc) }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries);

            Assert.Equal(1, wasteType.WasteCodes.Count(wc => wc.WasteCodeId == oecdCodeId));
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
            var ewcCode = AnyWasteCode(CodeType.Ewc);
            var ewcCodeId = new Guid("A4DA3323-B6BD-4245-B144-B9E05661BEC6");
            ObjectInstantiator<WasteCode>.SetProperty(x => x.Id, ewcCodeId, ewcCode);

            var wasteType = new WasteType(ImportNotificationId,
                "WateTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[] { ewcCode }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries);

            Assert.Equal(1, wasteType.WasteCodes.Count(wc => wc.WasteCodeId == ewcCodeId));
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
            var yCode = AnyWasteCode(CodeType.Y);
            var yCodeId = new Guid("F416BD1D-56E9-45D8-9A4E-7476DF201AB2");
            ObjectInstantiator<WasteCode>.SetProperty(x => x.Id, yCodeId, yCode);

            var wasteType = new WasteType(ImportNotificationId,
                "WateTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[] { AnyWasteCode(CodeType.Ewc) }),
                YCode.CreateFor(new[] { yCode }),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries);

            Assert.Equal(1, wasteType.WasteCodes.Count(wc => wc.WasteCodeId == yCodeId));
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
            var hCode = AnyWasteCode(CodeType.H);
            var hCodeId = new Guid("B21D8610-EBB0-436F-BD26-D2F40B42B866");
            ObjectInstantiator<WasteCode>.SetProperty(x => x.Id, hCodeId, hCode);

            var wasteType = new WasteType(ImportNotificationId,
                "WateTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[] { AnyWasteCode(CodeType.Ewc) }),
                YCode.CreateNotApplicable(),
                HCode.CreateFor(new[] { hCode }),
                UnClass.CreateNotApplicable(),
                ChemicalComposition.Other, WasteCategoryType.Batteries);

            Assert.Equal(1, wasteType.WasteCodes.Count(wc => wc.WasteCodeId == hCodeId));
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
            var unCode = AnyWasteCode(CodeType.Un);
            var unCodeId = new Guid("4B50E7AA-0136-444A-9185-FE31BE6CF98E");
            ObjectInstantiator<WasteCode>.SetProperty(x => x.Id, unCodeId, unCode);

            var wasteType = new WasteType(ImportNotificationId,
                "WateTypeName",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[] { AnyWasteCode(CodeType.Ewc) }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateFor(new[] { unCode }),
                ChemicalComposition.Other, WasteCategoryType.Batteries);

            Assert.Equal(1, wasteType.WasteCodes.Count(wc => wc.WasteCodeId == unCodeId));
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

        [Fact]
        public void UnsetChemicalComposition_Throws()
        {
            Assert.Throws<ArgumentException>(() => new WasteType(ImportNotificationId,
                "Name",
                BaselOecdCode.CreateNotListed(),
                EwcCode.CreateFor(new[]
                {
                    AnyWasteCode(CodeType.Ewc)
                }),
                YCode.CreateNotApplicable(),
                HCode.CreateNotApplicable(),
                UnClass.CreateNotApplicable(),
                default(ChemicalComposition), WasteCategoryType.Batteries));
        }

        private WasteCode AnyWasteCode(CodeType codeType)
        {
            var wasteCode = ObjectInstantiator<WasteCode>.CreateNew();

            ObjectInstantiator<WasteCode>.SetProperty(x => x.Id, new Guid("0C4D98DA-91F3-4F5D-922B-C7A6BE8B5008"), wasteCode);
            ObjectInstantiator<WasteCode>.SetProperty(x => x.Code, "WasteCode", wasteCode);
            ObjectInstantiator<WasteCode>.SetProperty(x => x.CodeType, codeType, wasteCode);
            ObjectInstantiator<WasteCode>.SetProperty(x => x.Description, "Description", wasteCode);

            return wasteCode;
        }
    }
}