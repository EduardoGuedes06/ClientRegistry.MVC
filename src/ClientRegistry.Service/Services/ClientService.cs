using ClientRegistry.Domain;
using ClientRegistry.Domain.Interfaces;
using ClientRegistry.Domain.Models;

namespace ClientRegistry.Service.Services
{
    public class ClientService : BaseService, IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository, INotificator notificator)
            : base(notificator)
        {
            _clientRepository = clientRepository;
        }

        public async Task<PagedResult<Client>> GetPaged(string? search, int page, int pageSize)
        {
            return await _clientRepository.GetPaged(search, page, pageSize);
        }

        public async Task<Client?> GetById(Guid id)
        {
            return await _clientRepository.ObterPorId(id);
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _clientRepository.ObterTodos();
        }

        public async Task Post(Client client)
        {

            if (await _clientRepository.Buscar(c => c.Document == client.Document).ContinueWith(t => t.Result.Any()))
            {
                Notificar("Já existe um cliente com este documento cadastrado.");
                return;
            }

            await _clientRepository.Adicionar(client);
        }

        public async Task Update(Client client)
        {
            if (await _clientRepository.Buscar(c => c.Document == client.Document && c.Id != client.Id).ContinueWith(t => t.Result.Any()))
            {
                Notificar("Já existe um cliente com este documento cadastrado.");
                return;
            }

            await _clientRepository.Atualizar(client);
        }

        public async Task Remove(Guid id)
        {
            var client = await _clientRepository.ObterPorId(id);
            if (client == null)
            {
                Notificar("Cliente não encontrado.");
                return;
            }

            await _clientRepository.Remover(id);
        }

        public void Dispose()
        {
            _clientRepository?.Dispose();
        }
    }
}
