using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace REC_ENDE_Ej5
{
    class DaoSQLite
    {
        SQLiteConnection miConexion;
        public void Conectar(string db)
        {
            string cadenaConexion = "Data Source="+ db +";Version=3;"+ "FailIfMissing=true;";
            try
            {
                miConexion = new SQLiteConnection(cadenaConexion);
                miConexion.Open();
            }
            catch (SQLiteException e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool Conectado()
        {
            return miConexion.State == System.Data.ConnectionState.Open;
        }

        public void Desconectar()
        {
            try
            {
                if (miConexion != null)
                {
                    miConexion.Close();
                }
            }
            catch (SQLiteException e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
