using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace REC_ENDE_Ej5
{
    class DaoSQLite
    {
        Dictionary<string, int> usuariosErroneos = new Dictionary<string, int>(); 
        SQLiteConnection conexion;
        public void Conectar(string db)
        {
            string cadenaConexion = "Data Source="+ db +";Version=3;"+ "FailIfMissing=true;";
            try
            {
                conexion = new SQLiteConnection(cadenaConexion);
                conexion.Open();
            }
            catch (SQLiteException e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool Conectado()
        {
            return conexion.State == System.Data.ConnectionState.Open;
        }

        public void Desconectar()
        {
            try
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
            catch (SQLiteException e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Existe(string id)
        {
            if (conexion != null)
            {
                string orden = "select id, pass, active from usuario where id = '" + id + "'";
                try
                {
                    SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
                    SQLiteDataReader lector = cmd.ExecuteReader();
                    Usuario unUsuario = null;
                    if (lector.Read())
                    {
                        unUsuario = new Usuario();
                        unUsuario.Id = id;
                        unUsuario.Pass = lector["pass"].ToString();
                        unUsuario.Active = int.Parse(lector["active"].ToString());
                    }
                    lector.Close();
                    if (unUsuario == null)
                    {
                        return -1;
                    }
                    else
                    {
                        return unUsuario.Active;
                    }
                }
                catch (SQLiteException e)
                {
                    throw new Exception(e.Message);
                }
            }
            return -1;
        }

        public int Registrar(string id, string pass)
        {
            if (Existe(id) == -1)
            {
                string orden = "insert into usuario (id, pass, active) values ('" + id + "', '" + pass + "', 1)";
                try
                {
                    SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
                    return cmd.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    throw new Exception(e.Message);
                }
            }
            return -1;
        }

        public int Desactivar(string id)
        {
            if (Existe(id) != -1)
            {
                string orden = "update usuario set active = 0 where id = '" + id + "'";
                try
                {
                    SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
                    return cmd.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    throw new Exception(e.Message);
                }
            }
            return -1;
        }

        public int Acceder(string id, string pass)
        {
            int resultado = Existe(id);
            if (resultado == 1)
            {
                string orden = "select id, pass, active from usuario where id = '" + id + "' and pass = '" + pass + "'";
                try
                {
                    SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
                    SQLiteDataReader lector = cmd.ExecuteReader();
                    Usuario unUsuario = null;
                    int intentos;
                    if(lector.Read())
                    {
                        unUsuario = new Usuario();
                        unUsuario.Id = id;
                        unUsuario.Pass = pass;
                        unUsuario.Active = int.Parse(lector["active"].ToString());
                    }
                    lector.Close();
                    if (unUsuario != null)
                    {
                        if (usuariosErroneos.ContainsKey(id))
                        {
                            usuariosErroneos.Remove(id);
                        }
                        return 10;
                    }
                    else
                    {
                        if (usuariosErroneos.ContainsKey(id))
                        {
                            
                            if (usuariosErroneos.TryGetValue(id, out intentos))
                            {
                                if (intentos != 0)
                                {
                                    intentos--;
                                    usuariosErroneos.Remove(id);
                                    usuariosErroneos.Add(id, intentos);
                                }
                            }
                        }
                        else
                        {
                            intentos = 2;
                            usuariosErroneos.Add(id, intentos);
                        }
                        return intentos;
                    }
                }
                catch (SQLiteException e)
                {
                    throw new Exception(e.Message);
                }
            }
            else
	        {
                return resultado;
	        }
        }
    }
}
