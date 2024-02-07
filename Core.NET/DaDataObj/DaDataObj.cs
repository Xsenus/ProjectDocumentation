using Dadata;
using Dadata.Model;
using System.Threading.Tasks;

namespace Core.NET.DaDataObj
{
    public static class DaDataObj
    {
        private static readonly string _token = "60b3fe73d105072c460248beb1ac9784bc836314";
        private static readonly string _secret = "56fe56f8037313f76dfc975325eb3c68eb5f2906";

        private static SuggestClientAsync SuggestClientAsync { get; set; }
        private static SuggestClientAsync GetSuggestClientAsync()
        {
            if (SuggestClientAsync is null)
            {
                SuggestClientAsync = new SuggestClientAsync(_token);
            }

            return SuggestClientAsync;
        }

        private static CleanClientAsync CleanClientAsync { get; set; }
        private static CleanClientAsync GetCleanClientAsync()
        {
            if (CleanClientAsync is null)
            {
                CleanClientAsync = new CleanClientAsync(_token, _secret);
            }

            return CleanClientAsync;
        }

        public async static Task<SuggestResponse<Party>> GetPartyAsync(string obj)
        {
            return await GetSuggestClientAsync().FindParty(obj);
        }

        public async static Task<Address> GetAddressAsync(string obj)
        {
            return await GetCleanClientAsync().Clean<Address>(obj);
        }
    }
}
