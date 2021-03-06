﻿namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Domain.NotificationApplication;

    internal class AddressViewModel
    {
        private readonly string addressLine2 = string.Empty;
        private readonly string country = string.Empty;
        private readonly string postcode = string.Empty;
        private readonly string region = string.Empty;
        private readonly string townOrCity = string.Empty;
        private string addressLine1 = string.Empty;

        public AddressViewModel(Address address)
        {
            if (address == null)
            {
                return;
            }

            addressLine1 = address.Address1 ?? string.Empty;
            addressLine2 = address.Address2 ?? string.Empty;
            townOrCity = address.TownOrCity ?? string.Empty;
            region = address.Region ?? string.Empty;
            postcode = address.PostalCode ?? string.Empty;
            country = address.Country ?? string.Empty;
        }

        private AddressViewModel()
        {
        }

        public string Address(AddressLines numberOfLines)
        {
            switch (numberOfLines)
            {
                case (AddressLines.Single):
                    return MergeLines(addressLine1, addressLine2, townOrCity, region, postcode, country);
                default:
                    return Convert(new[]
                    {
                        MergeLines(addressLine1, addressLine2),
                        MergeLines(townOrCity, region),
                        MergeLines(postcode, country)
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

                if (i < args.Length - 1 && args.Any(s => !string.IsNullOrWhiteSpace(s) && s != args[i]))
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