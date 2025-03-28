using ClientRegistry.Domain;
using ClientRegistry.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClientRegistry.MVC.Controllers
{
    public abstract class BaseController : Controller
    {
        public readonly INotificator _notificator;

        protected BaseController(INotificator notificador)
        {
            _notificator = notificador;
        }

        protected bool OperacaoValida()
        {
            return !_notificator.HaveNotification();
        }

        protected bool ResponsePossuiErros(ResponseResult resposta)
        {
            if (resposta != null && resposta.Errors.Mensagens.Any())
            {
                foreach (var mensagem in resposta.Errors.Mensagens)
                {
                    ModelState.AddModelError(string.Empty, mensagem);
                }

                return true;
            }

            return false;
        }
    }
}
