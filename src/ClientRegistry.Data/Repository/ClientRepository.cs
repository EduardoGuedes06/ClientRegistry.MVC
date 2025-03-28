using ClientRegistry.Data.Context;
using ClientRegistry.Domain.Interfaces;
using ClientRegistry.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientRegistry.Data.Repository
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        private readonly IPagedResultRepository<Client> _pagedResultRepository;

        public ClientRepository(MeuDbContext context, IPagedResultRepository<Client> pagedResultRepository)
            : base(context)
        {
            _pagedResultRepository = pagedResultRepository;
        }

        public async Task<PagedResult<Client>> GetPaged(string? search, int page, int pageSize)
        {
            IQueryable<Client> query = Db.Client.AsNoTracking();
            query = query.Where(c => c.Active == true);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c =>
                    c.Name.Contains(search) ||
                    c.Document.Contains(search) ||
                    c.Phone.Contains(search) ||
                    c.Type.Contains(search));
            }

            return await _pagedResultRepository.GetPagedResult(query, page, pageSize);
        }
    }
}
