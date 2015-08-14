namespace EA.Iws.Core.WasteCodes
{
    using System;

    public class WasteCodeData
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public CodeType CodeType { get; set; }

        public string CustomCode { get; set; }

        public bool IsNotApplicable { get; set; }
    }
}