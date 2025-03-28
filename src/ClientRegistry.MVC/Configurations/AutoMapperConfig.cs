using AutoMapper;
using ClientRegistry.Domain.Models;
using ClientRegistry.Domain.Models.Data;
using ClientRegistry.MVC.Models;
using ClientRegistry.MVC.Models.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClientRegistry.MVC.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Client, ClientViewModel>().ReverseMap();

            #region Data

            CreateMap<CadastroPorDiaTipo, CadastroPorDiaTipoViewModel>().ReverseMap();
            CreateMap<EvolucaoCadastros, EvolucaoCadastrosViewModel>().ReverseMap();
            CreateMap<ProporcaoTipoPessoa, ProporcaoTipoPessoaViewModel>().ReverseMap();
            CreateMap<CadastroPorDia, CadastroPorDiaViewModel>().ReverseMap();
    
            #endregion

        }
    }
}
