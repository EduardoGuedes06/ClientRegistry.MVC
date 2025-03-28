namespace ClientRegistry.MVC.Models.Data
{
    public class CadastroPorDiaTipoViewModel
    {
        public List<string> Dias { get; set; }
        public List<int> PessoaFisica { get; set; }
        public List<int> PessoaJuridica { get; set; }
    }
}
