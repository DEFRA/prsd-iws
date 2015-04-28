namespace EA.Iws.Cqrs.Organisations
{
    using System;
    using System.Collections.Generic;
    using Core.Cqrs;

    public class FindMatchingCompanies : IQuery<IList<KeyValuePair<string, Guid>>>
    {
        public readonly string CompanyName;

        public FindMatchingCompanies(string companyName)
        {
            if (companyName == null)
            {
                throw new ArgumentNullException();
            }

            this.CompanyName = companyName;
        }
    }
}
