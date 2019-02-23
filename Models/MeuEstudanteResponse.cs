using System.Collections.Generic;

namespace SistemaControle.Models
{
    public class MeuEstudanteResponse
    {
        public double Porcentagem { get; set; }
        public List<MeuEstudante> Estudante { get; set; }
    }
}