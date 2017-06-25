using System;
using System.ComponentModel;
using System.Windows.Input;

namespace REC_ENDE_Ej5
{
    class LogMVVM: INotifyPropertyChanged
    {
        DaoSQLite _dao;
        string _mensaje = "<sin datos>";
        string _user;
        string _password;
        string _rePassword;

        #region Propiedades

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

        public string User
        {
            get { return _user; }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    NotificarCambioPropiedad("User");
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    NotificarCambioPropiedad("Password");
                    NotificarCambioPropiedad("Color");
                }
            }
        }

        public string RePassword
        {
            get { return _rePassword; }
            set
            {
                if (_rePassword != value)
                {
                    _rePassword = value;
                    NotificarCambioPropiedad("RePassword");
                    NotificarCambioPropiedad("Color");
                }
            }
        }

        public bool Coincide
        {
            get 
            {
                return Password != null && RePassword != null && Password.Equals(RePassword);
            }
        }

        public string Color
        {
            get 
            {
                if (Coincide)
                {
                    return "green";
                }
                else
                {
                    return "red";
                }
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotificarCambioPropiedad(string propiedad)
        {
            PropertyChangedEventHandler manejador = PropertyChanged;
            if (manejador != null)
            {
                manejador(this, new PropertyChangedEventArgs(propiedad));
            }
        }

        #region Metodos

        void Conectar()
        {
            try
            {
                _dao = new DaoSQLite();
                _dao.Conectar("formulario.db");
            }
            catch (Exception e)
            {
                Mensaje = "No se pudo conectar a la base de datos:" + e.Message;
            }
        }

        void Acceder()
        {

            if (_dao == null)
            {
                Conectar();
            }
            try
            {
                if (User != null && Password != null)
                {
                    int resultado = _dao.Acceder(User, Password);
                    switch (resultado)
                    {
                        case 10:
                            Mensaje = "Has accedido con éxito.";
                            break;
                        case -1:
                            Mensaje = "El usuario no existe.";
                            break;
                        case 0:
                            _dao.Desactivar(User);
                            Mensaje = "Usuario bloqueado. Contacte con el administrador de la base de datos.";
                            break;
                        default:
                            Mensaje = "Contraseña incorrecta. Te quedan " + resultado + " intento(s).";
                            break;
                    }
                }
                else
                {
                    Mensaje = "Rellene los campos.";
                }
            }
            catch (Exception e)
            {
                Mensaje = "Hubo un fallo al intentar acceder: " + e.Message;
            }
        }

        void Registrar()
        {
            if (_dao == null)
            {
                Conectar();
            }
            try
            {
                if (Coincide)
                {
                    if (User == null)
                    {
                        Mensaje = "Rellene los campos.";
                    }
                    else if (_dao.Registrar(User, Password) == 1)
                    {
                        Mensaje = "Usuario registrado con éxito.";
                    }
                    else
                    {
                        Mensaje = "El usuario ya existe.";
                    }
                }
                else
                {
                    Mensaje = "Las contraseñas no coinciden.";
                }
            }
            catch (Exception e)
            {
                Mensaje = "Hubo un fallo al intentar acceder: " + e.Message;
            }
        } 
        #endregion

        #region Comandos
        public ICommand Acceder_Click
        {
            get { return new RelayComand(o => Acceder(), o => true); }
        }

        public ICommand Registrar_Click
        {
            get { return new RelayComand(o => Registrar(), o => true); }
        } 
        #endregion
    }
}
