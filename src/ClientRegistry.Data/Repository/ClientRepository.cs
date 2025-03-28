using ClientRegistry.Data.Context;
using ClientRegistry.Domain.Interfaces;
using ClientRegistry.Domain.Models;
using ClientRegistry.Domain.Models.Data;
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

        #region Consulta
        public async Task<PagedResult<Client>> GetPaged(string? search, int pageSize, int? page = null)
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

            return await _pagedResultRepository.GetPagedResult(query, pageSize, page);
        }

        public async Task<CadastroPorDia> GetCadastrosPorDia(DateTime? startDate, DateTime? endDate)
        {
            IQueryable<Client> query = Db.Client.AsNoTracking();
            if (startDate.HasValue) query = query.Where(c => c.RegisterDateTime >= startDate.Value);
            if (endDate.HasValue) query = query.Where(c => c.RegisterDateTime <= endDate.Value);

            var result = await query
                .GroupBy(c => c.RegisterDateTime.Date)
                .Select(g => new
                {
                    Dia = g.Key, // Mantenha como DateTime
                    Count = g.Count()
                })
                .OrderBy(x => x.Dia)
                .ToListAsync();

            return new CadastroPorDia
            {
                Dias = result.Select(r => r.Dia.ToString("dd/MM")).ToList(), // Converta para string aqui
                Cadastros = result.Select(r => r.Count).ToList()
            };
        }

        public async Task<ProporcaoTipoPessoa> GetProporcaoTipoPessoa(DateTime? startDate, DateTime? endDate)
        {
            IQueryable<Client> query = Db.Client.AsNoTracking();
            if (startDate.HasValue) query = query.Where(c => c.RegisterDateTime >= startDate.Value);
            if (endDate.HasValue) query = query.Where(c => c.RegisterDateTime <= endDate.Value);

            var totalPF = await query.CountAsync(c => c.Type == "PF");
            var totalPJ = await query.CountAsync(c => c.Type == "PJ");

            return new ProporcaoTipoPessoa
            {
                PessoaFisica = totalPF,
                PessoaJuridica = totalPJ
            };
        }

        public async Task<EvolucaoCadastros> GetEvolucaoCadastros(DateTime? startDate, DateTime? endDate)
        {
            IQueryable<Client> query = Db.Client.AsNoTracking();
            if (startDate.HasValue) query = query.Where(c => c.RegisterDateTime >= startDate.Value);
            if (endDate.HasValue) query = query.Where(c => c.RegisterDateTime <= endDate.Value);

            var cadastrosPorDia = await query
                .GroupBy(c => c.RegisterDateTime.Date)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Dia = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            var dias = new List<string>();
            var acumulado = new List<int>();
            int soma = 0;

            foreach (var item in cadastrosPorDia)
            {
                soma += item.Count;
                dias.Add(item.Dia.ToString("dd/MM"));
                acumulado.Add(soma);
            }

            return new EvolucaoCadastros
            {
                Dias = dias,
                Acumulado = acumulado
            };
        }

        public async Task<CadastroPorDiaTipo> GetCadastroPorDiaTipo(DateTime? startDate, DateTime? endDate)
        {
            IQueryable<Client> query = Db.Client.AsNoTracking();
            if (startDate.HasValue) query = query.Where(c => c.RegisterDateTime >= startDate.Value);
            if (endDate.HasValue) query = query.Where(c => c.RegisterDateTime <= endDate.Value);

            var result = await query
                .GroupBy(c => new { c.RegisterDateTime.Date, c.Type })
                .Select(g => new
                {
                    Dia = g.Key.Date,
                    Tipo = g.Key.Type,
                    Count = g.Count()
                })
                .ToListAsync();

            var dias = result.Select(r => r.Dia.ToString("dd/MM")).Distinct().OrderBy(d => d).ToList(); // Converta para string aqui
            var pessoaFisica = dias.Select(d => result.Where(r => r.Dia.ToString("dd/MM") == d && r.Tipo == "PF").Sum(r => r.Count)).ToList();
            var pessoaJuridica = dias.Select(d => result.Where(r => r.Dia.ToString("dd/MM") == d && r.Tipo == "PJ").Sum(r => r.Count)).ToList();

            return new CadastroPorDiaTipo
            {
                Dias = dias,
                PessoaFisica = pessoaFisica,
                PessoaJuridica = pessoaJuridica
            };
        }


        #endregion
    }
}
