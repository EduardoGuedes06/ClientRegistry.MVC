using ClientRegistry.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClientRegistry.Domain.Interfaces
{
    public interface IPagedResultRepository<T> where T : class
    {
        Task<PagedResult<T>> GetPagedResult(IQueryable<T> query, int pageSize, int? page = null);
    }


}
