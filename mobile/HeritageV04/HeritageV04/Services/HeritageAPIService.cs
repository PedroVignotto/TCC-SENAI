using HeritageV04.Models;
using HeritageV04.Services.Abstractions;
using HeritageV04.Utilities;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Environment = HeritageV04.Models.Environment;
using System.IO;

namespace HeritageV04.Services
{
    public class HeritageAPIService : IHeritageAPIService
    {

        public static Uri ApiBaseUrl = new Uri("http://10.0.2.2:3333");

        public HttpClient HttpClient;
        readonly INetworkService _networkService;

        public HeritageAPIService()
        {

            HttpClient = new HttpClient { BaseAddress = ApiBaseUrl };
            HttpClient.DefaultRequestHeaders.ConnectionClose = false;
            Token token = new Token();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.GetToken());
            _networkService = new NetworkService();

        }

        #region RetryPolitic
        Task OnRetry(Exception e, int retryCount)
        {
            return Task.Factory.StartNew(() =>
            {
                System.Diagnostics.Debug.WriteLine($"Tentativa #{retryCount}");
            });
        }
        #endregion

        #region UserRequests
        async Task<UserRoot> UserLoginRequest(User user)
        {
            var data = JsonConvert.SerializeObject(user);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await HttpClient.PostAsync($"{ApiBaseUrl}sessions", content);

            if (response.IsSuccessStatusCode)
            {
                var repost = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserRoot>(repost);
            }
            else
                return null;
        }

        async Task<Repost> UserPutRequest(User user, string route)
        {
            var data = JsonConvert.SerializeObject(user);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await HttpClient.PutAsync($"{ApiBaseUrl}{route}", content);

            var repostage = await response.Content.ReadAsStringAsync();
            Repost repost = JsonConvert.DeserializeObject<Repost>(repostage);
            repost.Success = response.IsSuccessStatusCode;

            return repost;
        }

