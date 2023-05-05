using System.Net.Http.Headers;
using System.Text.Json;
using LOFit.Tools;
using LOFit.Models.Accounts;
using LOFit.Models.ProfileMenu;
using LOFit.Models.Menu;

namespace LOFit.DataServices.Admin
{
    public class AdminRestService : IAdminRestService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddresss;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializaerOptions;

        public AdminRestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseAddresss = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5217" : "https://localhost:7205";
            _url = $"{_baseAddresss}/api/adminpanel";

            _jsonSerializaerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<AdminModel> GetOne(int id)
        {
            AdminModel model = new AdminModel();

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    model = JsonSerializer.Deserialize<AdminModel>(responseContent, _jsonSerializaerOptions);

                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Zarzadzanie
        public async Task<List<CoachModel>> GetWgTypeCoach(int type)
        {
            List<CoachModel> list = new List<CoachModel>();

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/coachs/{type}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    list = JsonSerializer.Deserialize<List<CoachModel>>(responseContent, _jsonSerializaerOptions);

                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string> SetCoach(int id, int type)
        {
            try
            {
                string token = Singleton.Instance.Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/coachs/{id}/{type}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return responseContent;
                }
                else
                {
                    return "Bad response";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public async Task<List<CertificateModel>> GetWgTypeCert(int type)
        {
            List<CertificateModel> list = new List<CertificateModel>();

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/certificate/{type}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    list = JsonSerializer.Deserialize<List<CertificateModel>>(responseContent, _jsonSerializaerOptions);

                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string> SetCert(int id, int type)
        {
            try
            {
                string token = Singleton.Instance.Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/certificate/{id}/{type}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return responseContent;
                }
                else
                {
                    return "Bad response";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public async Task<List<OpinionModel>> GetWgTypeOpinion(int type)
        {
            List<OpinionModel> list = new List<OpinionModel>();

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/opinion/{type}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    list = JsonSerializer.Deserialize<List<OpinionModel>>(responseContent, _jsonSerializaerOptions);

                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string> SetOpinion(int id, int type)
        {
            try
            {
                string token = Singleton.Instance.Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/opinion/{id}/{type}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return responseContent;
                }
                else
                {
                    return "Bad response";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public async Task<List<ProductModel>> GetWgTypeProducts(int type)
        {
            List<ProductModel> list = new List<ProductModel>();

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/products/{type}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    list = JsonSerializer.Deserialize<List<ProductModel>>(responseContent, _jsonSerializaerOptions);

                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string> SetProduct(int id, int type)
        {
            try
            {
                string token = Singleton.Instance.Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/products/{id}/{type}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return responseContent;
                }
                else
                {
                    return "Bad response";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public async Task<List<AdminModel>> GetAppUsersAdmins()
        {
            List<AdminModel> list = new List<AdminModel>();

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/appusers/admins");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    list = JsonSerializer.Deserialize<List<AdminModel>>(responseContent, _jsonSerializaerOptions);

                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<UserModel>> GetAppUsers()
        {
            List<UserModel> list = new List<UserModel>();

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/appusers/users");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    list = JsonSerializer.Deserialize<List<UserModel>>(responseContent, _jsonSerializaerOptions);

                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<CoachModel>> GetAppUsersCoachs()
        {
            List<CoachModel> list = new List<CoachModel>();

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/appusers/coachs");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    list = JsonSerializer.Deserialize<List<CoachModel>>(responseContent, _jsonSerializaerOptions);

                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string> BlockAccount(int id, int type)
        {
            try
            {
                string token = Singleton.Instance.Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/appusers/block/{id}/{type}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return responseContent;
                }
                else
                {
                    return "Bad response";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public async Task<string> UnblockAccount(int id, int type)
        {
            try
            {
                string token = Singleton.Instance.Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/appusers/unblock/{id}/{type}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return responseContent;
                }
                else
                {
                    return "Bad response";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public async Task<string> DeleteAccount(int id, int type)
        {
            try
            {
                string token = Singleton.Instance.Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.DeleteAsync($"{_url}/appusers/{id}/{type}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return responseContent;
                }
                else
                {
                    return "Bad response";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public async Task<bool> IsBlock(int id, int type)
        {
            try
            {
                string token = Singleton.Instance.Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/appusers/isblock/{id}/{type}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return bool.Parse(responseContent);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
