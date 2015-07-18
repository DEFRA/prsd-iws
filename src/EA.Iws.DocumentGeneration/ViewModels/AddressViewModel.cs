namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System.Collections.Generic;
    using System.Text;
    using Domain;

    internal class AddressViewModel
    {
        private readonly string addressLine2;
        private readonly string country;
        private readonly string postcode;
        private readonly string region;
        private readonly string townOrCity;
        private string addressLine1;

        public AddressViewModel(Address address)
        {
            addressLine1 = address.Building + " " + address.Address1;
            addressLine2 = address.Address2;
            townOrCity = address.TownOrCity;
            region = address.Region;
            postcode = address.PostalCode;
            country = address.Country;
        }

        private AddressViewModel()
        {
        }

        public string Address(AddressLines numberOfLines)
        {
            switch (numberOfLines)
            {
                case (AddressLines.Two):
                    return Convert(new[]
                    {
                        MergeLines(addressLine1, addressLine2, townOrCity),
                        MergeLines(region, postcode, country)
                    });
                case (AddressLines.Three):
                    return Convert(new[]
                    {
                        MergeLines(addressLine1, addressLine2),
                        MergeLines(townOrCity, region),
                        MergeLines(postcode, country)
                    });
                default:
                    return
                        Convert(new[]
                        {
                            MergeLines(addressLine1),
                            MergeLines(addressLine2, townOrCity),
                            MergeLines(region, postcode),
                            MergeLines(country)
                        });
            }
        }

        private static string MergeLines(params string[] args)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < args.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(args[i]))
                {
                    continue;
                }

                sb.Append(args[i]);

                if (i < args.Length - 1 && !string.IsNullOrWhiteSpace(args[i + 1]))
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Converts the address represented by the collection of ordered strings to a string with
        ///     each string being on a new line.
        /// </summary>
        /// <param name="contents">All the lines of the address.</param>
        /// <returns>The address as a string with every entry being on a new line.</returns>
        private static string Convert(IList<string> contents)
        {
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < contents.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(contents[i]))
                {
                    continue;
                }

                stringBuilder.Append(contents[i]);

                if (i < contents.Count - 1)
                {
                    stringBuilder.AppendLine(",");
                }
            }

            return stringBuilder.ToString();
        }

        public static AddressViewModel GetAddressViewModelShowingSeeAnnexInstruction(string seeAnnexNotice)
        {
            return new AddressViewModel { addressLine1 = seeAnnexNotice };
        }
    }
}