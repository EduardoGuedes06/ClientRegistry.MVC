using ClientRegistry.Data.Context;
using ClientRegistry.Domain.Interfaces;
using ClientRegistry.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientRegistry.Data.Repository
{
    public class PagedResultRepository<T> : IPagedResultRepository<T> where T : class
    {
        private readonly MeuDbContext _context;

        public PagedResultRepository(MeuDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<T>> GetPagedResult(IQueryable<T> query, int page, int pageSize)
        {
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>(items, totalCount, page, pageSize);
        }

    }


}
