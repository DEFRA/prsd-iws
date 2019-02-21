namespace EA.Iws.RequestHandlers.AddressBook
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.AddressBook;
    using Core.ImportNotification.Draft;
    using DataAccess;
    using DataAccess.Draft;
    using Domain.AddressBook;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;
    using FinalAddress = Domain.NotificationApplication.Address;
    using FinalBusiness = Domain.NotificationApplication.Business;
    using FinalContact = Domain.NotificationApplication.Contact;

    internal class AddImportAddressBookEntryHandler : IRequestHandler<AddImportAddressBookEntry, bool>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly IAddressBookRepository addressBookRepository;
        private readonly IDraftImportNotificationRepository draftImportNotificationRepository;
        private const string notApplicable = "Not applicable";

        public AddImportAddressBookEntryHandler(IwsContext context, IUserContext userContext, IAddressBookRepository addressBookRepository, IDraftImportNotificationRepository draftImportNotificationRepository)
        {
            this.context = context;
            this.userContext = userContext;
            this.addressBookRepository = addressBookRepository;
            this.draftImportNotificationRepository = draftImportNotificationRepository;
        }

        public async Task<bool> HandleAsync(AddImportAddressBookEntry message)
        {
            var draft = await draftImportNotificationRepository.Get(message.ImportNotificationId);

            var types = Enum.GetValues(typeof(AddressRecordType)).Cast<AddressRecordType>();

            foreach (var type in types)
            {
                await AddToAddressBookForType(type, draft);
            }

            return true;
        }

        private async Task AddToAddressBookForType(AddressRecordType type, ImportNotification draft)
        {
            AddressBook addressBook = await addressBookRepository.GetAddressBookForUser(userContext.UserId, type);

            switch (type)
            {
                case AddressRecordType.Facility:
                    foreach (var facility in draft.Facilities.Facilities.Where(p => p.IsAddedToAddressBook))
                    {
                        var facilityAddress = await FacilityAddressBookRecord(facility);
                        addressBook.Add(facilityAddress);
                    }
                    if (draft.Importer.IsAddedToAddressBook)
                    {
                        var importerAddress = await ImporterAddressBookRecord(draft.Importer);
                        addressBook.Add(importerAddress);
                    }
                    break;
                case AddressRecordType.Producer:
                    if (draft.Exporter.IsAddedToAddressBook)
                    {
                        var exporter = await ExporterAddressBookRecord(draft.Exporter);
                        addressBook.Add(exporter);
                    }

                    if (draft.Producer.IsAddedToAddressBook)
                    {
                        var producer = await ProducerAddressBookRecord(draft.Producer);
                        addressBook.Add(producer);
                    }
                    
                    break;
                case AddressRecordType.Carrier:
                default:
                    break;
            }
            await addressBookRepository.Update(addressBook);
        }

        private async Task<AddressBookRecord> ProducerAddressBookRecord(Producer producer)
        {
            var address = await GetAddress(producer.Address);
            var contact = GetContact(producer.Contact);

            FinalBusiness business = FinalBusiness.CreateOtherBusiness(producer.BusinessName, notApplicable, notApplicable, notApplicable);

            return new AddressBookRecord(address, business, contact);
        }

        private async Task<AddressBookRecord> ExporterAddressBookRecord(Exporter exporter)
        {
            var address = await GetAddress(exporter.Address);
            var contact = GetContact(exporter.Contact);

            FinalBusiness business = FinalBusiness.CreateOtherBusiness(exporter.BusinessName, notApplicable, notApplicable, notApplicable);
            return new AddressBookRecord(address, business, contact);
        }

        private async Task<AddressBookRecord> ImporterAddressBookRecord(Importer importer)
        {
            var address = await GetAddress(importer.Address);
            var contact = GetContact(importer.Contact);

            importer.RegistrationNumber = importer.RegistrationNumber == null ? notApplicable : importer.RegistrationNumber;

            FinalBusiness business = FinalBusiness.CreateBusiness(importer.BusinessName, importer.Type.GetValueOrDefault(), importer.RegistrationNumber, notApplicable);
            return new AddressBookRecord(address, business, contact);
        }

        private async Task<AddressBookRecord> FacilityAddressBookRecord(Facility facility)
        {
            var address = await GetAddress(facility.Address);
            var contact = GetContact(facility.Contact);

            facility.RegistrationNumber = facility.RegistrationNumber == null ? notApplicable : facility.RegistrationNumber;

            FinalBusiness business = FinalBusiness.CreateBusiness(facility.BusinessName, facility.Type.GetValueOrDefault(), facility.RegistrationNumber, string.Empty);
            return new AddressBookRecord(address, business, contact);
        }

        private async Task<FinalAddress> GetAddress(Address address)
        {
            var country = await context.Countries.SingleAsync(c => c.Id == address.CountryId);
            return new FinalAddress(address.AddressLine1, address.AddressLine2, address.TownOrCity, string.Empty, address.PostalCode, country.Name);
        }

        private FinalContact GetContact(Contact contact)
        {
            string telephone = string.Format("{0}-{1}", contact.TelephonePrefix, contact.Telephone);
            return new FinalContact(contact.ContactName, telephone, contact.Email);
        }
    }
}
