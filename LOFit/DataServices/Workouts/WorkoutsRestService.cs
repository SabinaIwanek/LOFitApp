﻿using LOFit.Tools;
using LOFit.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LOFit.DataServices.Workouts
{
    public class WorkoutsRestService : IWorkoutsRestService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddresss;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializaerOptions;

        public WorkoutsRestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseAddresss = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5217" : "https://localhost:7205";
            _url = $"{_baseAddresss}/api/workoutslist";

            _jsonSerializaerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<string> Add(WorkoutDayModel form)
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
        public async Task<string> Update(WorkoutDayModel form)
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
        public async Task<WorkoutDayModel> GetOne(int id)
        {
            WorkoutDayModel model = new WorkoutDayModel();

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

                    model = JsonSerializer.Deserialize<WorkoutDayModel>(responseContent, _jsonSerializaerOptions);

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
        public async Task<List<WorkoutDayModel>> GetUserList(DateTime date)
        {
            List<WorkoutDayModel> model = new List<WorkoutDayModel>();

            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return null;
            }

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/userlist/{date.ToString("yyyy-MM-dd")}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    model = JsonSerializer.Deserialize<List<WorkoutDayModel>>(responseContent, _jsonSerializaerOptions);

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