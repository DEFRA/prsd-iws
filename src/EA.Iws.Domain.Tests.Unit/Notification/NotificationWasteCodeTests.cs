namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Core.WasteCodes;
    using Core.WasteType;
    using Domain.Notification;
    using TestHelpers.Helpers;
    using Xunit;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation",
        Justification = "Variables relate to Y/H/UN-Codes")]
    public class NotificationWasteCodeTests
    {
        private readonly NotificationApplication notification;

        public NotificationWasteCodeTests()
        {
            notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
        }

        [Fact]
        public void CanAddOecdCode()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Oecd);

            notification.SetBaselOecdCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            Assert.NotNull(notification.BaselOecdCode);
            Assert.Equal(CodeType.Oecd, notification.BaselOecdCode.WasteCode.CodeType);
        }

        [Fact]
        public void CanAddBaselCode()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Basel);

            notification.SetBaselOecdCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            Assert.NotNull(notification.BaselOecdCode);
            Assert.Equal(CodeType.Basel, notification.BaselOecdCode.WasteCode.CodeType);
        }

        [Fact]
        public void SetBaselOecdCode_NotBaselOrOecd_ThrowsException()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Ewc);

            Action setBaselOecdCode = () => notification.SetBaselOecdCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            Assert.Throws<InvalidOperationException>(setBaselOecdCode);
        }

        [Fact]
        public void SetBaselOecdCode_ReplacesExisting()
        {
            var baselCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Basel);
            var oecdCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Oecd);

            notification.SetBaselOecdCode(WasteCodeInfo.CreateWasteCodeInfo(baselCode));
            notification.SetBaselOecdCode(WasteCodeInfo.CreateWasteCodeInfo(oecdCode));

            Assert.NotNull(notification.BaselOecdCode);
            Assert.Equal(CodeType.Oecd, notification.BaselOecdCode.WasteCode.CodeType);
        }

        [Fact]
        public void BaselOecdCode_ReturnsNullWhenNotSet()
        {
            Assert.Null(notification.BaselOecdCode);
        }

        [Fact]
        public void EwcCodes_ReturnsEmptyIEnumerableWhenNotSet()
        {
            Assert.Empty(notification.EwcCodes);
        }

        [Fact]
        public void CanSetEwcCodes()
        {
            var ewcCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.Ewc)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.Ewc)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.Ewc))
            };
            notification.SetEwcCodes(ewcCodes);

            Assert.Equal(3, notification.EwcCodes.Count());
        }

        [Fact]
        public void SetEwcCodes_ReplacesExisting()
        {
            var ewcCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.Ewc)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.Ewc)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.Ewc))
            };
            notification.SetEwcCodes(ewcCodes);

            var newEwcCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.Ewc)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("369E9D15-828F-4C8D-AAF9-C2F3E4F4765E"),
                    CodeType.Ewc))
            };
            notification.SetEwcCodes(newEwcCodes);

            Assert.Collection(notification.EwcCodes,
                item =>
                    Assert.True(
                        notification.EwcCodes.Any(
                            p => p.WasteCode.Id == new Guid("521154F9-A265-472A-9275-8871F0DACA9F"))),
                item =>
                    Assert.True(
                        notification.EwcCodes.Any(
                            p => p.WasteCode.Id == new Guid("369E9D15-828F-4C8D-AAF9-C2F3E4F4765E"))));
        }

        [Fact]
        public void SetEwcCodes_NotAllEwcCodes_ThrowsException()
        {
            var ewcCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.H)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.Oecd)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.Ewc))
            };
            Action setEwcCodes = () => notification.SetEwcCodes(ewcCodes);

            Assert.Throws<InvalidOperationException>(setEwcCodes);
        }

        [Fact]
        public void SetEwcCodse_DuplicateCodes_ThrowsException()
        {
            var ewcCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.Ewc)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.Ewc))
            };

            Action setEwcCodes = () => notification.SetEwcCodes(ewcCodes);

            Assert.Throws<InvalidOperationException>(setEwcCodes);
        }

        [Fact]
        public void YCodes_ReturnsEmptyIEnumerableWhenNotSet()
        {
            Assert.Empty(notification.YCodes);
        }

        [Fact]
        public void CanSetYCodes()
        {
            var yCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.Y)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.Y)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.Y))
            };
            notification.SetYCodes(yCodes);

            Assert.Equal(3, notification.YCodes.Count());
        }

        [Fact]
        public void SetYCodes_ReplacesExisting()
        {
            var yCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.Y)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.Y)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.Y))
            };
            notification.SetYCodes(yCodes);

            var newYCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.Y)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("369E9D15-828F-4C8D-AAF9-C2F3E4F4765E"),
                    CodeType.Y))
            };
            notification.SetYCodes(newYCodes);

            Assert.Collection(notification.YCodes,
                item =>
                    Assert.True(
                        notification.YCodes.Any(p => p.WasteCode.Id == new Guid("521154F9-A265-472A-9275-8871F0DACA9F"))),
                item =>
                    Assert.True(
                        notification.YCodes.Any(p => p.WasteCode.Id == new Guid("369E9D15-828F-4C8D-AAF9-C2F3E4F4765E"))));
        }

        [Fact]
        public void SetYCodes_NotAllYCodes_ThrowsException()
        {
            var yCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.H)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.Oecd)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.Y))
            };
            Action setYCodes = () => notification.SetYCodes(yCodes);

            Assert.Throws<InvalidOperationException>(setYCodes);
        }

        [Fact]
        public void SetYCodse_DuplicateCodes_ThrowsException()
        {
            var yCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.Y)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.Y))
            };

            Action setYCodes = () => notification.SetYCodes(yCodes);

            Assert.Throws<InvalidOperationException>(setYCodes);
        }

        [Fact]
        public void HCodes_ReturnsEmptyIEnumerableWhenNotSet()
        {
            Assert.Empty(notification.HCodes);
        }

        [Fact]
        public void CanSetHCodes()
        {
            var hCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.H)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.H)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.H))
            };
            notification.SetHCodes(hCodes);

            Assert.Equal(3, notification.HCodes.Count());
        }

        [Fact]
        public void SetHCodes_ReplacesExisting()
        {
            var hCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.H)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.H)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.H))
            };
            notification.SetHCodes(hCodes);

            var newHCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.H)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("369E9D15-828F-4C8D-AAF9-C2F3E4F4765E"),
                    CodeType.H))
            };
            notification.SetHCodes(newHCodes);

            Assert.Collection(notification.HCodes,
                item =>
                    Assert.True(
                        notification.HCodes.Any(p => p.WasteCode.Id == new Guid("521154F9-A265-472A-9275-8871F0DACA9F"))),
                item =>
                    Assert.True(
                        notification.HCodes.Any(p => p.WasteCode.Id == new Guid("369E9D15-828F-4C8D-AAF9-C2F3E4F4765E"))));
        }

        [Fact]
        public void SetHCodes_NotAllHCodes_ThrowsException()
        {
            var hCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.H)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.Oecd)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.H))
            };
            Action setHCodes = () => notification.SetHCodes(hCodes);

            Assert.Throws<InvalidOperationException>(setHCodes);
        }

        [Fact]
        public void SetHCodse_DuplicateCodes_ThrowsException()
        {
            var hCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.H)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.H))
            };

            Action setHCodes = () => notification.SetHCodes(hCodes);

            Assert.Throws<InvalidOperationException>(setHCodes);
        }

        [Fact]
        public void UnCodes_ReturnsEmptyIEnumerableWhenNotSet()
        {
            Assert.Empty(notification.UnClasses);
        }

        [Fact]
        public void CanSetUnCodes()
        {
            var unCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.Un)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.Un)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.Un))
            };
            notification.SetUnClasses(unCodes);

            Assert.Equal(3, notification.UnClasses.Count());
        }

        [Fact]
        public void SetUnCodes_ReplacesExisting()
        {
            var unCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.Un)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.Un)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.Un))
            };
            notification.SetUnClasses(unCodes);

            var newUnCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.Un)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("369E9D15-828F-4C8D-AAF9-C2F3E4F4765E"),
                    CodeType.Un))
            };
            notification.SetUnClasses(newUnCodes);

            Assert.Collection(notification.UnClasses,
                item =>
                    Assert.True(
                        notification.UnClasses.Any(p => p.WasteCode.Id == new Guid("521154F9-A265-472A-9275-8871F0DACA9F"))),
                item =>
                    Assert.True(
                        notification.UnClasses.Any(p => p.WasteCode.Id == new Guid("369E9D15-828F-4C8D-AAF9-C2F3E4F4765E"))));
        }

        [Fact]
        public void SetUnCodes_NotAllUnCodes_ThrowsException()
        {
            var unCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.H)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.Oecd)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.Un))
            };
            Action setUnCodes = () => notification.SetUnClasses(unCodes);

            Assert.Throws<InvalidOperationException>(setUnCodes);
        }

        [Fact]
        public void SetUnCodse_DuplicateCodes_ThrowsException()
        {
            var unCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.Un)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.Un))
            };

            Action setUnCodes = () => notification.SetUnClasses(unCodes);

            Assert.Throws<InvalidOperationException>(setUnCodes);
        }

        [Fact]
        public void UnNumberCodes_ReturnsEmptyIEnumerableWhenNotSet()
        {
            Assert.Empty(notification.UnNumbers);
        }

        [Fact]
        public void CanSetUnNumberCodes()
        {
            var unNumberCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.UnNumber)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.UnNumber)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.UnNumber))
            };
            notification.SetUnNumbers(unNumberCodes);

            Assert.Equal(3, notification.UnNumbers.Count());
        }

        [Fact]
        public void SetUnNumberCodes_ReplacesExisting()
        {
            var unNumberCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.UnNumber)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.UnNumber)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.UnNumber))
            };
            notification.SetUnNumbers(unNumberCodes);

            var newUnNumberCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.UnNumber)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("369E9D15-828F-4C8D-AAF9-C2F3E4F4765E"),
                    CodeType.UnNumber))
            };
            notification.SetUnNumbers(newUnNumberCodes);

            Assert.Collection(notification.UnNumbers,
                item =>
                    Assert.True(
                        notification.UnNumbers.Any(
                            p => p.WasteCode.Id == new Guid("521154F9-A265-472A-9275-8871F0DACA9F"))),
                item =>
                    Assert.True(
                        notification.UnNumbers.Any(
                            p => p.WasteCode.Id == new Guid("369E9D15-828F-4C8D-AAF9-C2F3E4F4765E"))));
        }

        [Fact]
        public void SetUnNumberCodes_NotAllUnNumberCodes_ThrowsException()
        {
            var unNumberCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.H)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.Oecd)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.UnNumber))
            };
            Action setUnNumberCodes = () => notification.SetUnNumbers(unNumberCodes);

            Assert.Throws<InvalidOperationException>(setUnNumberCodes);
        }

        [Fact]
        public void SetUnNumberCodse_DuplicateCodes_ThrowsException()
        {
            var unNumberCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.UnNumber)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.UnNumber))
            };

            Action setUnNumberCodes = () => notification.SetUnNumbers(unNumberCodes);

            Assert.Throws<InvalidOperationException>(setUnNumberCodes);
        }

        [Fact]
        public void CreateWasteCode_AsOptionalCode_ThrowsException()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Basel);

            Action createWasteCode =
                () =>
                    WasteCodeInfo.CreateCustomWasteCodeInfo(wasteCode, "optional code");

            Assert.Throws<InvalidOperationException>(createWasteCode);
        }

        [Fact]
        public void CreateOptionalWasteCode_WithWrongWasteType_ThrowsException()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Ewc);

            Action createWasteCode =
                () =>
                    WasteCodeInfo.CreateCustomWasteCodeInfo(wasteCode, "optional code");

            Assert.Throws<InvalidOperationException>(createWasteCode);
        }

        [Fact]
        public void CreateCustomWasteCode_CustomCodeIsSet()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.OtherCode);
            var wasteCodeInfo = WasteCodeInfo.CreateCustomWasteCodeInfo(wasteCode, "code");

            Assert.Equal("code", wasteCodeInfo.CustomCode);
        }

        [Fact]
        public void CanAddExportCode()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.ExportCode);

            notification.SetExportCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            Assert.NotNull(notification.ExportCode);
            Assert.Equal(CodeType.ExportCode, notification.ExportCode.WasteCode.CodeType);
        }

        [Fact]
        public void SetExportCode_NotExport_ThrowsException()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Ewc);

            Action setExportCode = () => notification.SetExportCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            Assert.Throws<InvalidOperationException>(setExportCode);
        }

        [Fact]
        public void SetExportCode_ReplacesExisting()
        {
            var exportCode = GetTestWasteCode(Guid.NewGuid(), CodeType.ExportCode);
            var newExportCode = GetTestWasteCode(Guid.NewGuid(), CodeType.ExportCode);

            notification.SetExportCode(WasteCodeInfo.CreateWasteCodeInfo(exportCode));
            notification.SetExportCode(WasteCodeInfo.CreateWasteCodeInfo(newExportCode));

            Assert.NotNull(notification.ExportCode);
            Assert.Equal(CodeType.ExportCode, notification.ExportCode.WasteCode.CodeType);
        }

        [Fact]
        public void ExportCode_ReturnsNullWhenNotSet()
        {
            Assert.Null(notification.ExportCode);
        }

        [Fact]
        public void CanAddImportCode()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.ImportCode);

            notification.SetImportCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            Assert.NotNull(notification.ImportCode);
            Assert.Equal(CodeType.ImportCode, notification.ImportCode.WasteCode.CodeType);
        }

        [Fact]
        public void SetImportCode_NotImport_ThrowsException()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Ewc);

            Action setImportCode = () => notification.SetImportCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            Assert.Throws<InvalidOperationException>(setImportCode);
        }

        [Fact]
        public void SetImportCode_ReplacesExisting()
        {
            var importCode = GetTestWasteCode(Guid.NewGuid(), CodeType.ImportCode);
            var newImportCode = GetTestWasteCode(Guid.NewGuid(), CodeType.ImportCode);

            notification.SetImportCode(WasteCodeInfo.CreateWasteCodeInfo(importCode));
            notification.SetImportCode(WasteCodeInfo.CreateWasteCodeInfo(newImportCode));

            Assert.NotNull(notification.ImportCode);
            Assert.Equal(CodeType.ImportCode, notification.ImportCode.WasteCode.CodeType);
        }

        [Fact]
        public void ImportCode_ReturnsNullWhenNotSet()
        {
            Assert.Null(notification.ImportCode);
        }

        [Fact]
        public void CanAddOtherCode()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.OtherCode);

            notification.SetOtherCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            Assert.NotNull(notification.OtherCode);
            Assert.Equal(CodeType.OtherCode, notification.OtherCode.WasteCode.CodeType);
        }

        [Fact]
        public void SetOtherCode_NotOther_ThrowsException()
        {
            var wasteCode = GetTestWasteCode(Guid.NewGuid(), CodeType.Ewc);

            Action setOtherCode = () => notification.SetOtherCode(WasteCodeInfo.CreateWasteCodeInfo(wasteCode));

            Assert.Throws<InvalidOperationException>(setOtherCode);
        }

        [Fact]
        public void SetOtherCode_ReplacesExisting()
        {
            var otherCode = GetTestWasteCode(Guid.NewGuid(), CodeType.OtherCode);
            var newOtherCode = GetTestWasteCode(Guid.NewGuid(), CodeType.OtherCode);

            notification.SetOtherCode(WasteCodeInfo.CreateWasteCodeInfo(otherCode));
            notification.SetOtherCode(WasteCodeInfo.CreateWasteCodeInfo(newOtherCode));

            Assert.NotNull(notification.OtherCode);
            Assert.Equal(CodeType.OtherCode, notification.OtherCode.WasteCode.CodeType);
        }

        [Fact]
        public void OtherCode_ReturnsNullWhenNotSet()
        {
            Assert.Null(notification.OtherCode);
        }

        [Fact]
        public void CustomsCodes_ReturnsEmptyIEnumerableWhenNotSet()
        {
            Assert.Empty(notification.CustomsCodes);
        }

        [Fact]
        public void CanSetCustomsCodes()
        {
            var customsCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.CustomsCode)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.CustomsCode)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.CustomsCode))
            };
            notification.SetCustomsCodes(customsCodes);

            Assert.Equal(3, notification.CustomsCodes.Count());
        }

        [Fact]
        public void SetCustomsCodes_ReplacesExisting()
        {
            var customsCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.CustomsCode)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.CustomsCode)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.CustomsCode))
            };
            notification.SetCustomsCodes(customsCodes);

            var newCustomsCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.CustomsCode)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("369E9D15-828F-4C8D-AAF9-C2F3E4F4765E"),
                    CodeType.CustomsCode))
            };
            notification.SetCustomsCodes(newCustomsCodes);

            Assert.Collection(notification.CustomsCodes,
                item =>
                    Assert.True(
                        notification.CustomsCodes.Any(
                            p => p.WasteCode.Id == new Guid("521154F9-A265-472A-9275-8871F0DACA9F"))),
                item =>
                    Assert.True(
                        notification.CustomsCodes.Any(
                            p => p.WasteCode.Id == new Guid("369E9D15-828F-4C8D-AAF9-C2F3E4F4765E"))));
        }

        [Fact]
        public void SetCustomsCodes_NotAllCustomsCodes_ThrowsException()
        {
            var customsCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E5B35439-AC37-479A-9992-627C453299B9"),
                    CodeType.H)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("209DAE97-84B8-4478-89EF-E57B3EA2AA70"),
                    CodeType.Oecd)),
                WasteCodeInfo.CreateWasteCodeInfo(GetTestWasteCode(new Guid("E1B62673-35C2-4120-87C4-F6986C8C1E2F"),
                    CodeType.CustomsCode))
            };
            Action setCustomsCodes = () => notification.SetCustomsCodes(customsCodes);

            Assert.Throws<InvalidOperationException>(setCustomsCodes);
        }

        [Fact]
        public void CanEnterMultipleCustomsCodes()
        {
            var customsCodes = new List<WasteCodeInfo>
            {
                WasteCodeInfo.CreateCustomWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.CustomsCode), "code1"),
                WasteCodeInfo.CreateCustomWasteCodeInfo(GetTestWasteCode(new Guid("521154F9-A265-472A-9275-8871F0DACA9F"),
                    CodeType.CustomsCode), "code2")
            };

            notification.SetCustomsCodes(customsCodes);

            Assert.Collection(notification.CustomsCodes,
                item =>
                    Assert.True(
                        notification.CustomsCodes.Any(
                            p => p.WasteCode.Id == new Guid("521154F9-A265-472A-9275-8871F0DACA9F") && p.CustomCode == "code1")),
                item =>
                    Assert.True(
                        notification.CustomsCodes.Any(
                            p => p.WasteCode.Id == new Guid("521154F9-A265-472A-9275-8871F0DACA9F") && p.CustomCode == "code2")));
        }

        private static WasteCode GetTestWasteCode(Guid id, CodeType codeType)
        {
            var wasteCode = ObjectInstantiator<WasteCode>.CreateNew();
            ObjectInstantiator<WasteCode>.SetProperty(x => x.Id, id, wasteCode);
            ObjectInstantiator<WasteCode>.SetProperty(x => x.CodeType, codeType, wasteCode);
            ObjectInstantiator<WasteCode>.SetProperty(x => x.Code, id.ToString(), wasteCode);
            ObjectInstantiator<WasteCode>.SetProperty(x => x.Description, id.ToString(), wasteCode);
            return wasteCode;
        }
    }
}