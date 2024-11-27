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

        public List<Cliente> ObtenerClientes()
        {
            return clienteDAL.ObtenerClientes();
        }

        public Cliente ObtenerClientePorCedula(string cedula)
        {
            return clienteDAL.ObtenerClientePorCedula(cedula);
        }
    }
}
