using System;

namespace CargaImagensNASA.Models
{
    public class ParametrosExecucao
    {
        public string CodDataReferencia { get; set; }
        public DateTime DataReferencia { get; set; }
        public DateTime Inicio { get; set; } = DateTime.Now;
    }
}