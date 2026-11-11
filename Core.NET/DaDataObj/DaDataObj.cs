using Dadata.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.NET.DaDataObj
{
    public static class DaDataObj
    {
        private const string TokenSetting = "PROJECTDOCUMENTATION_DADATA_TOKEN";
        private const string SecretSetting = "PROJECTDOCUMENTATION_DADATA_SECRET";
        private static readonly Uri PartyEndpoint = new Uri("https://suggestions.dadata.ru/suggestions/api/4_1/rs/findById/party");
        private static readonly Uri AddressEndpoint = new Uri("https://cleaner.dadata.ru/api/v1/clean/address");
        private static readonly HttpClient Client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false })
        {
            Timeout = TimeSpan.FromSeconds(12),
            MaxResponseContentBufferSize = 4 * 1024 * 1024,
        };

        public static Task<SuggestResponse<Party>> GetPartyAsync(string query, CancellationToken cancellationToken = default,
            bool? individual = null)
        {
            return GetPartyAsync(Client, Environment.GetEnvironmentVariable, query, cancellationToken, individual);
        }

        internal static Task<SuggestResponse<Party>> GetPartyAsync(HttpClient client, Func<string, string> getSetting,
            string query, CancellationToken cancellationToken, bool? individual)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var parts = (query ?? "").Split('/');
            var identifier = parts[0].Trim();
            var kpp = parts.Length == 2 ? parts[1].Trim() : null;
            var legalIdentifier = IsDigits(identifier, 10) || IsDigits(identifier, 13);
            var individualIdentifier = IsDigits(identifier, 12) || IsDigits(identifier, 15);
            if (parts.Length > 2 || (kpp != null && (!legalIdentifier || !IsDigits(kpp, 9)))
                || (!legalIdentifier && !individualIdentifier) || (individual.HasValue && individual.Value != individualIdentifier))
            {
                throw new InvalidOperationException("Введите ИНН или ОГРН нужного типа. КПП юридического лица можно указать через дробь.");
            }

            var body = new JObject { ["query"] = identifier, ["type"] = legalIdentifier ? "LEGAL" : "INDIVIDUAL" };
            if (kpp != null) body["kpp"] = kpp;
            return SendAsync<SuggestResponse<Party>>(client, PartyEndpoint, body, ReadSetting(getSetting, TokenSetting), null, cancellationToken);
        }

        public static Task<Address> GetAddressAsync(string query, CancellationToken cancellationToken = default)
        {
            return GetAddressAsync(Client, Environment.GetEnvironmentVariable, query, cancellationToken);
        }

        internal static async Task<Address> GetAddressAsync(HttpClient client, Func<string, string> getSetting, string query,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            query = query?.Trim();
            if (string.IsNullOrEmpty(query) || query.Length < 3 || query.Length > 400)
                throw new InvalidOperationException("Введите адрес от 3 до 400 символов.");

            var addresses = await SendAsync<Address[]>(client, AddressEndpoint, new[] { query },
                ReadSetting(getSetting, TokenSetting), ReadSetting(getSetting, SecretSetting), cancellationToken).ConfigureAwait(false);
            var address = addresses?.Length == 1 ? addresses[0] : null;
            return IsRussianAddress(address) && address.qc == "0" ? address : null;
        }

        public static bool IsRussianAddress(Address address)
        {
            return address != null && (!string.IsNullOrEmpty(address.country_iso_code)
                ? string.Equals(address.country_iso_code, "RU", StringComparison.OrdinalIgnoreCase)
                : address.country == "Россия");
        }

        public static bool IsDigits(string value, int length)
        {
            return value?.Length == length && value.All(character => character >= '0' && character <= '9');
        }

        private static string ReadSetting(Func<string, string> getSetting, string name)
        {
            var value = getSetting(name)?.Trim();
            if (string.IsNullOrEmpty(value))
                throw new InvalidOperationException($"DaData не настроена. Задайте {name} и перезапустите приложение либо заполните поля вручную.");
            if (value.Any(character => character < 33 || character > 126))
                throw new InvalidOperationException($"Проверьте настройку {name} или заполните поля вручную.");
            return value;
        }

        private static async Task<T> SendAsync<T>(HttpClient client, Uri endpoint, object body, string token, string secret,
            CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Token", token);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (secret != null) request.Headers.Add("X-Secret", secret);
                request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                try
                {
                    using (var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false))
                    {
                        if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                            throw new InvalidOperationException("DaData отклонила запрос. Проверьте ключи, состояние учётной записи и лимиты.");
                        if ((int)response.StatusCode == 429)
                            throw new InvalidOperationException("DaData ограничила частоту запросов. Повторите позже или заполните поля вручную.");
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException($"DaData вернула ошибку HTTP {(int)response.StatusCode}. Повторите позже или заполните поля вручную.");

                        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        cancellationToken.ThrowIfCancellationRequested();
                        return JsonConvert.DeserializeObject<T>(content);
                    }
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    throw new InvalidOperationException("DaData не ответила вовремя. Повторите запрос или заполните поля вручную.");
                }
                catch (HttpRequestException)
                {
                    throw new InvalidOperationException("Не удалось подключиться к DaData. Проверьте интернет или заполните поля вручную.");
                }
                catch (JsonException)
                {
                    throw new InvalidOperationException("DaData вернула непонятный ответ. Повторите запрос или заполните поля вручную.");
                }
            }
        }
    }
}
