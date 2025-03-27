using AutoMapper;
using ClientRegistry.Domain;
using ClientRegistry.Domain.Interfaces;
using ClientRegistry.Domain.Models;
using ClientRegistry.MVC.Models;
using Microsoft.AspNetCore.Mvc;
namespace ClientRegistry.MVC.Controllers
{
    public class ClientController : BaseController
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(IMapper mapper, IClientService clientService, INotificator notificator)
            : base(notificator)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        // GET: Listagem com Paginação e Filtro
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {

            var pagedClients = await _clientService.GetPaged(search, page, pageSize);

            var clientViewModels = _mapper.Map<IEnumerable<ClientViewModel>>(pagedClients.Items);

            var totalPages = (int)Math.Ceiling((double)pagedClients.TotalCount / pageSize);
            var viewModel = new PagedClientViewModel
            {
                Clients = clientViewModels,
                Search = search,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalItems = pagedClients.TotalCount
            };

            return View(viewModel);
        }


        // GET: Tela de Cadastro/Edição
        public async Task<IActionResult> UpSert(Guid? id)
        {
            if (id == null) return View(new ClientViewModel());

            var client = await _clientService.GetById(id.Value);
            if (client == null) return NotFound();

            var clientViewModel = _mapper.Map<ClientViewModel>(client);
            return View(clientViewModel);
        }

        // POST: Salvar Cadastro/Edição
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpSert(ClientViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _mapper.Map<Client>(model);

            if (model.Id == Guid.Empty)
                await _clientService.Post(client);
            else
                await _clientService.Update(client);

            if (OperacaoValida())
                return RedirectToAction(nameof(Index));

            return View(model);
        }
    }
}