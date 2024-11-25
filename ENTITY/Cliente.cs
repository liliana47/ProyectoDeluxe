using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Cliente 
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }

        public Cliente()
        {

        }

        public Cliente(string cedula, string nombre, string apellido, string direccion, string telefono, string correoElectronico)
        {
            Nombre = nombre;
            Apellido = apellido;
            Cedula = cedula;
            Direccion = direccion;
            Telefono = telefono;
            CorreoElectronico = correoElectronico;
        }
    }
}
