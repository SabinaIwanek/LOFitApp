using LOFit.Tools;
using LOFit.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;

namespace LOFit.DataServices.Measurement
{
    public class MeasurementRestService : IMeasurementRestService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddresss;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializaerOptions;

        public MeasurementRestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseAddresss = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5217" : "https://localhost:7205";
            _url = $"{_baseAddresss}/api/measurement";

            _jsonSerializaerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<MeasurementModel> Get(DateTime date)
        {
            MeasurementModel model = new MeasurementModel();

            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return null;
            }

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/{date.ToString("yyyy-MM-dd")}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    model = JsonSerializer.Deserialize<MeasurementModel>(responseContent, _jsonSerializaerOptions);
                    model.Data_pomiaru = date;

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
        public async Task<List<MeasurementModel>> GetWeek(DateTime date)
        {
            List<MeasurementModel> list = new List<MeasurementModel>();

            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return null;
            }

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/week/{date.ToString("yyyy-MM-dd")}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    list = JsonSerializer.Deserialize<List<MeasurementModel>>(responseContent, _jsonSerializaerOptions);

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
        public async Task<string> Add(MeasurementModel form)
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

                HttpResponseMessage response = await _httpClient.PostAsync($"{_url}", content);

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
        public async Task<string> Update(MeasurementModel form)
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
    }
}
