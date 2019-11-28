using HeritageV02MVVM.Models;
using HeritageV02MVVM.Utilities;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static HeritageV02MVVM.Models.Ambiente;
using HeritageV02MVVM.Services.Abstraction;

namespace HeritageV02MVVM.Services
{
    public class HeritageAPIService : IHeritageAPIService
    {

        public static Uri ApiBaseUrl = new Uri("http://heritage-cloudstate.000webhostapp.com/api/");

        public HttpClient HttpClient;

        public HeritageAPIService()
        {

            HttpClient = new HttpClient
            {
                BaseAddress = ApiBaseUrl
            };
            HttpClient.DefaultRequestHeaders.ConnectionClose = false;

        }

        #region Usuario

        public async Task<bool> SetAsync(Usuario usuario)
        {

            bool set = false;

            try
            {
                await Policy
                       .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                       .WaitAndRetryAsync
                       (
                           retryCount: 3,
                           sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                           onRetry: (ex, time) =>
                           {
                               Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                           }
                       )
                       .ExecuteAsync(async () =>
                       {
                           Uri uri = new Uri(ApiBaseUrl + "usuarios");
                           var data = JsonConvert.SerializeObject(usuario);
                           var content = new StringContent(data, Encoding.UTF8, "application/json");
                           HttpResponseMessage response = await HttpClient.PostAsync(uri, content);

                           set = response.IsSuccessStatusCode;
                       });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return set;

        }

        public async Task<Usuario> Login(Usuario usuario)
        {
            Usuario usuarioLogin = usuario;
            HttpResponseMessage response = null;

            try
            {

                await Policy
                       .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                       .WaitAndRetryAsync
                       (
                           retryCount: 3,
                           sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                           onRetry: (ex, time) =>
                           {
                               Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                           }
                       )
                       .ExecuteAsync(async () =>
                       {
                           string url = ApiBaseUrl + "login";
                           var data = JsonConvert.SerializeObject(usuario);
                           var content = new StringContent(data, Encoding.UTF8, "application/json");
                           response = await HttpClient.PostAsync(url, content);

                           if (response.IsSuccessStatusCode)
                           {
                               var repost = await response.Content.ReadAsStringAsync();
                               Token token = JsonConvert.DeserializeObject<Token>(repost);

                               usuarioLogin.Token = token.Token_acesso;

                               Desc_Token desc_token = new Desc_Token()
                               {
                                   Token = token.Token_acesso,
                               };
                           }
                           else
                           {
                               usuarioLogin = null;
                           }
                       });

                if (response.IsSuccessStatusCode)
                {
                    await Policy
                           .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                           .WaitAndRetryAsync
                           (
                                retryCount: 3,
                                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                onRetry: (ex, time) =>
                                {
                                    Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                                }
                           )
                           .ExecuteAsync(async () =>
                           {
                               Desc_Token desc_Token = new Desc_Token
                               {
                                   Token = usuarioLogin.Token
                               };

                               string url = ApiBaseUrl + "me";
                               var data = JsonConvert.SerializeObject(desc_Token);
                               var content = new StringContent(data, Encoding.UTF8, "application/json");
                               response = await HttpClient.PostAsync(url, content);

                               if (response.IsSuccessStatusCode)
                               {
                                   var repost = await response.Content.ReadAsStringAsync();
                                   usuarioLogin = JsonConvert.DeserializeObject<Usuario>(repost);
                                   usuarioLogin.Token = desc_Token.Token;
                               }
                               else
                               {
                                   usuarioLogin = null;
                               }
                           });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return usuarioLogin;

        }

        public async Task<bool> DeleteAsync(Usuario usuario)
        {

            bool delete = false;

            try
            {

                await Policy
                      .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                      .WaitAndRetryAsync
                      (
                          retryCount: 3,
                          sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                          onRetry: (ex, time) =>
                          {
                              Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                          }
                      )
                      .ExecuteAsync(async () =>
                      {
                          string url = ApiBaseUrl + "usuario/" + usuario.Id;
                          HttpResponseMessage response = await HttpClient.GetAsync(url);
                          delete = response.IsSuccessStatusCode;
                      });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return delete;

        }

        public async Task<Usuario> Refresh(Usuario usuario)
        {

            HttpResponseMessage response = null;

            try
            {

                await Policy
                      .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                      .WaitAndRetryAsync
                      (
                          retryCount: 3,
                          sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                          onRetry: (ex, time) =>
                          {
                              Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                          }
                      )
                      .ExecuteAsync(async () =>
                      {
                          HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                          Uri uri = new Uri(ApiBaseUrl + "refresh");
                          var data = JsonConvert.SerializeObject(usuario);
                          var content = new StringContent(data, Encoding.UTF8, "application/json");
                          response = await HttpClient.PostAsync(uri, content);

                          if (response.IsSuccessStatusCode)
                          {
                              var repost = await response.Content.ReadAsStringAsync();
                              Token token = JsonConvert.DeserializeObject<Token>(repost);

                              Desc_Token desc_token = new Desc_Token()
                              {
                                  Token = token.Token_acesso,
                              };

                              usuario.Token = desc_token.Token;
                          }
                          else
                              usuario = null;
                      });

                await Policy
                           .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                           .WaitAndRetryAsync
                           (
                                retryCount: 3,
                                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                onRetry: (ex, time) =>
                                {
                                    Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                                }
                           )
                           .ExecuteAsync(async () =>
                           {
                               HttpClient.DefaultRequestHeaders.Authorization = null;

                               Desc_Token desc_Token = new Desc_Token
                               {
                                   Token = usuario.Token
                               };

                               string url = ApiBaseUrl + "me";
                               var data = JsonConvert.SerializeObject(desc_Token);
                               var content = new StringContent(data, Encoding.UTF8, "application/json");
                               response = await HttpClient.PostAsync(url, content);

                               if (response.IsSuccessStatusCode)
                               {
                                   var repost = await response.Content.ReadAsStringAsync();
                                   usuario = JsonConvert.DeserializeObject<Usuario>(repost);
                                   usuario.Token = desc_Token.Token;
                               }
                               else
                                   usuario = null;
                           });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return usuario;

        }

        public async Task<Usuario> Me(Usuario usuario)
        {

            HttpResponseMessage response = null;

            try
            {

                await Policy
                      .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                      .WaitAndRetryAsync
                      (
                          retryCount: 3,
                          sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                          onRetry: (ex, time) =>
                          {
                              Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                          }
                      )
                      .ExecuteAsync(async () =>
                      {
                          Desc_Token desc_Token = new Desc_Token
                          {
                              Token = usuario.Token
                          };

                          string url = ApiBaseUrl + "me";
                          var data = JsonConvert.SerializeObject(desc_Token);
                          var content = new StringContent(data, Encoding.UTF8, "application/json");
                          response = await HttpClient.PostAsync(url, content);

                          if (response.IsSuccessStatusCode)
                          {
                              var repost = await response.Content.ReadAsStringAsync();
                              usuario = JsonConvert.DeserializeObject<Usuario>(repost);
                              usuario.Token = usuario.Token;
                          }
                          else
                          {
                              usuario = null;
                          }
                      });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return usuario;

        }

        public async Task<bool> PutAsync(Usuario usuario)
        {

            bool put = false;

            try
            {
                Usuario.Usuario_Update usuario_Update = new Usuario.Usuario_Update
                {
                    Email = usuario.Email,
                    Id = usuario.Id,
                    Id_nivel_usuario = usuario.Id_nivel_usuario,
                    Id_empresa = usuario.Id_empresa,
                    Imagem = usuario.Imagem,
                    Name = usuario.Name,
                };

                await Policy
                      .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                      .WaitAndRetryAsync
                      (
                          retryCount: 3,
                          sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                          onRetry: (ex, time) =>
                          {
                              Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                          }
                      )
                      .ExecuteAsync(async () =>
                      {
                          Uri uri = new Uri(ApiBaseUrl.ToString() + "usuario_up/" + usuario.Id);
                          var data = JsonConvert.SerializeObject(usuario_Update);
                          StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                          HttpResponseMessage response = await HttpClient.PostAsync(uri, content);

                          put = response.IsSuccessStatusCode;
                      });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return put;

        }

        public async Task<ObservableCollection<Usuario>> GetAsyncUsuarios(int? Id_empresa)
        {

            ObservableCollection<Usuario> usuarios = new ObservableCollection<Usuario>();

            try
            {
                await Policy
                      .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                      .WaitAndRetryAsync
                      (
                          retryCount: 3,
                          sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                          onRetry: (ex, time) =>
                          {
                              Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                          }
                      )
                      .ExecuteAsync(async () =>
                      {
                          Uri uri = new Uri(ApiBaseUrl.ToString() + "/usuarios");
                          var response = await HttpClient.GetStringAsync(uri);
                          usuarios = JsonConvert.DeserializeObject<ObservableCollection<Usuario>>(response);
                      });

            }
            catch (Exception )
            {
            }

            return OrganizeAsyncUsuario(usuarios);

        }

        public async Task<ObservableCollection<Usuario>> GetAsyncUsuarios()
        {

            ObservableCollection<Usuario> usuarios = new ObservableCollection<Usuario>();

            try
            {

                await Policy
                     .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                     .WaitAndRetryAsync
                     (
                         retryCount: 3,
                         sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                         onRetry: (ex, time) =>
                         {
                             Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                         }
                     )
                     .ExecuteAsync(async () =>
                     {
                         Uri uri = new Uri(ApiBaseUrl.ToString() + "usuarios");
                         var response = await HttpClient.GetStringAsync(uri);
                         usuarios = JsonConvert.DeserializeObject<ObservableCollection<Usuario>>(response);
                     });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return OrganizeAsyncUsuario(usuarios);

        }

        public async Task<Usuario> GetAsyncUsuario(int? Id)
        {

            Usuario usuario = new Usuario();

            try
            {

                await Policy
                     .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                     .WaitAndRetryAsync
                     (
                         retryCount: 3,
                         sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                         onRetry: (ex, time) =>
                         {
                             Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                         }
                     )
                     .ExecuteAsync(async () =>
                     {
                         Uri uri = new Uri(ApiBaseUrl.ToString() + "usuarios/" + Id);
                         var response = await HttpClient.GetStringAsync(uri);
                         usuario = JsonConvert.DeserializeObject<Usuario>(response);
                     });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return usuario;

        }

        public ObservableCollection<Usuario> OrganizeAsyncUsuario(ObservableCollection<Usuario> usuarios)
        {
            foreach (Usuario usuario in usuarios)
            {

                if (Convert.ToInt32(usuario.Id_nivel_usuario) == 1)
                {
                    usuario.Nivel_usuario = "Adm. Geral";
                }
                else if (Convert.ToInt32(usuario.Id_nivel_usuario) == 2)
                {
                    usuario.Nivel_usuario = "Gerenciador de Ambiente";
                }
                else if (Convert.ToInt32(usuario.Id_nivel_usuario) == 3)
                {
                    usuario.Nivel_usuario = "Suporte";
                }
            }

            return usuarios;
        }

        #endregion

        #region Ambiente

        public async Task<bool> SetAsync(Ambiente ambiente)
        {

            bool set = false;

            try
            {

                await Policy
                     .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                     .WaitAndRetryAsync
                     (
                         retryCount: 3,
                         sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                         onRetry: (ex, time) =>
                         {
                             Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                         }
                     )
                     .ExecuteAsync(async () =>
                     {
                         Uri uri = new Uri(ApiBaseUrl + "ambientes");
                         var data = JsonConvert.SerializeObject(ambiente);
                         var content = new StringContent(data, Encoding.UTF8, "application/json");
                         HttpResponseMessage response = await HttpClient.PostAsync(uri, content);

                         set = response.IsSuccessStatusCode;
                     });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return set;

        }

        public async Task<bool> DeleteAsync(Ambiente ambiente)
        {

            bool delete = false;

            try
            {

                await Policy
                     .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                     .WaitAndRetryAsync
                     (
                         retryCount: 3,
                         sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                         onRetry: (ex, time) =>
                         {
                             Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                         }
                     )
                     .ExecuteAsync(async () =>
                     {
                         string url = ApiBaseUrl + "ambiente/" + ambiente.Id;
                         var response = await HttpClient.GetAsync(url);

                         delete = response.IsSuccessStatusCode;
                     });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return delete;

        }

        public async Task<bool> PutAsync(Ambiente ambiente)
        {

            bool put = false;

            try
            {

                Ambiente_Update ambiente_Update = new Ambiente_Update
                {
                    Id = ambiente.Id,
                    Id_empresa = ambiente.Id_empresa,
                    Nome_ambiente = ambiente.Nome_ambiente
                };

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + "ambiente_up/" + ambiente.Id);
                        var data = JsonConvert.SerializeObject(ambiente_Update);
                        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await HttpClient.PostAsync(uri, content);

                        put = response.IsSuccessStatusCode;
                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return put;

        }

        public async Task<ObservableCollection<Ambiente>> GetAsyncAmbientes(int? Id_empresa)
        {

            ObservableCollection<Ambiente> ambientes = new ObservableCollection<Ambiente>();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/ambientes");
                        string response = await HttpClient.GetStringAsync(uri);
                        ambientes = JsonConvert.DeserializeObject<ObservableCollection<Ambiente>>(response);
                    });

            }
            catch (Exception )
            {
            }

            return await OrganizeAsyncAmbientes(ambientes, Id_empresa);

        }

        public async Task<ObservableCollection<Ambiente>> GetAsyncAmbientes(int? Id_empresa, int? Id_usuario)
        {

            ObservableCollection<Ambiente> ambientes = new ObservableCollection<Ambiente>();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + "ambientes");
                        string response = await HttpClient.GetStringAsync(uri);
                        ObservableCollection<Ambiente> ambientes_recebidos = JsonConvert.DeserializeObject<ObservableCollection<Ambiente>>(response);

                        foreach (Ambiente ambiente in ambientes_recebidos)
                        {
                            if (ambiente.Id_usuario == Id_usuario)
                            {
                                ambientes.Add(ambiente);
                            }
                        }

                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return await OrganizeAsyncAmbientes(ambientes, Id_empresa);

        }

        public async Task<Ambiente> GetAsyncAmbiente(int? Id)
        {

            Ambiente ambiente = new Ambiente();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + "ambientes/" + Id);
                        var response = await HttpClient.GetStringAsync(uri);
                        ambiente = JsonConvert.DeserializeObject<Ambiente>(response);
                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ambiente;

        }

        public async Task<ObservableCollection<Ambiente>> OrganizeAsyncAmbientes(ObservableCollection<Ambiente> ambientes, int? Id_empresa)
        {

            ObservableCollection<Usuario> usuarios = new ObservableCollection<Usuario>();
            ObservableCollection<Ambiente> ambientes_usuarios = new ObservableCollection<Ambiente>();

            try
            {

                await Policy
                      .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                      .WaitAndRetryAsync
                      (
                          retryCount: 3,
                          sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                          onRetry: (ex, time) =>
                          {
                              Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                          }
                      )
                      .ExecuteAsync(async () =>
                      {
                          Uri uri = new Uri(ApiBaseUrl.ToString()  + "/usuarios");
                          var response = await HttpClient.GetStringAsync(uri);
                          usuarios = JsonConvert.DeserializeObject<ObservableCollection<Usuario>>(response);

                          foreach (Ambiente ambiente in ambientes)
                          {
                              if (ambiente.Id_usuario != null)
                              {
                                  foreach (Usuario usuario in usuarios)
                                  {
                                      if (ambiente.Id_usuario == usuario.Id)
                                      {
                                          ambiente.Nome_usuario = usuario.Name;
                                      }
                                  }
                              }
                              else
                              {
                                  ambiente.Nome_usuario = "Sem gerenciador";
                              }

                              ambientes_usuarios.Add(ambiente);
                          }

                      });

            }
            catch (Exception)
            {
            }

            return ambientes_usuarios;

        }

        public async Task<string> ValidationAsyncAmbiente(Ambiente ambiente, int? Id_empresa)
        {
            string notice = null;
            ObservableCollection<Ambiente> ambientes = new ObservableCollection<Ambiente>();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/ambientes");
                        string response = await HttpClient.GetStringAsync(uri);
                        ambientes = JsonConvert.DeserializeObject<ObservableCollection<Ambiente>>(response);

                        foreach (Ambiente ambiente1 in ambientes)
                        {
                            if (ambiente1.Nome_ambiente != ambiente.Nome_ambiente)
                            {
                                notice = null;
                            }
                            else
                            {
                                notice = "Nome de ambiente";
                                break;
                            }
                        }

                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return notice;

        }

        #endregion

        #region Patrimônio

        public async Task<bool> SetAsync(Patrimonio patrimonio)
        {

            bool set = false;

            try
            {

                await Policy
                     .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                     .WaitAndRetryAsync
                     (
                         retryCount: 3,
                         sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                         onRetry: (ex, time) =>
                         {
                             Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                         }
                     )
                     .ExecuteAsync(async () =>
                     {
                         Uri uri = new Uri(ApiBaseUrl + "patrimonios");
                         var data = JsonConvert.SerializeObject(patrimonio);
                         var content = new StringContent(data, Encoding.UTF8, "application/json");
                         HttpResponseMessage response = await HttpClient.PostAsync(uri, content);

                         set = response.IsSuccessStatusCode;
                     });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return set;

        }

        public async Task<bool> DeleteAsync(Patrimonio patrimonio)
        {

            bool delete = false;

            try
            {

                await Policy
                     .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                     .WaitAndRetryAsync
                     (
                         retryCount: 3,
                         sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                         onRetry: (ex, time) =>
                         {
                             Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                         }
                     )
                     .ExecuteAsync(async () =>
                     {
                         string url = ApiBaseUrl + "patrimonio/" + patrimonio.Id;
                         var response = await HttpClient.GetAsync(url);

                         delete = response.IsSuccessStatusCode;
                     });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return delete;

        }

        public async Task<bool> PutAsync(Patrimonio patrimonio)
        {

            bool put = false;

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        string url = ApiBaseUrl + "patrimonio_up/" + patrimonio.Id;
                        var data = JsonConvert.SerializeObject(patrimonio);
                        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await HttpClient.PostAsync(url, content);

                        put = response.IsSuccessStatusCode;
                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return put;

        }

        public async Task<ObservableCollection<Patrimonio>> GetAsyncPatrimonios(int? Id_empresa)
        {

            ObservableCollection<Patrimonio> patrimonios = new ObservableCollection<Patrimonio>();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/patrimonios/");
                        string response = await HttpClient.GetStringAsync(uri);
                        patrimonios = JsonConvert.DeserializeObject<ObservableCollection<Patrimonio>>(response);
                    });

            }
            catch (Exception)
            {
                
            }

            return await OrganizeAsyncPatrimonio(patrimonios, Id_empresa);

        }

        public async Task<ObservableCollection<Patrimonio>> OrganizeAsyncPatrimonio(ObservableCollection<Patrimonio> patrimonios, int? Id_empresa)
        {

            ObservableCollection<Ambiente> ambientes = new ObservableCollection<Ambiente>();

            try
            {

                await Policy
                   .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                   .WaitAndRetryAsync
                   (
                       retryCount: 3,
                       sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                       onRetry: (ex, time) =>
                       {
                           Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                       }
                   )
                   .ExecuteAsync(async () =>
                   {
                       Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/ambientes");
                       string response = await HttpClient.GetStringAsync(uri);
                       ambientes = JsonConvert.DeserializeObject<ObservableCollection<Ambiente>>(response);

                       foreach (Patrimonio patrimonio in patrimonios)
                       {
                           foreach (Ambiente ambiente in ambientes)
                           {
                               if (patrimonio.Id_ambiente == ambiente.Id)
                               {
                                   patrimonio.Nome_ambiente = ambiente.Nome_ambiente;
                               }
                               else if (patrimonio.Id_ambiente == null)
                               {
                                   patrimonio.Nome_ambiente = "Sem ambiente";
                               }
                           }
                       }
                   });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return patrimonios;

        }

        public async Task<Patrimonio> GetAsyncPatrimonio(int Id)
        {

            Patrimonio patrimonio = new Patrimonio();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + "patrimonios/" + Id);
                        var response = await HttpClient.GetStringAsync(uri);
                        patrimonio = JsonConvert.DeserializeObject<Patrimonio>(response);
                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return patrimonio;

        }

        public async Task<ObservableCollection<Patrimonio>> GetAsyncPatrimonios(int Id_ambiente, int? Id_empresa)
        {

            ObservableCollection<Patrimonio> patrimonios = new ObservableCollection<Patrimonio>();
            ObservableCollection<Patrimonio> patrimonios_ambiente = new ObservableCollection<Patrimonio>();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/patrimonios/");
                        string response = await HttpClient.GetStringAsync(uri);
                        patrimonios = JsonConvert.DeserializeObject<ObservableCollection<Patrimonio>>(response);

                        foreach (Patrimonio patrimonio in patrimonios)
                        {
                            if (patrimonio.Id_ambiente == Id_ambiente)
                            {
                                patrimonios_ambiente.Add(patrimonio);
                            }
                        }

                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return await OrganizeAsyncPatrimonio(patrimonios_ambiente, Id_empresa);

        }

        public async Task<string> ValidationAsyncPatrimonio(Patrimonio patrimonio, int? Id_empresa)
        {

            string notice = null;
            ObservableCollection<Patrimonio> patrimonios = new ObservableCollection<Patrimonio>();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/patrimonios/");
                        string response = await HttpClient.GetStringAsync(uri);
                        patrimonios = JsonConvert.DeserializeObject<ObservableCollection<Patrimonio>>(response);

                        foreach (Patrimonio patrimonio1 in patrimonios)
                        {
                            if (patrimonio1.Codigo_patrimonio != patrimonio.Codigo_patrimonio)
                            {
                                notice = null;
                            }
                            else
                            {
                                notice = "Código do patrimônio";
                                break;
                            }
                        }

                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return notice;

        }

        #endregion

        #region Historico

        public async Task<bool> SetAsync(Historico historico)
        {

            bool set = false;

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl + "historicos");
                        var data = JsonConvert.SerializeObject(historico);
                        var content = new StringContent(data, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await HttpClient.PostAsync(uri, content);

                        set = response.IsSuccessStatusCode;
                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return set;

        }

        public async Task<ObservableCollection<Historico>> GetAsyncHistoricos(int? Id_empresa)
        {

            ObservableCollection<Historico> historicos = new ObservableCollection<Historico>();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/historicos");
                        string response = await HttpClient.GetStringAsync(uri);
                        historicos = JsonConvert.DeserializeObject<ObservableCollection<Historico>>(response);
                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return historicos;

        }

        public async Task<ObservableCollection<Historico.Manutencao>> GetAsyncManutencoes(int? Id_empresa)
        {

            ObservableCollection<Historico> historicos = new ObservableCollection<Historico>();
            ObservableCollection<Patrimonio> patrimonios = new ObservableCollection<Patrimonio>();
            ObservableCollection<Historico.Manutencao> manutencoes = new ObservableCollection<Historico.Manutencao>();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/historicos");
                        string response = await HttpClient.GetStringAsync(uri);
                        historicos = JsonConvert.DeserializeObject<ObservableCollection<Historico>>(response);
                    });

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri_patrimonio = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/patrimonios/");
                        string response_patrimonio = await HttpClient.GetStringAsync(uri_patrimonio);
                        patrimonios = JsonConvert.DeserializeObject<ObservableCollection<Patrimonio>>(response_patrimonio);

                        foreach (Historico historico in historicos)
                        {
                            if (historico.Nome_historico == 2)
                            {
                                foreach (Patrimonio patrimonio in patrimonios)
                                {
                                    if (historico.Id_patrimonio == patrimonio.Id)
                                    {
                                        manutencoes.Add(new Historico.Manutencao() { Id = historico.Id, Criacao = historico.Criacao, Descricao = historico.Descricao, Id_ambiente = historico.Id_ambiente, Id_empresa = historico.Id_empresa, Id_patrimonio = historico.Id_patrimonio, Nome = patrimonio.Nome_patrimonio + " - " + historico.Criacao, Codigo_patrimonio = patrimonio.Codigo_patrimonio });
                                    }
                                }
                            }
                        }
                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return manutencoes;

        }

        public async Task<ObservableCollection<Historico.Movimentacao>> GetAsyncMovimentacoes(int? Id_empresa)
        {

            ObservableCollection<Historico> historicos = new ObservableCollection<Historico>();
            ObservableCollection<Patrimonio> patrimonios = new ObservableCollection<Patrimonio>();
            ObservableCollection<Historico.Movimentacao> movimentacoes = new ObservableCollection<Historico.Movimentacao>();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/historicos");
                        string response = await HttpClient.GetStringAsync(uri);
                        historicos = JsonConvert.DeserializeObject<ObservableCollection<Historico>>(response);

                    });

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri_patrimonio = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/patrimonios/");
                        string response_patrimonio = await HttpClient.GetStringAsync(uri_patrimonio);
                        patrimonios = JsonConvert.DeserializeObject<ObservableCollection<Patrimonio>>(response_patrimonio);

                        foreach (Historico historico in historicos)
                        {
                            if (historico.Nome_historico == 1)
                            {
                                foreach (Patrimonio patrimonio in patrimonios)
                                {
                                    if (historico.Id_patrimonio == patrimonio.Id)
                                    {
                                        movimentacoes.Add(new Historico.Movimentacao() { Id = historico.Id, Criacao = historico.Criacao, Id_ambiente = historico.Id_ambiente, Id_empresa = historico.Id_empresa, Id_patrimonio = historico.Id_patrimonio, Nome = patrimonio.Nome_patrimonio + " para " + historico.Local_destino });
                                    }
                                }

                            }
                        }

                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return movimentacoes;

        }

        public async Task<ObservableCollection<Historico>> GetAsyncHistoricosAmbiente(int? Id_empresa, int? Id_ambiente)
        {

            ObservableCollection<Historico> historicos = new ObservableCollection<Historico>();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/historicos/" + Id_ambiente + "/ambientes");
                        string response = await HttpClient.GetStringAsync(uri);
                        historicos = JsonConvert.DeserializeObject<ObservableCollection<Historico>>(response);
                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return historicos;

        }

        public async Task<ObservableCollection<Historico>> GetAsyncHistoricosPatrimonio(int? Id_empresa, int? Id_patrimonio)
        {

            ObservableCollection<Historico> historicos = new ObservableCollection<Historico>();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/historicos/" + Id_patrimonio + "/patrimonios");
                        string response = await HttpClient.GetStringAsync(uri);
                        historicos = JsonConvert.DeserializeObject<ObservableCollection<Historico>>(response);
                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return historicos;

        }

        public async Task<ObservableCollection<Historico.Manutencao>> GetAsyncManutencoesPatrimonio(int? Id_empresa, int? Id_patrimonio)
        {

            ObservableCollection<Historico> historicos = new ObservableCollection<Historico>();
            ObservableCollection<Historico.Manutencao> manutencoes = new ObservableCollection<Historico.Manutencao>();
            ObservableCollection<Patrimonio> patrimonios = new ObservableCollection<Patrimonio>();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/historicos/" + Id_patrimonio + "/patrimonios");
                        string response = await HttpClient.GetStringAsync(uri);
                        historicos = JsonConvert.DeserializeObject<ObservableCollection<Historico>>(response);
                    });

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri_patrimonio = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/patrimonios/");
                        string response_patrimonio = await HttpClient.GetStringAsync(uri_patrimonio);
                        patrimonios = JsonConvert.DeserializeObject<ObservableCollection<Patrimonio>>(response_patrimonio);

