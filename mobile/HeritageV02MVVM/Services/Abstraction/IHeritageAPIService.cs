using HeritageV02MVVM.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeritageV02MVVM.Services.Abstraction
{
    public interface IHeritageAPIService
    {

        #region Usuario

        Task<bool> SetAsync(Usuario usuario);

        Task<Usuario> Login(Usuario usuario);

        Task<bool> DeleteAsync(Usuario usuario);

        Task<Usuario> Refresh(Usuario usuario);

        Task<Usuario> Me(Usuario usuario);

        Task<bool> PutAsync(Usuario usuario);

        Task<ObservableCollection<Usuario>> GetAsyncUsuarios(int? Id_empresa);

        Task<ObservableCollection<Usuario>> GetAsyncUsuarios();

        Task<Usuario> GetAsyncUsuario(int? Id);

        ObservableCollection<Usuario> OrganizeAsyncUsuario(ObservableCollection<Usuario> usuarios);

        #endregion

        #region Ambiente

        Task<bool> SetAsync(Ambiente ambiente);

        Task<bool> DeleteAsync(Ambiente ambiente);

        Task<bool> PutAsync(Ambiente ambiente);

        Task<ObservableCollection<Ambiente>> GetAsyncAmbientes(int? Id_empresa);

        Task<ObservableCollection<Ambiente>> GetAsyncAmbientes(int? Id_empresa, int? Id_usuario);

        Task<Ambiente> GetAsyncAmbiente(int? Id);

        Task<ObservableCollection<Ambiente>> OrganizeAsyncAmbientes(ObservableCollection<Ambiente> ambientes, int? Id_empresa);

        Task<string> ValidationAsyncAmbiente(Ambiente ambiente, int? Id_empresa);

        #endregion

        #region Patrimônio

        Task<bool> SetAsync(Patrimonio patrimonio);

        Task<bool> DeleteAsync(Patrimonio patrimonio);

        Task<bool> PutAsync(Patrimonio patrimonio);

        Task<ObservableCollection<Patrimonio>> GetAsyncPatrimonios(int? Id_empresa);

        Task<ObservableCollection<Patrimonio>> OrganizeAsyncPatrimonio(ObservableCollection<Patrimonio> patrimonios, int? Id_empresa);

        Task<Patrimonio> GetAsyncPatrimonio(int Id);

        Task<ObservableCollection<Patrimonio>> GetAsyncPatrimonios(int Id_ambiente, int? Id_empresa);

        Task<string> ValidationAsyncPatrimonio(Patrimonio patrimonio, int? Id_empresa);

        #endregion

        #region Histórico

        Task<bool> SetAsync(Historico historico);

        Task<ObservableCollection<Historico>> GetAsyncHistoricos(int? Id_empresa);

        Task<ObservableCollection<Historico.Manutencao>> GetAsyncManutencoes(int? Id_empresa);

        Task<ObservableCollection<Historico.Movimentacao>> GetAsyncMovimentacoes(int? Id_empresa);

        Task<ObservableCollection<Historico>> GetAsyncHistoricosAmbiente(int? Id_empresa, int? Id_ambiente);

        Task<ObservableCollection<Historico>> GetAsyncHistoricosPatrimonio(int? Id_empresa, int? Id_patrimonio);

        Task<ObservableCollection<Historico.Manutencao>> GetAsyncManutencoesPatrimonio(int? Id_empresa, int? Id_patrimonio);

        Task<ObservableCollection<Historico.Manutencao>> GetAsyncManutencoesAmbiente(int? Id_empresa, int? Id_ambiente);

        Task<ObservableCollection<Historico.Movimentacao>> GetAsyncMovimentacoesPatrimonio(int? Id_empresa, int? Id_patrimonio);

        Task<ObservableCollection<Historico.Movimentacao>> GetAsyncMovimentacoesAmbiente(int? Id_empresa, int? Id_ambiente);

        #endregion

    }
}
