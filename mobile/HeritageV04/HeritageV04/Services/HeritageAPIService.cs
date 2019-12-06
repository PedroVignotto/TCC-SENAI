using HeritageV02MVVM.Utilities;
using HeritageV04.Models;
using HeritageV04.Services.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Environment = HeritageV04.Models.Environment;

namespace HeritageV04.Services
{
    public class HeritageAPIService
    {

        public static Uri ApiBaseUrl = new Uri("http://10.0.2.2:3333");

        public HttpClient HttpClient;
        readonly INetworkService _networkService; 

        public HeritageAPIService()
        {

            HttpClient = new HttpClient { BaseAddress = ApiBaseUrl };

            HttpClient.DefaultRequestHeaders.ConnectionClose = false;

            _networkService = new NetworkService();

        }

        #region RetryPolitic
        Task OnRetry(Exception e, int retryCount)
        {
            return Task.Factory.StartNew(() => {
                System.Diagnostics.Debug.WriteLine($"Tentativa #{retryCount}");
            });
        }
        #endregion

        #region UserRequests
        async Task<UserRoot> UserLoginRequest(User user)
        {
            var data = JsonConvert.SerializeObject(user);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await HttpClient.PostAsync($"{ApiBaseUrl}/sessions", content);

            if (response.IsSuccessStatusCode)
            {
                var repost = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserRoot>(repost);
            }
            else
                return null;
        }

        async Task<bool> UserPutRequest(User user, string route)
        {
            var data = JsonConvert.SerializeObject(user);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await HttpClient.PutAsync($"{ApiBaseUrl}/users{route}", content);

            return response.IsSuccessStatusCode;
        }

