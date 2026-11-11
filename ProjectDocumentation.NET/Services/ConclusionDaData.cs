using Core.NET.DaDataObj;
using Core.NET.Extensions;
using Dadata.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDocumentation.NET.Services
{
    internal static class ConclusionDaData
    {
        public static async Task<Dictionary<string, string>> FindOrganizationAsync(string inn)
        {
            var response = await DaDataObj.GetPartyAsync(inn);
            return MapOrganization(response?.suggestions?.Select(suggestion => suggestion.data), inn);
        }

        public static async Task<Dictionary<string, string>> NormalizeAddressAsync(string query)
        {
            return MapAddress(await DaDataObj.GetAddressAsync(query));
        }

        internal static Dictionary<string, string> MapOrganization(IEnumerable<Party> parties, string inn)
        {
            var matching = parties?.Where(party => party?.inn == inn).ToArray();
            if (matching == null || matching.Length == 0)
            {
                return null;
            }

            var fields = new Dictionary<string, string>();
            Add(fields, "FullName", CommonValue(matching, party => party.name?.full_with_opf ?? party.name?.full));
            Add(fields, "AbbreviatedName", CommonValue(matching, party => party.name?.short_with_opf ?? party.name?.@short));
            Add(fields, "OGRN", CommonValue(matching, party => IsDigits(party.ogrn, 13) ? party.ogrn : null));
            Add(fields, "INN", inn);
            // One INN can identify multiple branches. Only return a KPP shared by the exact matches.
            Add(fields, "KPP", CommonValue(matching, party => IsDigits(party.kpp, 9) ? party.kpp : null));
            return fields;
        }

        internal static Dictionary<string, string> MapAddress(Address address)
        {
            if (address == null || (address.qc != null && address.qc != "0")
                || (!string.IsNullOrEmpty(address.country_iso_code) && !address.country_iso_code.Equals("RU", StringComparison.OrdinalIgnoreCase))
                || (string.IsNullOrEmpty(address.country_iso_code) && !string.IsNullOrEmpty(address.country) && address.country != "Россия"))
            {
                return null;
            }

            var fields = new Dictionary<string, string>();
            var region = address.region_kladr_id.GetKladrRegionCode();
            Add(fields, "RegionCode", IsDigits(region, 2) && region != "00" ? region : null);
            Add(fields, "PostIndex", IsDigits(address.postal_code, 6) ? address.postal_code : null);
            if (IsDigits(address.oktmo, 8) || IsDigits(address.oktmo, 11))
            {
                Add(fields, "OKTMOCode", address.oktmo);
            }

            var countBeforeAddress = fields.Count;
            Add(fields, "District", address.city_district_with_type ?? address.city_district ?? address.area_with_type ?? address.area);
            Add(fields, "City", address.city_with_type ?? address.city);
            Add(fields, "Settlement", address.settlement_with_type ?? address.settlement);
            Add(fields, "Street", address.street_with_type ?? address.street);
            Add(fields, "Building", Join(address.house_with_type ?? address.house, Join(address.block_type, address.block)));
            Add(fields, "Room", string.IsNullOrWhiteSpace(address.flat) ? null : Join(address.flat_type, address.flat));
            if (fields.Count == countBeforeAddress)
            {
                Add(fields, "Note", address.result);
            }

            return fields.Count == 0 ? null : fields;
        }

        public static bool IsDigits(string value, int length)
        {
            return value?.Length == length && value.All(character => character >= '0' && character <= '9');
        }

        private static string CommonValue(Party[] items, Func<Party, string> selector)
        {
            var first = selector(items[0]);
            return items.All(item => selector(item) == first) ? first : null;
        }

        private static string Join(string first, string second)
        {
            return string.Join(" ", new[] { first, second }.Where(value => !string.IsNullOrWhiteSpace(value)));
        }

        private static void Add(Dictionary<string, string> fields, string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                fields[name] = value.Trim();
            }
        }
    }
}
