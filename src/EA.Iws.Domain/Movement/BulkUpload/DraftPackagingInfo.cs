namespace EA.Iws.Domain.Movement.BulkUpload
{
    using System;
    using Core.PackagingType;
    using Prsd.Core.Domain;

    public class DraftPackagingInfo : Entity
    {
        public PackagingType PackagingType { get; set; }

        public string OtherDescription { get; set; }
    }
}
