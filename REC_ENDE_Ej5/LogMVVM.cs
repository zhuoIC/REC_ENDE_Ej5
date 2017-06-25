using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REC_ENDE_Ej5
{
    class LogMVVM: INotifyPropertyChanged
    {
        DaoSQLite _dao;
        string _mensaje = "<sin datos>";

        public bool Conectado
        {
            get 
            {
                if (_dao != null)
                {
                    return _dao.Conectado();
                }
                else
                {
                    return false;
                }
            }
        }

        public bool NoConectado
        {
            get { return !Conectado; }
        }

        public string Mensaje
        {
            get { return _mensaje; }
            set 
            {
                if (_mensaje != value)
                {
                    _mensaje = value;
                    NotificarCambioPropiedad("Mensaje");
                }  
            }
        }

        void Conectar()
        {
            try
            {
                if (_dao == null)
                {
                    _dao = new DaoSQLite();
                    _dao.Conectar("unabasededatos");
                }
            }
            catch (Exception e)
            {
                Mensaje = "No se pudo conectar a la base de datos:" + e.Message;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotificarCambioPropiedad(string propiedad)
        {
            PropertyChangedEventHandler manejador = PropertyChanged;
            if (manejador != null)
            {
                manejador(this, new PropertyChangedEventArgs(propiedad));
            }
        }
    }
}
