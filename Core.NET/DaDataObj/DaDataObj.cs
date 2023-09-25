using Dadata;
using Dadata.Model;
using System.Threading.Tasks;

namespace Core.NET.DaDataObj
{
    public static class DaDataObj
    {
        private static string _token = "60b3fe73d105072c460248beb1ac9784bc836314";
        private static string _secret = "56fe56f8037313f76dfc975325eb3c68eb5f2906";

        private static SuggestClientAsync _suggestClientAsync { get; set; }
        private static SuggestClientAsync GetSuggestClientAsync()
        {
            if (_suggestClientAsync is null)
            {
                _suggestClientAsync = new SuggestClientAsync(_token);
            }

            return _suggestClientAsync;
        }

        private static CleanClientAsync _cleanClientAsync { get; set; }
        private static CleanClientAsync GetCleanClientAsync()
        {
            if (_cleanClientAsync is null)
            {
                _cleanClientAsync = new CleanClientAsync(_token, _secret);
            }

            return _cleanClientAsync;
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
