using LOFit.Enums;
using LOFit.Models.Accounts;
using LOFit.Tools;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LOFit.DataServices.Login
{
    public class LoginRestService : ILoginRestService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddresss;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializaerOptions;

        public LoginRestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseAddresss = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5217" : "https://localhost:7205";
            _url = $"{_baseAddresss}/api/login";

            _jsonSerializaerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        // TokenGetModel
        public async Task<string> Login(LoginModel model)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return "Brak połączenia z internetem...";
            }

            try
            {
                string jsonBody = JsonSerializer.Serialize(model, _jsonSerializaerOptions);
                StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"{_url}", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    TokenGetModel tokenInfo = JsonSerializer.Deserialize<TokenGetModel>(responseContent, _jsonSerializaerOptions);

                    Singleton.Instance.Token = tokenInfo.Token;
                    Singleton.Instance.Type = (TypKonta)tokenInfo.Typ;
                    Singleton.Instance.User = model.Email;

                    return "Ok";
                }
                else
                {
                    return response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";
            }
        }

        public async Task<string> SendMail(string email)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return "Brak połączenia z internetem...";
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/sendcode/{email}");

                if (response.IsSuccessStatusCode)
                {
                    string answer = response.Content.ReadAsStringAsync().Result;

                    return answer;
                }
                else
                {
                    return response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";
            }
        }

        public async Task<string> ChangePassword(ChangePasswordModel form)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return "Brak połączenia z internetem...";
            }

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string jsonBody = JsonSerializer.Serialize(form, _jsonSerializaerOptions);
                StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/changepsw", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return "Ok";
                }
                else
                {
                    return response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";
            }
        }
    }
}
