﻿namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Notification;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableNotificationApplication : NotificationApplication
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<NotificationApplication>.SetProperty(x => x.Id, value, this); }
        }

        public new IList<WasteCodeInfo> WasteCodes
        {
            get { return base.WasteCodes.ToArray(); }
            set { WasteCodeInfoCollection = value; }
        }

        public new Guid UserId
        {
            get { return base.UserId; }
            set { ObjectInstantiator<NotificationApplication>.SetProperty(x => x.UserId, value, this); }
        }

        public new NotificationType NotificationType
        {
            get { return base.NotificationType; }
            set { ObjectInstantiator<NotificationApplication>.SetProperty(x => x.NotificationType, value, this); }
        }

        public new string NotificationNumber
        {
            get { return base.NotificationNumber; }
            set { ObjectInstantiator<NotificationApplication>.SetProperty(x => x.NotificationNumber, value, this); }
        }
        
        public new IEnumerable<PackagingInfo> PackagingInfos
        {
            get { return base.PackagingInfos; }
            set { PackagingInfosCollection = value.ToList(); }
        }

        public new IList<OperationInfo> OperationInfos
        {
            get { return base.OperationInfos.ToList(); }
            set { OperationInfosCollection = value; }
        }

        public new string ReasonForExport
        {
            get { return base.ReasonForExport; }
            set { ObjectInstantiator<NotificationApplication>.SetProperty(x => x.ReasonForExport, value, this); }
        }
        
        public new bool? HasSpecialHandlingRequirements
        {
            get { return base.HasSpecialHandlingRequirements; }
            set { ObjectInstantiator<NotificationApplication>.SetProperty(x => x.HasSpecialHandlingRequirements, value, this); }
        }

        public new IList<PhysicalCharacteristicsInfo> PhysicalCharacteristics
        {
            get { return base.PhysicalCharacteristics.ToList(); }
            set { PhysicalCharacteristicsCollection = value; }
        } 

        public new bool? WasteRecoveryInformationProvidedByImporter
        {
            get { return base.WasteRecoveryInformationProvidedByImporter; }
            set { ObjectInstantiator<NotificationApplication>.SetProperty(x => x.WasteRecoveryInformationProvidedByImporter, value, this); }
        }

        public new bool? IsWasteGenerationProcessAttached
        {
            get { return base.IsWasteGenerationProcessAttached; }
            set { ObjectInstantiator<NotificationApplication>.SetProperty(x => x.IsWasteGenerationProcessAttached, value, this); }
        }

        public new WasteType WasteType
        {
            get { return base.WasteType; }
            set { ObjectInstantiator<NotificationApplication>.SetProperty(x => x.WasteType, value, this); }
        }

        public new UKCompetentAuthority CompetentAuthority
        {
            get { return base.CompetentAuthority; }
            set { ObjectInstantiator<NotificationApplication>.SetProperty(x => x.CompetentAuthority, value, this); }
        }
    }
}
