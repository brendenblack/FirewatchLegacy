using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackbox.Firewatch.Domain.Bank
{
    public class FinancialInstitution
    {
        public string Name { get; set; }

        public string Abbreviation { get; set; }

        private static List<FinancialInstitution> _institutions = new List<FinancialInstitution>
        {
            new FinancialInstitution
            {
                Name = "Royal Bank of Canada",
                Abbreviation = "RBC"
            }
        };

        public static FinancialInstitution FromAbbreviation(string abbreviation)
            => _institutions.FirstOrDefault(i => i.Abbreviation == "abbreviation") ?? Empty;

        // TODO: this probably won't scale...
        /// <summary>
        /// Returns a collection of all officially supported institutions.
        /// </summary>
        public static IReadOnlyCollection<FinancialInstitution> AllInstitutions => _institutions;

        public static FinancialInstitution RoyalBank
            => _institutions.First(i => i.Abbreviation == "RBC");

        public static FinancialInstitution Empty
            => new FinancialInstitution() { Name = "Unknown", Abbreviation = "Unknown" };
    }
}