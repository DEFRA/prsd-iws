namespace EA.Iws.RequestHandlers.Copy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.NotificationApplication;
    using Prsd.Core;

    internal class WasteCodeCopy
    {
        public void CopyWasteCodes(NotificationApplication source, NotificationApplication destination)
        {
            var clone = new NotificationApplicationClone(source, destination);

            CopyBaselOecdCode(clone);
            CopyCustomsCode(clone);
            CopyExportCode(clone);
            CopyImportCode(clone);
            CopyOtherCode(clone);

            CopyEwcCodes(clone);
            CopyUnClasses(clone);
            CopyUnNumbers(clone);
            CopyYCodes(clone);
            CopyHCodes(clone);
        }

        private void CopySingleCodeOperation(NotificationApplicationClone clone,
            Func<NotificationApplication, WasteCodeInfo> codeGetter,
            Action<NotificationApplication, WasteCodeInfo> codeSetMethod)
        {
            if (codeGetter(clone.Source) == null)
            {
                return;
            }

            codeSetMethod(clone.Destination, CreateCodeInfoCopy(codeGetter(clone.Source)));
        }

        private void CopyCodesListOperation(NotificationApplicationClone clone,
            Func<NotificationApplication, IEnumerable<WasteCodeInfo>> codesGetter,
            Action<NotificationApplication, IEnumerable<WasteCodeInfo>> codesSetter)
        {
            if (codesGetter(clone.Source) == null 
                || !codesGetter(clone.Source).Any())
            {
                return;
            }

            codesSetter(clone.Destination, codesGetter(clone.Source).Select(CreateCodeInfoCopy));
        }

        private void CopyBaselOecdCode(NotificationApplicationClone clone)
        {
            CopySingleCodeOperation(clone,
                na => na.BaselOecdCode,
                (na, wc) => na.SetBaselOecdCode(wc));
        }

        private void CopyExportCode(NotificationApplicationClone clone)
        {
            CopySingleCodeOperation(clone,
                na => na.ExportCode,
                (na, wc) => na.SetExportCode(wc));
        }

        private void CopyImportCode(NotificationApplicationClone clone)
        {
            CopySingleCodeOperation(clone,
                na => na.ImportCode,
                (na, wc) => na.SetImportCode(wc));
        }

        private void CopyCustomsCode(NotificationApplicationClone clone)
        {
            CopySingleCodeOperation(clone,
                na => na.CustomsCode,
                (na, wc) => na.SetCustomsCode(wc));
        }

        private void CopyOtherCode(NotificationApplicationClone clone)
        {
            CopySingleCodeOperation(clone,
                na => na.OtherCode,
                (na, wc) => na.SetOtherCode(wc));
        }

        private void CopyEwcCodes(NotificationApplicationClone clone)
        {
            CopyCodesListOperation(clone,
                na => na.EwcCodes,
                (na, wcs) => na.SetEwcCodes(wcs));
        }

        private void CopyUnClasses(NotificationApplicationClone clone)
        {
            CopyCodesListOperation(clone,
                na => na.UnClasses,
                (na, wcs) => na.SetUnClasses(wcs));
        }

        private void CopyUnNumbers(NotificationApplicationClone clone)
        {
            CopyCodesListOperation(clone,
                na => na.UnNumbers,
                (na, wcs) => na.SetUnNumbers(wcs));
        }

        private void CopyYCodes(NotificationApplicationClone clone)
        {
            CopyCodesListOperation(clone,
                na => na.YCodes,
                (na, wcs) => na.SetYCodes(wcs));
        }

        private void CopyHCodes(NotificationApplicationClone clone)
        {
            CopyCodesListOperation(clone,
                na => na.HCodes,
                (na, wcs) => na.SetHCodes(wcs));
        }

        public WasteCodeInfo CreateCodeInfoCopy(WasteCodeInfo codeInfo)
        {
            Guard.ArgumentNotNull(() => codeInfo, codeInfo);

            if (!string.IsNullOrWhiteSpace(codeInfo.CustomCode))
            {
                return WasteCodeInfo.CreateCustomWasteCodeInfo(codeInfo.CodeType, codeInfo.CustomCode);
            }

            if (codeInfo.IsNotApplicable)
            {
                return WasteCodeInfo.CreateNotApplicableCodeInfo(codeInfo.CodeType);
            }

            return WasteCodeInfo.CreateWasteCodeInfo(codeInfo.WasteCode);
        }

        private class NotificationApplicationClone
        {
            public NotificationApplication Source { get; private set; }
            public NotificationApplication Destination { get; private set; }

            public NotificationApplicationClone(NotificationApplication source, NotificationApplication destination)
            {
                Source = source;
                Destination = destination;
            }
        }
    }
}
