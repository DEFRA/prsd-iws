﻿namespace EA.Iws.RequestHandlers.Organisations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Organisations;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.Organisations;
    using Requests.Shared;

    internal class FindMatchingOrganisationsHandler :
        IRequestHandler<FindMatchingOrganisations, IList<OrganisationData>>
    {
        private readonly IwsContext context;

        private readonly SpecialBusinessSearchCase[] specialCases =
        {
            new SpecialBusinessSearchCase(new[] { "LTD", "LIMITED" }, Position.End),
            new SpecialBusinessSearchCase(new[] { "THE" }, Position.Start),
            new SpecialBusinessSearchCase(new[] { "PLC" }, Position.End),
            new SpecialBusinessSearchCase(new[] { "COMPANY", "CO" }, Position.End)
        };

        public FindMatchingOrganisationsHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IList<OrganisationData>> HandleAsync(FindMatchingOrganisations query)
        {
            var searchTerm = PrepareQuery(query);

            // This search uses the Levenshtein edit distance as a search algorithm.
            var permittedDistance = CalculateMaximumLevenshteinDistance(searchTerm);

            var possibleOrganisations = await GetPossibleOrganisationNames(searchTerm);

            var uppercaseOrganisationNames =
                possibleOrganisations.Select(o => new KeyValuePair<string, Guid>(o.Name.ToUpperInvariant(), o.Id))
                    .ToArray();

            // Special cases should be ignored when counting the distance. This loop replaces special cases with string.Empty.
            for (var i = 0; i < possibleOrganisations.Length; i++)
            {
                foreach (var specialCase in specialCases)
                {
                    specialCase.CleanseSpecialCases(ref uppercaseOrganisationNames[i]);
                }
            }

            var matchingIdsWithDistance = new List<KeyValuePair<Guid, int>>();

            for (var i = 0; i < possibleOrganisations.Length; i++)
            {
                var distance = StringSearch.CalculateLevenshteinDistance(searchTerm, uppercaseOrganisationNames[i].Key);

                if (distance <= permittedDistance)
                {
                    matchingIdsWithDistance.Add(new KeyValuePair<Guid, int>(uppercaseOrganisationNames[i].Value,
                        distance));
                }
            }

            matchingIdsWithDistance = matchingIdsWithDistance.OrderBy(m => m.Value).ToList();

            var matchingOrganisations =
                matchingIdsWithDistance.Select(
                    m => possibleOrganisations.Single(o => o.Id == m.Key));

            return matchingOrganisations.Select(o => 
                new OrganisationData
                {
                    Id = o.Id, 
                    Name = o.Name
                }).ToList();
        }

        private string PrepareQuery(FindMatchingOrganisations query)
        {
            var returnString = query.CompanyName.Trim().ToUpperInvariant();

            foreach (var specialCase in specialCases)
            {
                specialCase.CleanseSpecialCases(ref returnString);
            }

            return returnString;
        }

        private int CalculateMaximumLevenshteinDistance(string searchTerm)
        {
            var distance = 1;

            if (searchTerm.Length >= 5 && searchTerm.Length < 9)
            {
                distance = 2;
            }
            else if (searchTerm.Length >= 9 && searchTerm.Length < 15)
            {
                distance = 3;
            }
            else if (searchTerm.Length >= 15)
            {
                distance = 4;
            }

            return distance;
        }

        private async Task<Organisation[]> GetPossibleOrganisationNames(string searchTerm)
        {
            var firstLetterOfSearchTerm = searchTerm[0].ToString();

            return await context.Organisations
                .Where(o => o.Name.StartsWith(firstLetterOfSearchTerm)
                            || o.Name.StartsWith("THE "))
                .ToArrayAsync();
        }

        /// <summary>
        ///     Equivalent words will be treated as blank for the comparison.
        /// </summary>
        private class SpecialBusinessSearchCase
        {
            private readonly string[] equivalentWords;
            private readonly Position position;

            public SpecialBusinessSearchCase(IList<string> equivalentWords, Position position)
            {
                this.equivalentWords = new string[equivalentWords.Count];

                for (var i = 0; i < equivalentWords.Count; i++)
                {
                    this.equivalentWords[i] = equivalentWords[i].ToUpperInvariant().Replace(".", string.Empty);

                    switch (position)
                    {
                        case Position.Start:
                            this.equivalentWords[i] = equivalentWords[i] + " ";
                            break;
                        default:
                            this.equivalentWords[i] = " " + equivalentWords[i];
                            break;
                    }
                }

                this.position = position;
            }

            public void CleanseSpecialCases(ref KeyValuePair<string, Guid> name)
            {
                foreach (var equivalentWord in equivalentWords)
                {
                    switch (position)
                    {
                        case Position.Start:
                            if (name.Key.StartsWith(equivalentWord))
                            {
                                name = new KeyValuePair<string, Guid>(name.Key.Replace(equivalentWord, string.Empty),
                                    name.Value);
                                return;
                            }
                            break;
                        default:
                            if (name.Key.EndsWith(equivalentWord))
                            {
                                name = new KeyValuePair<string, Guid>(name.Key.Replace(equivalentWord, string.Empty),
                                    name.Value);
                                return;
                            }
                            break;
                    }
                }
            }

            public void CleanseSpecialCases(ref string name)
            {
                var kvp = new KeyValuePair<string, Guid>(name, Guid.Empty);

                CleanseSpecialCases(ref kvp);

                name = kvp.Key;
            }
        }

        private enum Position
        {
            Start,
            End
        }
    }
}