using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REC_ENDE_Ej5
{
    public class Usuario
    {
        string _id;
        string _pass;
        int _active;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Pass
        {
            get { return _pass; }
            set { _pass = value; }
        }

        public int Active
        {
            get { return _active; }
            set { _active = value; }
        }
    }

    public class UsuariosErroneos
    {
        Usuario _unUsuario;
        int _intentos;

        public Usuario UnUsuario
        {
            get { return _unUsuario; }
            set { _unUsuario = value; }
        }

        public int Intentos
        {
            get { return _intentos; }
            set { _intentos = value; }
        }
    }
}
