namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Movement;
    using Domain.NotificationApplication;

    internal class MovementCarriersViewModel
    {
        private AddressViewModel firstAddress;
        private AddressViewModel secondAddress;
        private AddressViewModel lastAddress;

        public string FirstAddress
        {
            get { return firstAddress.Address(AddressLines.Single); }
        }

        public string SecondAddress
        {
            get { return secondAddress.Address(AddressLines.Single); }
        }

        public string LastAddress
        {
            get { return lastAddress.Address(AddressLines.Single); }
        }

        public string FirstReg { get; private set; }
        public string FirstName { get; private set; }
        public string FirstTel { get; private set; }
        public string FirstFax { get; private set; }
        public string FirstEmail { get; private set; }

        public string SecondReg { get; private set; }
        public string SecondName { get; private set; }
        public string SecondTel { get; private set; }
        public string SecondFax { get; private set; }
        public string SecondEmail { get; private set; }

        public string LastReg { get; private set; }
        public string LastName { get; private set; }
        public string LastTel { get; private set; }
        public string LastFax { get; private set; }
        public string LastEmail { get; private set; }

        public string AnnexMessage { get; private set; }

        public bool IsAnnexNeeded { get; private set; }

        public List<MovementCarrierDetails> CarrierDetails { get; private set; }

        public MovementCarriersViewModel(List<MovementCarrier> movementCarriersList)
        {
            CarrierDetails = new List<MovementCarrierDetails>();
            SetAllPropertiesToEmptyString();
            var sortedCarriersList = movementCarriersList.OrderBy(c => c.Order).ToList();
            IsAnnexNeeded = false;

            if (sortedCarriersList.Count == 1)
            {
                SetPropertiesForOneCarrier(sortedCarriersList);
            }

            if (sortedCarriersList.Count == 2)
            {
                SetPropertiesForTwoCarriers(sortedCarriersList);
            }

            if (sortedCarriersList.Count == 3)
            {
                SetPropertiesForThreeCarriers(sortedCarriersList);
            }

            if (sortedCarriersList.Count > 3)
            {
                SetPropertiesForMoreThanThreeCarriers(sortedCarriersList);
            }
        }

        private void SetAllPropertiesToEmptyString()
        {
            FirstReg = string.Empty;
            FirstName = string.Empty;
            firstAddress = AddressViewModel.GetAddressViewModelShowingSeeAnnexInstruction(string.Empty);
            FirstTel = string.Empty;
            FirstFax = string.Empty;
            FirstEmail = string.Empty;

            SecondReg = string.Empty;
            SecondName = string.Empty;
            secondAddress = AddressViewModel.GetAddressViewModelShowingSeeAnnexInstruction(string.Empty);
            SecondTel = string.Empty;
            SecondFax = string.Empty;
            SecondEmail = string.Empty;

            LastReg = string.Empty;
            LastName = string.Empty;
            lastAddress = AddressViewModel.GetAddressViewModelShowingSeeAnnexInstruction(string.Empty);
            LastTel = string.Empty;
            LastFax = string.Empty;
            LastEmail = string.Empty;

            AnnexMessage = string.Empty;
        }

        private void SetPropertiesForOneCarrier(List<MovementCarrier> movementCarriersList)
        {
            AddCarrierToFirstFields(movementCarriersList[0].Carrier);
        }

        private void SetPropertiesForTwoCarriers(List<MovementCarrier> movementCarriersList)
        {
            AddCarrierToFirstFields(movementCarriersList[0].Carrier);
            AddCarrierToLastFields(movementCarriersList[1].Carrier);
        }

        private void SetPropertiesForThreeCarriers(List<MovementCarrier> movementCarriersList)
        {
            AddCarrierToFirstFields(movementCarriersList[0].Carrier);
            AddCarrierToSecondFields(movementCarriersList[1].Carrier);
            AddCarrierToLastFields(movementCarriersList[2].Carrier);
        }

        private void SetPropertiesForMoreThanThreeCarriers(List<MovementCarrier> sortedCarriersList)
        {
            for (int i = 0; i < sortedCarriersList.Count; i++)
            {
                var mcd = new MovementCarrierDetails
                {
                    Order = (i + 1).ToString(),
                    Reg = sortedCarriersList[i].Carrier.Business.RegistrationNumber,
                    Name = sortedCarriersList[i].Carrier.Business.Name,
                    AddressViewModel = new AddressViewModel(sortedCarriersList[i].Carrier.Address),
                    Tel = sortedCarriersList[i].Carrier.Contact.Telephone.ToFormattedContact(),
                    Fax = sortedCarriersList[i].Carrier.Contact.Fax.ToFormattedContact(),
                    Email = sortedCarriersList[i].Carrier.Contact.Email
                };

                CarrierDetails.Add(mcd);
                IsAnnexNeeded = true;
            }

            AnnexMessage = "See carriers annex";
        }

        private void AddCarrierToFirstFields(Carrier carrier)
        {
            FirstReg = carrier.Business.RegistrationNumber;
            FirstName = carrier.Business.Name;
            firstAddress = new AddressViewModel(carrier.Address);
            FirstTel = carrier.Contact.Telephone.ToFormattedContact();
            FirstFax = carrier.Contact.Fax.ToFormattedContact();
            FirstEmail = carrier.Contact.Email;
        }

        private void AddCarrierToSecondFields(Carrier carrier)
        {
            SecondReg = carrier.Business.RegistrationNumber;
            SecondName = carrier.Business.Name;
            secondAddress = new AddressViewModel(carrier.Address);
            SecondTel = carrier.Contact.Telephone.ToFormattedContact();
            SecondFax = carrier.Contact.Fax.ToFormattedContact();
            SecondEmail = carrier.Contact.Email;
        }

        private void AddCarrierToLastFields(Carrier carrier)
        {
            LastReg = carrier.Business.RegistrationNumber;
            LastName = carrier.Business.Name;
            lastAddress = new AddressViewModel(carrier.Address);
            LastTel = carrier.Contact.Telephone.ToFormattedContact();
            LastFax = carrier.Contact.Fax.ToFormattedContact();
            LastEmail = carrier.Contact.Email;
        }
    }
}