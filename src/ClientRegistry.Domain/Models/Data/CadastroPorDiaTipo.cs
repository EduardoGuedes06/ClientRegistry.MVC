using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRegistry.Domain.Models.Data
{
    public class CadastroPorDiaTipo
    {
        public List<string> Dias { get; set; }
        public List<int> PessoaFisica { get; set; }
        public List<int> PessoaJuridica { get; set; }
    }
}