        async Task<ObservableCollection<User>> UsersGetRequest(string route)
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiBaseUrl}{route}");

            if (response.IsSuccessStatusCode)
            {
                var repost = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<User>>(repost);
            }
            else
                return null;

        }

        async Task<User> UserGetRequest(string route)
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiBaseUrl}{route}");

            if (response.IsSuccessStatusCode)
            {
                var repost = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(repost);
            }
            else
                return null;

        }
        #endregion

        #region UserMethods
        public async Task<User> UserLogin(User user)
        {
            Token token = new Token();
            UserRoot userRoot = new UserRoot();

            try
            {
                var func = new Func<Task<UserRoot>>(() => UserLoginRequest(user));
                userRoot = await _networkService.Retry(func, 3, OnRetry);
                token.SetToken(userRoot.Token);
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", userRoot.Token);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userRoot.User;

        }

        public async Task<Repost> PutAsync(User user)
        {

            Repost repost = new Repost();

            try
            {
                var func = new Func<Task<Repost>>(() => UserPutRequest(user, $"users/{user.Id}"));
                repost = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return repost;

        }

        public async Task<Repost> AddUserAsync(User user)
        {

            Repost repost = new Repost();

            try
            {
                var func = new Func<Task<Repost>>(() => UserPutRequest(user, $"company/users/{user.Email}"));
                repost = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return repost;

        }

        public async Task<ObservableCollection<User>> GetAsyncUsers(int? CompanyId)
        {

            ObservableCollection<User> users = new ObservableCollection<User>();

            try
            {
                var func = new Func<Task<ObservableCollection<User>>>(() => UsersGetRequest($"{CompanyId}/users"));
                users = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return users;

        }

        public async Task<ObservableCollection<User>> GetAsyncUsers()
        {

            ObservableCollection<User> users = new ObservableCollection<User>();

            try
            {

                var func = new Func<Task<ObservableCollection<User>>>(() => UsersGetRequest("users"));
                users = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return users;

        }

        public async Task<User> GetAsyncUser(int? Id)
        {

            User user = new User();

            try
            {

                var func = new Func<Task<User>>(() => UserGetRequest($"users/{Id}"));
                user = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return user;

        }

        public async Task<bool> DeleteAsync(User user)
        {

            Repost repost = new Repost();

            try
            {

                var func = new Func<Task<Repost>>(() => UserPutRequest(user, $"users/{user.Id}"));
                repost = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return repost.Success;

        }
        #endregion

        #region EnvironmentRequests
        async Task<Repost> EnvironmentPostRequest(Environment environment)
        {
            var data = JsonConvert.SerializeObject(environment.Name);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await HttpClient.PostAsync($"{ApiBaseUrl}environments", content);

            var repostage = await response.Content.ReadAsStringAsync();
            Repost repost = JsonConvert.DeserializeObject<Repost>(repostage);
            repost.Success = response.IsSuccessStatusCode;

            return repost;
        }

        async Task<bool> EnvironmentPutRequest(Environment environment)
        {
            var data = JsonConvert.SerializeObject(environment);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await HttpClient.PutAsync($"{ApiBaseUrl}environments/{environment.Id}", content);

            return response.IsSuccessStatusCode;

        }

        async Task<bool> EnvironmentDeleteRequest(int? Id)
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiBaseUrl}environments/{Id}");
            return response.IsSuccessStatusCode;
        }

        async Task<ObservableCollection<Environment>> EnvironmentsGetRequest(int? CompanyId)
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiBaseUrl}{CompanyId}/environments");

            if (response.IsSuccessStatusCode)
            {
                var repost = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<Environment>>(repost);
            }
            else
                return null;

        }

        async Task<Environment> EnvironmentGetRequest(int? CompanyId, string EnvironmentName)
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiBaseUrl}{CompanyId}/environments/{EnvironmentName}");

            if (response.IsSuccessStatusCode)
            {
                var repost = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Environment>(repost);
            }
            else
                return null;
        }
        #endregion

        #region EnvironmentMethods
        public async Task<Repost> PostAsync(Environment environment)
        {

            Repost repost = new Repost();

            try
            {
                var func = new Func<Task<Repost>>(() => EnvironmentPostRequest(environment));
                repost = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return repost;

        }

        public async Task<bool> DeleteAsync(Environment environment)
        {

            bool delete = false;

            try
            {
                var func = new Func<Task<bool>>(() => EnvironmentDeleteRequest(environment.Id));
                delete = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return delete;

        }

        public async Task<bool> PutAsync(Environment environment)
        {

            bool put = false;

            try
            {
                var func = new Func<Task<bool>>(() => EnvironmentPutRequest(environment));
                put = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return put;

        }

        public async Task<ObservableCollection<Environment>> GetAsyncEnvironments(int? CompanyId)
        {

            ObservableCollection<Environment> environments = new ObservableCollection<Environment>();

            try
            {
                var func = new Func<Task<ObservableCollection<Environment>>>(() => EnvironmentsGetRequest(CompanyId));
                environments = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return environments;

        }

        public async Task<Environment> GetAsyncEnvironment(int? CompanyId, string EnvironmentName)
        {

            Environment environment = new Environment();

            try
            {
                var func = new Func<Task<Environment>>(() => EnvironmentGetRequest(CompanyId, EnvironmentName));
                environment = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return environment;

        }
        #endregion

        #region HeritageResquests
        async Task<Repost> HeritagePostRequest(Heritage heritage)
        {
            heritage.SerializeCode = true;
            var data = JsonConvert.SerializeObject(heritage);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await HttpClient.PostAsync($"{ApiBaseUrl}heritages", content);

            var repostage = await response.Content.ReadAsStringAsync();
            Repost repost = JsonConvert.DeserializeObject<Repost>(repostage);
            repost.Success = response.IsSuccessStatusCode;

            return repost;
        }

        async Task<bool> HeritageDeleteRequest(int? Id)
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiBaseUrl}heritages/{Id}");
            return response.IsSuccessStatusCode;
        }

        async Task<Repost> HeritagePutRequest(Heritage heritage)
        {
            heritage.SerializeCode = false;
            var data = JsonConvert.SerializeObject(heritage);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await HttpClient.PutAsync($"{ApiBaseUrl}heritages/{heritage.Id}", content);

            var repostage = await response.Content.ReadAsStringAsync();
            Repost repost = JsonConvert.DeserializeObject<Repost>(repostage);
            repost.Success = response.IsSuccessStatusCode;

            return repost;
        }

        async Task<Repost> HeritagesPutRequest(string route)
        {
            HttpContent content = new StringContent("");
            HttpResponseMessage response = await HttpClient.PutAsync($"{ApiBaseUrl}{route}", content);

            var repostage = await response.Content.ReadAsStringAsync();
            Repost repost = JsonConvert.DeserializeObject<Repost>(repostage);
            repost.Success = response.IsSuccessStatusCode;

            return repost;
        }

        async Task<ObservableCollection<Heritage>> HeritagesGetRequest(int? CompanyId)
        {
            string url = $"{ApiBaseUrl}{CompanyId}/heritages";
            HttpResponseMessage response = await HttpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var repost = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<Heritage>>(repost);
            }
            else
                return null;

        }

        async Task<ObservableCollection<Heritage>> HeritagesGetRequest(int? CompanyId, int? EnvironmentId)
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiBaseUrl}{CompanyId}/environments/{EnvironmentId}/heritages");

            if (response.IsSuccessStatusCode)
            {
                var repost = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<Heritage>>(repost);
            }
            else
                return null;

        }
        #endregion

        #region HeritageMethods
        public async Task<Repost> PostAsync(Heritage heritage)
        {
            Repost repost = new Repost();

            try
            {

                var func = new Func<Task<Repost>>(() => HeritagePostRequest(heritage));
                repost = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return repost;

        }

        public async Task<bool> DeleteAsync(Heritage heritage)
        {

            bool delete = false;

            try
            {
                var func = new Func<Task<bool>>(() => HeritageDeleteRequest(heritage.Id));
                delete = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return delete;

        }

        public async Task<bool> PutAsync(Heritage heritage)
        {

            Repost repost = new Repost();

            try
            {
                var func = new Func<Task<Repost>>(() => HeritagePutRequest(heritage));
                repost = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return repost.Success;

        }

        public async Task<ObservableCollection<Heritage>> GetAsyncHeritages(int? CompanyId)
        {

            ObservableCollection<Heritage> heritages = new ObservableCollection<Heritage>();

            try
            {
                var func = new Func<Task<ObservableCollection<Heritage>>>(() => HeritagesGetRequest(CompanyId));
                heritages = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return heritages;

        }

        public async Task<ObservableCollection<Heritage>> GetAsyncHeritages(int? EnvironmentId, int? CompanyId)
        {

            ObservableCollection<Heritage> heritages = new ObservableCollection<Heritage>();

            try
            {

                var func = new Func<Task<ObservableCollection<Heritage>>>(() => HeritagesGetRequest(CompanyId, EnvironmentId));
                heritages = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return heritages;

        }

        public async Task<bool> VerificationAsync(int? EnvironmentId)
        {
            Repost repost = new Repost();

            try
            {
                var func = new Func<Task<Repost>>(() => HeritagesPutRequest($"conferences/{EnvironmentId}"));
                repost = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return repost.Success;
        }
        #endregion

        async Task<ObservableCollection<Historic>> HistoricsGetRequest(int? CompanyId)
        {
            string url = $"{ApiBaseUrl}{CompanyId}/historical";
            HttpResponseMessage response = await HttpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var repost = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<Historic>>(repost);
            }
            else
                return null;

        }

        public async Task<ObservableCollection<Historic>> GetAsyncHistorics(int? CompanyId)
        {

            ObservableCollection<Historic> historics = new ObservableCollection<Historic>();

            try
            {

                var func = new Func<Task<ObservableCollection<Historic>>>(() => HistoricsGetRequest(CompanyId));
                historics = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return historics;

        }

        async Task<Repost> MaintenancePostRequest(Maintenance maintenance)
        {
            var data = JsonConvert.SerializeObject(maintenance);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await HttpClient.PostAsync($"{ApiBaseUrl}maintenance/{maintenance.CompanyId}", content);

            var repostage = await response.Content.ReadAsStringAsync();
            Repost repost = JsonConvert.DeserializeObject<Repost>(repostage);
            repost.Success = response.IsSuccessStatusCode;

            return repost;
        }

        //async Task<Repost> FilePostRequest(MediaFile file)
        //{

        //    var fileBytes = System.IO.File.ReadAllBytes(file);
        //    MultipartFormDataContent content = new MultipartFormDataContent();
        //    ByteArrayContent byteArray = new ByteArrayContent();
        //    content.Add();
        //    HttpResponseMessage response = await HttpClient.PostAsync($"{ApiBaseUrl}maintenance/{maintenance.CompanyId}", content);

        //    var repostage = await response.Content.ReadAsStringAsync();
        //    Repost repost = JsonConvert.DeserializeObject<Repost>(repostage);
        //    repost.Success = response.IsSuccessStatusCode;

        //    return repost;
        //}

        public async Task<Repost> PostAsync(Maintenance maintenance)
        {
            Repost repost = new Repost();

            try
            {

                var func = new Func<Task<Repost>>(() => MaintenancePostRequest(maintenance));
                repost = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return repost;

        }

        //public async Task<Repost> PostAsync(MediaFile file)
        //{
        //    Repost repost = new Repost();

        //    try
        //    {

        //        var func = new Func<Task<Repost>>(() => MaintenancePostRequest(maintenance));
        //        repost = await _networkService.Retry(func, 3, OnRetry);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return repost;

        //}

    }
}
