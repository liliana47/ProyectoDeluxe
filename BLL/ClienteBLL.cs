using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ClienteBLL
    {
        ClienteDAL clienteDAL = new ClienteDAL();

        public void AgregarCliente(string cedula, string nombre, string apellido, string direccion, string telefono, string correo)
        {
            ValidarCampoObligatorio(cedula, "Cedula");
            ValidarCampoObligatorio(nombre, "Nombre");
            ValidarCampoObligatorio(apellido, "Apellido");
            ValidarCampoObligatorio(direccion, "Direccion");
            ValidarCampoObligatorio(telefono, "Telefono");
            ValidarCampoObligatorio(correo, "Correo");

            if (clienteDAL.CedulaExiste(cedula))
            {
                throw new ArgumentException("La cédula ya está registrada.");
            }

            Cliente cliente = new Cliente
            {
                Cedula = cedula,
                Nombre = nombre,
                Apellido = apellido,
                Direccion = direccion,
                Telefono = telefono,
                CorreoElectronico = correo,
            };

            clienteDAL.agregarCliente(cliente);

        }

        private void ValidarCampoObligatorio(string valor, string nombreCampo)
        {
            if (string.IsNullOrEmpty(valor))
            {
                throw new ArgumentException($"El campo '{nombreCampo}' es obligatorio.");
            }
        }

        public class ClienteManager
        {
            public void ValidarCliente(Cliente cliente)
            {
                if (string.IsNullOrWhiteSpace(cliente.Cedula))
                    throw new Exception("La cédula es obligatoria.");
                if (string.IsNullOrWhiteSpace(cliente.Nombre))
                    throw new Exception("El nombre es obligatorio.");
                if (string.IsNullOrWhiteSpace(cliente.Apellido))
                    throw new Exception("El apellido es obligatorio.");
                if (!string.IsNullOrWhiteSpace(cliente.CorreoElectronico) && !cliente.CorreoElectronico.Contains("@"))
                    throw new Exception("El correo electrónico no es válido.");
            }
        }
    }
}
