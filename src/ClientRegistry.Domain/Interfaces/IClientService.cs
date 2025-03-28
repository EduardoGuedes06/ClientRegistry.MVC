using ClientRegistry.Domain.Models;
using ClientRegistry.Domain.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRegistry.Domain.Interfaces
{
    public interface IClientService : IDisposable
    {
        Task Post(Client client);
        Task Update(Client client);
        Task Remove(Guid id);
        Task<IEnumerable<Client>> GetAll();
        Task<Client?> GetById(Guid id);
        Task<PagedResult<Client>> GetPaged(string? search, int page, int pageSize);
        Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> items);
        Task<IEnumerable<Client>> GetWhitoutPagination(string? search, int pageSize);
        Task<CadastroPorDia> GetCadastrosPorDia(DateTime? startDate, DateTime? endDate);
        Task<ProporcaoTipoPessoa> GetProporcaoTipoPessoa(DateTime? startDate, DateTime? endDate);
        Task<CadastroPorDiaTipo> GetCadastroPorDiaTipo(DateTime? startDate, DateTime? endDate);
        Task<EvolucaoCadastros> GetEvolucaoCadastros(DateTime? startDate, DateTime? endDate);
    }
}
