using DBMigratePro.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMigratePro.BusinessLogic
{
    public class ConexionService
    {
        public async Task<bool> ProbarConexionAsync(string cadenaConexion, GestorBD gestorBD)
        {
            try
            {
                switch (gestorBD)
                {
                    case GestorBD.SQLServer:
                        using (var conexion = new Microsoft.Data.SqlClient.SqlConnection(cadenaConexion))
                        {
                            await conexion.OpenAsync();
                            return true;
                        }
                    // Añade casos para otros gestores de base de datos
                    // Por ejemplo:
                    case GestorBD.MongoDB:
                        // Implementa la prueba de conexión para MongoDB
                        break;
                    case GestorBD.PostgreSQL:
                        // Implementa la prueba de conexión para PostgreSQL
                        break;
                        // Añade más casos según sea necesario
                }
            }
            catch
            {
                return false;
            }

            return false; // Retorna falso por defecto si no entra en ninguno de los casos
        }
    }
}
