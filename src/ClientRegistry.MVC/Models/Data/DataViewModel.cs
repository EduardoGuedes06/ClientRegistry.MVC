namespace ClientRegistry.MVC.Models.Data
{
    public class DataViewModel
    {
        public CadastroPorDiaViewModel CadastroPorDia { get; set; }
        public ProporcaoTipoPessoaViewModel ProporcaoTipoPessoa { get; set; }
        public EvolucaoCadastrosViewModel EvolucaoCadastros { get; set; }
        public CadastroPorDiaTipoViewModel CadastroPorDiaTipo { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
