using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class CertificadoDigital
    {
        public string RutaCertificado { get; set; }
        public string Contraseña { get; set; }
        public DateTime FechaExpiracion { get; set; }

        public void CargarCertificado()
        {
      
        }
        public void FirmarXML()
        {

        }
        public bool ValidarCertificado()
        {
            return DateTime.Now < FechaExpiracion;
        }
    }
}
