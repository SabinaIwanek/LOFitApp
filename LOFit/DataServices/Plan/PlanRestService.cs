﻿using LOFit.Tools;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using LOFit.Models.Menu;
using LOFit.Models.MenuCoach;

namespace LOFit.DataServices.Plan
{
    public class PlanRestService : IPlanRestService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddresss;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializaerOptions;

        public PlanRestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseAddresss = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5217" : "https://localhost:7205";
            _url = $"{_baseAddresss}/api/plan";

            _jsonSerializaerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<List<List<WorkoutDayModel>>> GetWorkouts(int id)
        {
            List<List<WorkoutDayModel>> list = new List<List<WorkoutDayModel>>();

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/workouts/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    list = JsonSerializer.Deserialize<List<List<WorkoutDayModel>>>(responseContent, _jsonSerializaerOptions);

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
        public async Task<List<List<MealModel>>> GetMeals(int id)
        {
            List<List<MealModel>> list = new List<List<MealModel>> ();

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/meals/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    list = JsonSerializer.Deserialize<List<List<MealModel>>>(responseContent, _jsonSerializaerOptions);

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
        public async Task<List<PlanModel>> GetByType(int type)
        {
            List<PlanModel> list = new List<PlanModel>();

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/all/{type}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    list = JsonSerializer.Deserialize<List<PlanModel>>(responseContent, _jsonSerializaerOptions);

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
        public async Task<PlanModel> GetOne(int id)
        {
            PlanModel model = new PlanModel();

            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    model = JsonSerializer.Deserialize<PlanModel>(responseContent, _jsonSerializaerOptions);

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
        public async Task<int> Add(PlanModel form)
        {
            try
            {
                string token = Singleton.Instance.Token;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string jsonBody = JsonSerializer.Serialize(form, _jsonSerializaerOptions);
                StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"{_url}", content);

                if (response.IsSuccessStatusCode)
                {
                    int responseContent = Int32.Parse(await response.Content.ReadAsStringAsync());

                    return responseContent;
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
        public async Task<string> Update(PlanModel form)
        {
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
    }
}