        async Task<ObservableCollection<User>> UsersGetRequest(string route)
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiBaseUrl}/{route}");

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
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiBaseUrl}/{route}");

            if (response.IsSuccessStatusCode)
            {
                var repost = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(repost);
            }
            else
                return null;

        }
        #endregion

        #region UserMethod
        public async Task<User> UserLogin(User user)
        {
            Token token = new Token();
            UserRoot userRoot = new UserRoot();

            try
            {
                //string url = ApiBaseUrl + "login";
                //var data = JsonConvert.SerializeObject(usuario);
                //var content = new StringContent(data, Encoding.UTF8, "application/json");
                //response = await HttpClient.PostAsync(url, content);

                //if (response.IsSuccessStatusCode)
                //{
                //    var repost = await response.Content.ReadAsStringAsync();
                //}
                //else
                //{
                //    usuarioLogin = null;
                //}

                var func = new Func<Task<UserRoot>>(() => UserLoginRequest(user));
                userRoot = await _networkService.Retry(func, 3, OnRetry);
                token.SetToken(userRoot.Token);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userRoot.User;

        }

        public async Task<bool> PutAsync(User user)
        {

            bool put = false;

            try
            {
                //Usuario.Usuario_Update usuario_Update = new Usuario.Usuario_Update
                //{
                //    Email = usuario.Email,
                //    Id = usuario.Id,
                //    Id_nivel_usuario = usuario.Id_nivel_usuario,
                //    Id_empresa = usuario.Id_empresa,
                //    Imagem = usuario.Imagem,
                //    Name = usuario.Name,
                //};

                //await Policy
                //      .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                //      .WaitAndRetryAsync
                //      (
                //          retryCount: 3,
                //          sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                //          onRetry: (ex, time) =>
                //          {
                //              Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                //          }
                //      )
                //      .ExecuteAsync(async () =>
                //      {
                //          Uri uri = new Uri(ApiBaseUrl.ToString() + "usuario_up/" + usuario.Id);
                //          var data = JsonConvert.SerializeObject(usuario_Update);
                //          StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                //          HttpResponseMessage response = await HttpClient.PostAsync(uri, content);

                //          put = response.IsSuccessStatusCode;
                //      });

                var func = new Func<Task<bool>>(() => UserPutRequest(user, null));
                put = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return put;

        }

        public async Task<ObservableCollection<User>> GetAsyncUsers(int? CompanyId)
        {

            ObservableCollection<User> users = new ObservableCollection<User>();

            try
            {
                //await Policy
                //      .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                //      .WaitAndRetryAsync
                //      (
                //          retryCount: 3,
                //          sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                //          onRetry: (ex, time) =>
                //          {
                //              Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                //          }
                //      )
                //      .ExecuteAsync(async () =>
                //      {
                //          Uri uri = new Uri(ApiBaseUrl.ToString() + "usuarios");
                //          var response = await HttpClient.GetStringAsync(uri);
                //          usuarios = JsonConvert.DeserializeObject<ObservableCollection<Usuario>>(response);
                //      });

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

                //await Policy
                //     .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                //     .WaitAndRetryAsync
                //     (
                //         retryCount: 3,
                //         sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                //         onRetry: (ex, time) =>
                //         {
                //             Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                //         }
                //     )
                //     .ExecuteAsync(async () =>
                //     {
                //         Uri uri = new Uri(ApiBaseUrl.ToString() + "usuarios");
                //         var response = await HttpClient.GetStringAsync(uri);
                //         usuarios = JsonConvert.DeserializeObject<ObservableCollection<Usuario>>(response);
                //     });

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

                //await Policy
                //     .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                //     .WaitAndRetryAsync
                //     (
                //         retryCount: 3,
                //         sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                //         onRetry: (ex, time) =>
                //         {
                //             Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                //         }
                //     )
                //     .ExecuteAsync(async () =>
                //     {
                //         Uri uri = new Uri(ApiBaseUrl.ToString() + "usuarios/" + Id);
                //         var response = await HttpClient.GetStringAsync(uri);
                //         usuario = JsonConvert.DeserializeObject<Usuario>(response);
                //     });

                var func = new Func<Task<User>>(() => UserGetRequest($"users/{Id}"));
                user = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return user;

        }
        #endregion

        #region EnvironmentRequests
        async Task<bool> EnvironmentPostRequest(Environment environment)
        {
            var data = JsonConvert.SerializeObject(environment);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await HttpClient.PostAsync($"{ApiBaseUrl}/Environments", content);

            return response.IsSuccessStatusCode;

        }

        async Task<bool> EnvironmentPutRequest(Environment environment)
        {
            var data = JsonConvert.SerializeObject(environment);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await HttpClient.PutAsync($"{ApiBaseUrl}/Environments", content);

            return response.IsSuccessStatusCode;

        }

        async Task<bool> EnvironmentDeleteRequest(int? Id)
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiBaseUrl}/Environments/{Id}");

            return response.IsSuccessStatusCode;

        } 

        async Task<ObservableCollection<Environment>> EnvironmentGetRequest(int? CompanyId)
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiBaseUrl}/{CompanyId}/Environments");

            if (response.IsSuccessStatusCode)
            {
                var repost = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<Environment>>(repost);
            }
            else
                return null;

        } 
        #endregion

        public async Task<bool> PostAsync(Environment environment)
        {

            bool post = false;

            try
            {

                //await Policy
                //     .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                //     .WaitAndRetryAsync
                //     (
                //         retryCount: 3,
                //         sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                //         onRetry: (ex, time) =>
                //         {
                //             Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                //         }
                //     )
                //     .ExecuteAsync(async () =>
                //     {
                //         Uri uri = new Uri(ApiBaseUrl + "ambientes");
                //         var data = JsonConvert.SerializeObject(ambiente);
                //         var content = new StringContent(data, Encoding.UTF8, "application/json");
                //         HttpResponseMessage response = await HttpClient.PostAsync(uri, content);

                //         set = response.IsSuccessStatusCode;
                //     });

                var func = new Func<Task<bool>>(() => EnvironmentPostRequest(environment));
                post = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return post;

        }

        public async Task<bool> DeleteAsync(Environment environment)
        {

            bool delete = false;

            try
            {

                //await Policy
                //     .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                //     .WaitAndRetryAsync
                //     (
                //         retryCount: 3,
                //         sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                //         onRetry: (ex, time) =>
                //         {
                //             Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                //         }
                //     )
                //     .ExecuteAsync(async () =>
                //     {
                //         string url = ApiBaseUrl + "ambiente/" + ambiente.Id;
                //         var response = await HttpClient.GetAsync(url);

                //         delete = response.IsSuccessStatusCode;
                //     });

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

                //Ambiente_Update ambiente_Update = new Ambiente_Update
                //{
                //    Id = ambiente.Id,
                //    Id_empresa = ambiente.Id_empresa,
                //    Nome_ambiente = ambiente.Nome_ambiente
                //};

                //await Policy
                //    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                //    .WaitAndRetryAsync
                //    (
                //        retryCount: 3,
                //        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                //        onRetry: (ex, time) =>
                //        {
                //            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                //        }
                //    )
                //    .ExecuteAsync(async () =>
                //    {
                //        Uri uri = new Uri(ApiBaseUrl.ToString() + "ambiente_up/" + ambiente.Id);
                //        var data = JsonConvert.SerializeObject(ambiente_Update);
                //        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                //        HttpResponseMessage response = await HttpClient.PostAsync(uri, content);

                //        put = response.IsSuccessStatusCode;
                //    });

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

                //await Policy
                //    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                //    .WaitAndRetryAsync
                //    (
                //        retryCount: 3,
                //        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                //        onRetry: (ex, time) =>
                //        {
                //            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                //        }
                //    )
                //    .ExecuteAsync(async () =>
                //    {
                //        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/ambientes");
                //        string response = await HttpClient.GetStringAsync(uri);
                //        ambientes = JsonConvert.DeserializeObject<ObservableCollection<Ambiente>>(response);
                //    });

                var func = new Func<Task<ObservableCollection<Environment>>>(() => EnvironmentGetRequest(CompanyId));
                environments = await _networkService.Retry(func, 3, OnRetry);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return environments;

        }

    }
}