                        foreach (Historico historico in historicos)
                        {
                            if (historico.Nome_historico == 2)
                            {
                                foreach (Patrimonio patrimonio in patrimonios)
                                {
                                    if (historico.Id_patrimonio == patrimonio.Id)
                                    {
                                        manutencoes.Add(new Historico.Manutencao() { Id = historico.Id, Criacao = historico.Criacao, Descricao = historico.Descricao, Id_ambiente = historico.Id_ambiente, Id_empresa = historico.Id_empresa, Id_patrimonio = historico.Id_patrimonio, Nome = patrimonio.Nome_patrimonio + " - " + historico.Criacao, Codigo_patrimonio = patrimonio.Codigo_patrimonio });
                                    }
                                }
                            }
                        }

                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return manutencoes;
        }

        public async Task<ObservableCollection<Historico.Manutencao>> GetAsyncManutencoesAmbiente(int? Id_empresa, int? Id_ambiente)
        {

            ObservableCollection<Historico> historicos = new ObservableCollection<Historico>();
            ObservableCollection<Historico.Manutencao> manutencoes = new ObservableCollection<Historico.Manutencao>();
            ObservableCollection<Patrimonio> patrimonios = new ObservableCollection<Patrimonio>();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/historicos/");
                        string response = await HttpClient.GetStringAsync(uri);
                        historicos = JsonConvert.DeserializeObject<ObservableCollection<Historico>>(response);
                    });

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri_patrimonio = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/patrimonios/");
                        string response_patrimonio = await HttpClient.GetStringAsync(uri_patrimonio);
                        patrimonios = JsonConvert.DeserializeObject<ObservableCollection<Patrimonio>>(response_patrimonio);

                        foreach (Historico historico in historicos)
                        {
                            if (historico.Nome_historico == 2 && historico.Id_ambiente == Id_ambiente)
                            {
                                foreach (Patrimonio patrimonio in patrimonios)
                                {
                                    if (historico.Id_patrimonio == patrimonio.Id)
                                    {
                                        manutencoes.Add(new Historico.Manutencao() { Id = historico.Id, Criacao = historico.Criacao, Descricao = historico.Descricao, Id_ambiente = historico.Id_ambiente, Id_empresa = historico.Id_empresa, Id_patrimonio = historico.Id_patrimonio, Nome = patrimonio.Nome_patrimonio + " - " + historico.Criacao, Codigo_patrimonio = patrimonio.Codigo_patrimonio });
                                    }
                                }
                            }
                        }


                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return manutencoes;

        }

        public async Task<ObservableCollection<Historico.Movimentacao>> GetAsyncMovimentacoesPatrimonio(int? Id_empresa, int? Id_patrimonio)
        {

            ObservableCollection<Historico> historicos = new ObservableCollection<Historico>();
            ObservableCollection<Historico.Movimentacao> movimentacoes = new ObservableCollection<Historico.Movimentacao>();
            ObservableCollection<Patrimonio> patrimonios = new ObservableCollection<Patrimonio>();
            ObservableCollection<Ambiente> ambientes = new ObservableCollection<Ambiente>();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/historicos");
                        string response = await HttpClient.GetStringAsync(uri);
                        historicos = JsonConvert.DeserializeObject<ObservableCollection<Historico>>(response);
                    });

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/patrimonios/");
                        string response = await HttpClient.GetStringAsync(uri);
                        patrimonios = JsonConvert.DeserializeObject<ObservableCollection<Patrimonio>>(response);
                    });

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/ambientes");
                        string response = await HttpClient.GetStringAsync(uri);
                        ambientes = JsonConvert.DeserializeObject<ObservableCollection<Ambiente>>(response);

                        foreach (Historico historico in historicos)
                        {
                            if (historico.Nome_historico == 1)
                            {
                                foreach (Patrimonio patrimonio in patrimonios)
                                {
                                    if (Id_patrimonio == patrimonio.Id)
                                    {
                                        foreach (Ambiente ambiente in ambientes)
                                        {
                                            if (ambiente.Id == historico.Id_ambiente)
                                            {
                                                movimentacoes.Add(new Historico.Movimentacao() { Id = historico.Id, Criacao = historico.Criacao, Id_ambiente = historico.Id_ambiente, Id_empresa = historico.Id_empresa, Id_patrimonio = historico.Id_patrimonio, Nome = patrimonio.Nome_patrimonio + " para " + historico.Local_destino });
                                            }
                                        }

                                    }
                                }

                            }
                        }

                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return movimentacoes;

        }

        public async Task<ObservableCollection<Historico.Movimentacao>> GetAsyncMovimentacoesAmbiente(int? Id_empresa, int? Id_ambiente)
        {

            ObservableCollection<Historico> historicos = new ObservableCollection<Historico>();
            ObservableCollection<Historico.Movimentacao> movimentacoes = new ObservableCollection<Historico.Movimentacao>();
            ObservableCollection<Patrimonio> patrimonios = new ObservableCollection<Patrimonio>();
            ObservableCollection<Ambiente> ambientes = new ObservableCollection<Ambiente>();

            try
            {

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/historicos");
                        string response = await HttpClient.GetStringAsync(uri);
                        historicos = JsonConvert.DeserializeObject<ObservableCollection<Historico>>(response);
                    });

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/patrimonios/");
                        string response = await HttpClient.GetStringAsync(uri);
                        patrimonios = JsonConvert.DeserializeObject<ObservableCollection<Patrimonio>>(response);
                    });

                await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .WaitAndRetryAsync
                    (
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Uri uri = new Uri(ApiBaseUrl.ToString() + Id_empresa + "/ambientes");
                        string response = await HttpClient.GetStringAsync(uri);
                        ambientes = JsonConvert.DeserializeObject<ObservableCollection<Ambiente>>(response);

                        foreach (Historico historico in historicos)
                        {
                            if (historico.Nome_historico == 1)
                            {
                                foreach (Patrimonio patrimonio in patrimonios)
                                {
                                    if (historico.Id_patrimonio == patrimonio.Id)
                                    {
                                        if (historico.Id_ambiente == Id_ambiente)
                                        {
                                            foreach (Ambiente ambiente in ambientes)
                                            {
                                                if (ambiente.Id == historico.Id_ambiente)
                                                {
                                                    movimentacoes.Add(new Historico.Movimentacao() { Id = historico.Id, Criacao = historico.Criacao, Id_ambiente = historico.Id_ambiente, Id_empresa = historico.Id_empresa, Id_patrimonio = historico.Id_patrimonio, Nome = patrimonio.Nome_patrimonio + " para " + historico.Local_destino });
                                                }
                                            }

                                        }

                                    }
                                }

                            }
                        }

                    });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return movimentacoes;

        }

        #endregion

    }
}
