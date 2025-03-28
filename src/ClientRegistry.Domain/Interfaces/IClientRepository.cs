using ClientRegistry.Domain.Models;
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
        Task<PagedResult<Client>> GetPaged(string? search, int pageSize, int? page = null);
    }
}
