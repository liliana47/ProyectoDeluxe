using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Impuesto
    {
        public string TipoImpuesto { get; set; }
        public double Porcentaje { get; set; }
        public double Valor { get; private set; }
        public Impuesto(string tipoImpuesto, double porcentaje)
        {
            TipoImpuesto = tipoImpuesto;
            Porcentaje = porcentaje;
            Valor = 0.0; 
        }

        public void CalcularImpuesto(double montoBase)
        {
            Valor = montoBase * (Porcentaje / 100);
        }

        public bool ValidarImpuesto()
        {
            return !string.IsNullOrEmpty(TipoImpuesto) && Porcentaje > 0;
        }
    }
}
