namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using WasteCodes;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Variables relate to waste codes")]
    public class WasteType : Entity
    {
        public Guid ImportNotificationId { get; private set; }

        public string Name { get; private set; }

        protected ICollection<WasteTypeWasteCode> WasteCodesCollection { get; private set; }

        public bool BaselOecdCodeNotListed { get; private set; }

        public bool YCodeNotApplicable { get; private set; }

        public bool HCodeNotApplicable { get; private set; }

        public bool UnClassNotApplicable { get; private set; }

        public IEnumerable<WasteTypeWasteCode> WasteCodes
        {
            get { return WasteCodesCollection.ToSafeIEnumerable(); }
        }

        protected WasteType()
        {
        }

        public WasteType(Guid importNotificationId,
            string name,
            BaselOecdCode baselOecdCode,
            EwcCode ewcCode,
            YCode yCode,
            HCode hCode,
            UnClass unClass)
        {
            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);
            Guard.ArgumentNotNullOrEmpty(() => name, name);
            Guard.ArgumentNotNull(() => baselOecdCode, baselOecdCode);
            Guard.ArgumentNotNull(() => ewcCode, ewcCode);
            Guard.ArgumentNotNull(() => yCode, yCode);
            Guard.ArgumentNotNull(() => hCode, hCode);
            Guard.ArgumentNotNull(() => unClass, unClass);

            Name = name;

            var wasteCodes = new List<WasteTypeWasteCode>();

            WasteCodesCollection = new List<WasteTypeWasteCode>();
            BaselOecdCodeNotListed = baselOecdCode.NotListed;
            YCodeNotApplicable = yCode.NotApplicable;
            HCodeNotApplicable = hCode.NotApplicable;
            UnClassNotApplicable = unClass.NotApplicable;

            wasteCodes.AddRange(ewcCode.Codes);

            if (!BaselOecdCodeNotListed)
            {
                wasteCodes.Add(baselOecdCode.Code);
            }

            if (!YCodeNotApplicable)
            {
                wasteCodes.AddRange(yCode.Codes);
            }

            if (!HCodeNotApplicable)
            {
                wasteCodes.AddRange(hCode.Codes);
            }

            if (!UnClassNotApplicable)
            {
                wasteCodes.AddRange(unClass.Codes);
            }

            WasteCodesCollection = wasteCodes;
        }
    }
}