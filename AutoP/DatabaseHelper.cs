using System;
using System.Data;
using System.Data.SqlClient;

namespace AutoP
{
    public class DatabaseHelper
    {
        // Reemplaza esta cadena de conexión con los datos de tu servidor y base de datos
        public string ConnectionString { get; private set; } = "Server=DESKTOP-51H3J14;Database=Sistemafacturas;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;";

        // Método para ejecutar consultas SELECT
        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dataTable = new DataTable();
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);

                    // Agrega parámetros, si existen
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al ejecutar la consulta: {ex.Message}");
                }
                return dataTable;
            }
        }
    }
}

