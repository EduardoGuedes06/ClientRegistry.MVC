using ClientRegistry.Domain.Models;
using ClientRegistry.Domain.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClientRegistry.Domain.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<CadastroPorDiaTipo> GetCadastroPorDiaTipo(DateTime? startDate, DateTime? endDate);
        Task<CadastroPorDia> GetCadastrosPorDia(DateTime? startDate, DateTime? endDate);
        Task<EvolucaoCadastros> GetEvolucaoCadastros(DateTime? startDate, DateTime? endDate);
        Task<PagedResult<Client>> GetPaged(string? search, int pageSize, int? page = null);
        Task<ProporcaoTipoPessoa> GetProporcaoTipoPessoa(DateTime? startDate, DateTime? endDate);
    }
}
