using HeritageV04.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeritageV04.Services.Abstractions
{
    public interface IHeritageAPIService
    {

        #region UserMethods
        Task<User> UserLogin(User user);

        Task<bool> PutAsync(User user);

        Task<ObservableCollection<User>> GetAsyncUsers(int? CompanyId);

        Task<ObservableCollection<User>> GetAsyncUsers();

        Task<User> GetAsyncUser(int? Id);
        #endregion

        #region EnvironmentMethods
        Task<bool> PostAsync(Environment environment);

        Task<bool> DeleteAsync(Environment environment);

        Task<bool> PutAsync(Environment environment);

        Task<ObservableCollection<Environment>> GetAsyncEnvironments(int? CompanyId);

        Task<Environment> GetAsyncEnvironment(int? CompanyId, string EnvironmentName);
        #endregion

        #region HeritageMethods
        Task<bool> PostAsync(Heritage heritage);

        Task<bool> DeleteAsync(int? Id);

        Task<bool> PutAsync(Heritage heritage);

        Task<ObservableCollection<Heritage>> GetAsyncHeritages(int? CompanyId);

        Task<ObservableCollection<Heritage>> GetAsyncHeritages(int? EnvironmentId, int? CompanyId); 
        #endregion

    }
}
