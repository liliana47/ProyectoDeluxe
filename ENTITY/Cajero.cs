using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Cajero
    {
        public int CodigoCajero { get; set; }
        public string NombreCajero { get; set; }
        public Cajero(int codigoCajero, string nombreCajero)
        {
            CodigoCajero = codigoCajero;
            NombreCajero = nombreCajero;
        }
    }
}
