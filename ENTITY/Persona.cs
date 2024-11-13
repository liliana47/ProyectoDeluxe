using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Persona
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string TipoDocumento { get; set; }
        public int NumeroDocumento { get; set; }
        public int Telefono { get; set; }
        public string CorreoElectronico { get; set; }

        public Persona(string nombre, string apellido, string tipoDocumento, int numeroDocumento, int telefono, string correoElectronico)
        {
            Nombre = nombre;
            Apellido = apellido;
            TipoDocumento = tipoDocumento;
            NumeroDocumento = numeroDocumento;
            Telefono = telefono;
            CorreoElectronico = correoElectronico;
        }
    }
}
