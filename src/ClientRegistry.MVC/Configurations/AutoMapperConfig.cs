using AutoMapper;
using ClientRegistry.Domain.Models;
using ClientRegistry.MVC.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClientRegistry.MVC.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Client, ClientViewModel>().ReverseMap();

        }
    }
}
