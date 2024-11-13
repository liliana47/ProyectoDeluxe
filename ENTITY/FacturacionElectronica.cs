using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class FacturaciónElectronica
    {
        public string UUID { get; set; }
        public string CodigoAutenticacion { get; set; }
        public DateTime FechaCertificacion { get; set; }
        public string CodigoQR { get; set; }

        public CertificadoDigital Certificado { get; set; }

        public void EnviarDian()
        {
            
        }

        public bool ValidarDatos()
        {
            return !string.IsNullOrEmpty(UUID) && !string.IsNullOrEmpty(CodigoAutenticacion);
        }
    }
}
