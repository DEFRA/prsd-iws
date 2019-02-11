namespace EA.Iws.DocumentGeneration.ViewModels
{
    internal class MovementCarrierDetails
    {
        public string Order { get; set; }
        public string Reg { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }

        public AddressViewModel AddressViewModel { private get; set; }

        public string Address
        {
            get
            {
                if (AddressViewModel != null)
                {
                    return AddressViewModel.Address(AddressLines.Multiple);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
