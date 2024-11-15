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

        public void AgregarCliente(string nombre, string apellido, string tipodocumento, int numerodocumento, int telefono, string correo)
        {
            Cliente cliente = new Cliente
            {
                Nombre = nombre,
                Apellido = apellido,
                TipoDocumento = tipodocumento,
                NumeroDocumento = numerodocumento,
                Telefono = telefono,
                CorreoElectronico = correo
            };

            clienteDAL.agregarCliente(cliente);

        }
    }
}
