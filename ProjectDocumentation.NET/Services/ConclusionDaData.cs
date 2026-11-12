using Core.NET.DaDataObj;
using Core.NET.Extensions;
using Dadata.Model;
using DevExpress.XtraEditors;
using ProjectDocumentation.NET.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDocumentation.NET.Services
{
    internal static class ConclusionDaData
    {
        public static async Task<Dictionary<string, string>> FindOrganizationAsync(string inn, string kpp = null,
            CancellationToken cancellationToken = default)
        {
            var response = await DaDataObj.GetPartyAsync(IsDigits(kpp, 9) ? inn + "/" + kpp : inn,
                cancellationToken, individual: false);
            var parties = response?.suggestions?.Select(suggestion => suggestion?.data).Where(party => party?.type == PartyType.LEGAL);
            if (IsDigits(kpp, 9)) parties = parties?.Where(party => party?.kpp == kpp);
            return MapOrganization(parties, inn);
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
            return DaDataObj.IsDigits(value, length);
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

    internal static class LegacyDaDataLookup
    {
        public static Address FindAddress(ButtonEdit source)
        {
            var owner = source.FindForm();
            if (owner == null || owner.IsDisposed || owner.Disposing) return null;
            if (source.EditValue != null && !(source.EditValue is string)) return null;
            return AddressLookupDialog.ShowFor(owner, source.EditValue as string);
        }

        public static async Task<Party> FindPartyAsync(ButtonEdit source, string query, BaseEdit inn, BaseEdit ogrn,
            BaseEdit kpp = null, bool individual = false)
        {
            return await RunAsync(source, async token =>
            {
                var parts = query.Split('/');
                var identifier = parts[0].Trim();
                var branchKpp = parts.Length == 2 ? parts[1].Trim() : kpp?.EditValue?.ToString()?.Trim();
                var request = parts.Length == 1 && DaDataObj.IsDigits(branchKpp, 9) ? identifier + "/" + branchKpp : query;
                var result = await DaDataObj.GetPartyAsync(request, token, individual);
                var party = SelectParty(result?.suggestions?.Select(suggestion => suggestion?.data), identifier, branchKpp, individual);
                if (party != null && (Conflicts(inn, party.inn) || Conflicts(ogrn, party.ogrn) || Conflicts(kpp, party.kpp)))
                    throw new InvalidOperationException("Реквизиты организации в DaData отличаются от заполненных. Проверьте ИНН, ОГРН и КПП.");
                return party;
            });
        }

        internal static Party SelectParty(IEnumerable<Party> parties, string identifier, string kpp, bool individual)
        {
            var type = individual ? PartyType.INDIVIDUAL : PartyType.LEGAL;
            var matches = parties?.Where(party => party != null && party.type == type
                && (party.inn == identifier || party.ogrn == identifier)
                && (string.IsNullOrEmpty(kpp) || party.kpp == kpp)).ToArray();
            if (matches == null || matches.Length == 0) return null;
            if (matches.Length == 1) return matches[0];
            var headquarters = matches.Where(party => party.branch_type == PartyBranchType.MAIN).ToArray();
            return headquarters.Length == 1 ? headquarters[0] : null;
        }

        public static void FillEmpty(BaseEdit target, object value)
        {
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()) && string.IsNullOrWhiteSpace(target.EditValue?.ToString()))
                target.EditValue = value;
        }

        private static bool Conflicts(BaseEdit target, string value)
        {
            var existing = target?.EditValue?.ToString()?.Trim();
            return !string.IsNullOrEmpty(existing) && !string.IsNullOrEmpty(value) && existing != value;
        }

        private static async Task<T> RunAsync<T>(ButtonEdit source, Func<CancellationToken, Task<T>> search) where T : class
        {
            var owner = source.FindForm();
            if (owner == null || owner.IsDisposed || owner.Disposing) return null;
            using (var cancellation = new CancellationTokenSource())
            {
                FormClosedEventHandler cancel = (_, __) => cancellation.Cancel();
                owner.FormClosed += cancel;
                source.Enabled = false;
                try
                {
                    var result = await search(cancellation.Token);
                    if (cancellation.IsCancellationRequested || source.IsDisposed || owner.IsDisposed || owner.Disposing) return null;
                    if (result == null) ShowMessage(owner, "DaData не нашла однозначного совпадения. Заполните поля вручную.");
                    return result;
                }
                catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
                {
                    return null;
                }
                catch (InvalidOperationException error)
                {
                    if (!owner.IsDisposed && !owner.Disposing) ShowMessage(owner, error.Message);
                    return null;
                }
                finally
                {
                    owner.FormClosed -= cancel;
                    if (!source.IsDisposed && !owner.IsDisposed && !owner.Disposing) source.Enabled = true;
                }
            }
        }

        private static void ShowMessage(Form owner, string message)
        {
            XtraMessageBox.Show(owner, message, "DaData", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
