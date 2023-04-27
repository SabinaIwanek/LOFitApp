using LOFit.Tools;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using LOFit.Models.Menu;

namespace LOFit.DataServices.Workout
{
    public class WorkoutRestService : IWorkoutRestService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddresss;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializaerOptions;

        public WorkoutRestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseAddresss = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5217" : "https://localhost:7205";
            _url = $"{_baseAddresss}/api/workout";

            _jsonSerializaerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<int> Add(WorkoutModel form)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return 0;
            }

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string jsonBody = JsonSerializer.Serialize(form, _jsonSerializaerOptions);
                StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"{_url}", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<int>(responseContent, _jsonSerializaerOptions);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<string> Update(WorkoutModel form)
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

                HttpResponseMessage response = await _httpClient.PutAsync($"{_url}", content);

                if (response.IsSuccessStatusCode)
                {
                    return "Ok";
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
        public async Task<string> Delete(int id)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return "Brak połączenia z internetem...";
            }

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.DeleteAsync($"{_url}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return "Ok";
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
        public async Task<WorkoutModel> GetOne(int id)
        {
            WorkoutModel model = new WorkoutModel();

            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return null;
            }

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    model = JsonSerializer.Deserialize<WorkoutModel>(responseContent, _jsonSerializaerOptions);

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
        public async Task<List<WorkoutModel>> GetUserList()
        {
            List<WorkoutModel> model = new List<WorkoutModel>();

            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return null;
            }

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/userlist");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    model = JsonSerializer.Deserialize<List<WorkoutModel>>(responseContent, _jsonSerializaerOptions);

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
        public async Task<List<WorkoutModel>> GetAppList()
        {
            List<WorkoutModel> model = new List<WorkoutModel>();

            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return null;
            }

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/applist");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    model = JsonSerializer.Deserialize<List<WorkoutModel>>(responseContent, _jsonSerializaerOptions);

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

    }
}