namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Linq;
    using Core.CustomsOffice;
    using TransportRoute;

    public partial class NotificationApplication
    {
        public CustomsOffices GetCustomsOfficesRequired()
        {
            if (this.StateOfExport == null || this.StateOfImport == null)
            {
                return CustomsOffices.TransitStatesNotSet;
            }

            bool isStartPointEU = this.StateOfExport.Country.IsEuropeanUnionMember;
            bool isEndPointEU = this.StateOfImport.Country.IsEuropeanUnionMember;
            bool areAllTransitStatesEU = isStartPointEU && isEndPointEU;

            if (this.TransitStates != null)
            {
                areAllTransitStatesEU = this.TransitStates.All(ts => ts.Country.IsEuropeanUnionMember);
            }

            if (isEndPointEU)
            {
                return areAllTransitStatesEU ? CustomsOffices.None : CustomsOffices.EntryAndExit;
            }
            return CustomsOffices.Exit;
        }

        public CustomsOffices GetCustomsOfficesCompleted()
        {
            bool exitOfficeSet = ExitCustomsOffice != null;
            bool entryOfficeSet = EntryCustomsOffice != null;

            if (exitOfficeSet && entryOfficeSet)
            {
                return CustomsOffices.EntryAndExit;
            }
            
            if (!entryOfficeSet && !exitOfficeSet)
            {
                return CustomsOffices.None;
            }

            if (entryOfficeSet)
            {
                return CustomsOffices.Entry;
            }

            return CustomsOffices.Exit;
        }

        public void SetExitCustomsOffice(ExitCustomsOffice customsOffice)
        {
            var customsOfficeRequiredStatus = GetCustomsOfficesRequired();

            switch (customsOfficeRequiredStatus)
            {
                case CustomsOffices.EntryAndExit:
                case CustomsOffices.Exit:
                    this.ExitCustomsOffice = customsOffice;
                    break;
                default:
                    throw new InvalidOperationException("Cannot set an exit customs office for Notification " + this.Id
                        + ". The Notification only requires the following customs offices: " + customsOfficeRequiredStatus);
            }
        }

        public void SetEntryCustomsOffice(EntryCustomsOffice customsOffice)
        {
            var customsOfficeRequiredStatus = GetCustomsOfficesRequired();

            switch (customsOfficeRequiredStatus)
            {
                case CustomsOffices.EntryAndExit:
                case CustomsOffices.Entry:
                    this.EntryCustomsOffice = customsOffice;
                    break;
                default:
                    throw new InvalidOperationException("Cannot set an entry customs office for Notification " + this.Id
                        + ". The Notification only requires the following customs offices: " + customsOfficeRequiredStatus);
            }
        }
    }
}
