using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Cliente : Persona
    {
        public Cliente(string nombre, string apellido, string tipoDocumento, int numeroDocumento, int telefono, string correoElectronico)
            : base(nombre, apellido, tipoDocumento, numeroDocumento, telefono, correoElectronico)
        {
        }
    }
}
