using AutoMapper;
using ClientRegistry.Domain;
using ClientRegistry.Domain.Interfaces;
using ClientRegistry.Domain.Models;
using ClientRegistry.MVC.Models;
using ClientRegistry.MVC.Models.Validation;
using ClientRegistry.MVC.Models.Validation.Utils;
using ClientRegistry.MVC.Models.Validations.Utils;
using Humanizer;
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

            var viewModel = new PagedClientViewModel
            {
                Clients = clientViewModels,
                Search = search,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)pagedClients.TotalCount / pageSize),
                TotalItems = pagedClients.TotalCount
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ClientList", viewModel);
            }

            return View(viewModel);
        }



        // GET: Tela de Cadastro/Edição
        public async Task<IActionResult> UpSert(Guid? id)
        {
            if (id == null)
                return View(new ClientViewModel());

            var client = await _clientService.GetById(id.Value);
            if (client == null)
                return NotFound();

            var clientViewModel = _mapper.Map<ClientViewModel>(client);
            return View(clientViewModel);
        }

        // POST: Salvar Cadastro/Edição
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpSert(ClientViewModel model)
        {
            var client = _mapper.Map<Client>(model);

            var documentWithNonNumeric = client.Document;
            client.Document = await ValidationUtils.RemoveNonNumericAsync(documentWithNonNumeric);

            var validator = new ClientValidation();
            var validationResult = await validator.ValidateAsync(client);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }

            client.Document = documentWithNonNumeric;

            if (client.Id == Guid.Empty)
            {
                await _clientService.Post(client);
            }
            else
            {
                await _clientService.Update(client);
            }

            if (OperacaoValida())
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = await _clientService.GetById(id);
            if (client == null)
            {
                return NotFound();
            }

            await _clientService.Remove(id);

            return RedirectToAction("Index", "Home");
        }


    }
}