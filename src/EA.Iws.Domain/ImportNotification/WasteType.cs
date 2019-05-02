﻿namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Core.WasteType;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using WasteCodes;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Variables relate to waste codes")]
    public class WasteType : Entity
    {
        public Guid ImportNotificationId { get; private set; }

        public string Name { get; private set; }

        protected virtual ICollection<WasteTypeWasteCode> WasteCodesCollection { get; set; }

        public bool BaselOecdCodeNotListed { get; private set; }

        public bool YCodeNotApplicable { get; private set; }

        public bool HCodeNotApplicable { get; private set; }

        public bool UnClassNotApplicable { get; private set; }

        public ChemicalComposition ChemicalCompositionType { get; private set; }

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
            UnClass unClass,
            ChemicalComposition chemicalComposition)
        {
            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);
            Guard.ArgumentNotNullOrEmpty(() => name, name);
            Guard.ArgumentNotNull(() => baselOecdCode, baselOecdCode);
            Guard.ArgumentNotNull(() => ewcCode, ewcCode);
            Guard.ArgumentNotNull(() => yCode, yCode);
            Guard.ArgumentNotNull(() => hCode, hCode);
            Guard.ArgumentNotNull(() => unClass, unClass);
            Guard.ArgumentNotDefaultValue(() => chemicalComposition, chemicalComposition);

            ImportNotificationId = importNotificationId;
            Name = name;

            var wasteCodes = new List<WasteTypeWasteCode>();

            WasteCodesCollection = new List<WasteTypeWasteCode>();
            BaselOecdCodeNotListed = baselOecdCode.NotListed;
            YCodeNotApplicable = yCode.NotApplicable;
            HCodeNotApplicable = hCode.NotApplicable;
            UnClassNotApplicable = unClass.NotApplicable;
            ChemicalCompositionType = chemicalComposition;

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

        public void Update(string name,
            BaselOecdCode baselOecdCode,
            EwcCode ewcCode,
            YCode yCode,
            HCode hCode,
            UnClass unClass)
        {
            Guard.ArgumentNotNullOrEmpty(() => name, name);
            Guard.ArgumentNotNull(() => baselOecdCode, baselOecdCode);
            Guard.ArgumentNotNull(() => ewcCode, ewcCode);
            Guard.ArgumentNotNull(() => yCode, yCode);
            Guard.ArgumentNotNull(() => hCode, hCode);
            Guard.ArgumentNotNull(() => unClass, unClass);

            Name = name;

            var wasteCodes = new List<WasteTypeWasteCode>();
            var existingCodes = WasteCodes.ToList();

            BaselOecdCodeNotListed = baselOecdCode.NotListed;
            YCodeNotApplicable = yCode.NotApplicable;
            HCodeNotApplicable = hCode.NotApplicable;
            UnClassNotApplicable = unClass.NotApplicable;

            foreach (var code in existingCodes)
            {
                WasteCodesCollection.Remove(code);
            }

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