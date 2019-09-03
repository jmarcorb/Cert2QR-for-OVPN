using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cert2QR_for_OVPN
{
    class perfilPorBloques
    {
        string nombrePerfil;
        string config;
        string cA;
        string certUser;
        string keyUser;

        public string Config
        {
            get
            {
                return config;
            }

            set
            {
                config = value;
            }
        }

        public string CA
        {
            get
            {
                return cA;
            }

            set
            {
                cA = value;
            }
        }

        public string CertUser
        {
            get
            {
                return certUser;
            }

            set
            {
                certUser = value;
            }
        }

        public string KeyUser
        {
            get
            {
                return keyUser;
            }

            set
            {
                keyUser = value;
            }
        }

        public string NombrePerfil
        {
            get
            {
                return nombrePerfil;
            }

            set
            {
                nombrePerfil = value;
            }
        }

        public perfilPorBloques(string nombre, string config, string cA, string certUser, string keyUser)
        {
            this.NombrePerfil = nombre;
            this.Config = config;
            this.CA = cA;
            this.CertUser = certUser;
            this.KeyUser = keyUser;
        }
    }
}
