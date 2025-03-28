using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRegistry.Domain.Models.Data
{
    public class CadastroPorDia
    {
        public IEnumerable<string> Dias { get; set; }
        public IEnumerable<int> Cadastros { get; set; }
    }
}
