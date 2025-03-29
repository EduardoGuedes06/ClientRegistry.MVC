using AutoMapper;
using ClientRegistry.Data.Repository;
using ClientRegistry.Domain;
using ClientRegistry.Domain.Interfaces;
using ClientRegistry.Domain.Models;
using ClientRegistry.Domain.Notification;
using ClientRegistry.MVC.Models;
using ClientRegistry.MVC.Models.Data;
using ClientRegistry.MVC.Models.Validation;
using ClientRegistry.MVC.Models.Validation.Utils;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
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
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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

            return View(viewModel);
        }

        // GET: Exportação em XLS
        public async Task<IActionResult> ExportToExcel(string? search,int page, int pageSize = 10)
        {
            var clients = await _clientService.GetPaged(search, page, pageSize);
            var excelFile = await _clientService.ExportToExcelAsync(clients.Items);
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.UtcNow}.xlsx");

        }

        // GET: Data
        public async Task<IActionResult> DataAsync(DateTime? startDate, DateTime? endDate)
        {
            startDate = startDate ?? new DateTime(DateTime.Now.Year, 1, 1);
            endDate = endDate ?? new DateTime(DateTime.Now.Year, 12, 31);

            var validator = new DateValidation();
            var validationResult = await validator.ValidateAsync(new DataViewModel
            {
                StartDate = startDate?.ToString("yyyy-MM-dd"),
                EndDate = endDate?.ToString("yyyy-MM-dd")
            });

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                startDate = new DateTime(DateTime.Now.Year, 1, 1);
                endDate = new DateTime(DateTime.Now.Year, 12, 31);
            }

            var cadastroPorDia = await _clientService.GetCadastrosPorDia(startDate, endDate);
            var proporcaoTipoPessoa = await _clientService.GetProporcaoTipoPessoa(startDate, endDate);
            var evolucaoCadastros = await _clientService.GetEvolucaoCadastros(startDate, endDate);
            var cadastroPorDiaTipo = await _clientService.GetCadastroPorDiaTipo(startDate, endDate);

            var model = new DataViewModel
            {
                CadastroPorDia = _mapper.Map<CadastroPorDiaViewModel>(cadastroPorDia),
                ProporcaoTipoPessoa = _mapper.Map<ProporcaoTipoPessoaViewModel>(proporcaoTipoPessoa),
                EvolucaoCadastros = _mapper.Map<EvolucaoCadastrosViewModel>(evolucaoCadastros),
                CadastroPorDiaTipo = _mapper.Map<CadastroPorDiaTipoViewModel>(cadastroPorDiaTipo),
                StartDate = startDate.Value.ToString("yyyy-MM-dd"),
                EndDate = endDate.Value.ToString("yyyy-MM-dd")
            };

            return View(model);
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
            if (!OperacaoValida())
            {
                var notifications = _notificator.GetNotifications();
                foreach (var notification in notifications)
                {
                    ModelState.AddModelError("", notification.Message);
                }
                return View(model);
            }
            if (OperacaoValida())
            {
                return RedirectToAction("Index", "Client");
            }
            return View(model);
        }

        //Delete
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

            return RedirectToAction("Index", "Client");
        }


    }
}