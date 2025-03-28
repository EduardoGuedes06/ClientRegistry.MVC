using AutoMapper;
using ClientRegistry.Domain.Interfaces;
using ClientRegistry.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClientRegistry.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;
        private readonly IClientService _clientService;

        public HomeController(IMapper mapper, ILogger<HomeController> logger, IClientService clientService)
        {
            _logger = logger;
            _clientService = clientService;
            _mapper = mapper;
        }

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


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
