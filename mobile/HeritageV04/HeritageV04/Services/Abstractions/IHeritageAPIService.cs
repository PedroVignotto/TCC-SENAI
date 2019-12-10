using HeritageV04.Models;
using HeritageV04.Utilities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeritageV04.Services.Abstractions
{
    public interface IHeritageAPIService
    {

        #region UserMethods
        Task<User> UserLogin(User user);

        Task<Repost> PutAsync(User user);

        Task<ObservableCollection<User>> GetAsyncUsers(int? CompanyId);

        Task<ObservableCollection<User>> GetAsyncUsers();

        Task<User> GetAsyncUser(int? Id);

        Task<Repost> AddUserAsync(User user);

        Task<bool> DeleteAsync(User user);

        #endregion

        #region EnvironmentMethods
        Task<Repost> PostAsync(Environment environment);

        Task<bool> DeleteAsync(Environment environment);

        Task<bool> PutAsync(Environment environment);

        Task<ObservableCollection<Environment>> GetAsyncEnvironments(int? CompanyId);

        Task<Environment> GetAsyncEnvironment(int? CompanyId, string EnvironmentName);

        #endregion

        #region HeritageMethods
        Task<Repost> PostAsync(Heritage heritage);

        Task<bool> PutAsync(Heritage heritage);

        Task<ObservableCollection<Heritage>> GetAsyncHeritages(int? CompanyId);

        Task<ObservableCollection<Heritage>> GetAsyncHeritages(int? EnvironmentId, int? CompanyId);

        Task<bool> DeleteAsync(Heritage heritage);

        Task<bool> VerificationAsync(int? EnvironmentId);
        #endregion

        Task<ObservableCollection<Historic>> GetAsyncHistorics(int? CompanyId);

        Task<Repost> PostAsync(Maintenance maintenance);

    }
}
