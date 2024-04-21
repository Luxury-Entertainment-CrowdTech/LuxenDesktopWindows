using Microsoft.Data.SqlClient;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMigratePro.BusinessLogic
{
    public class DatabaseService
    {
        private string _mongoConnectionString;

        public DatabaseService( string mongoConnection )
        {
            _mongoConnectionString = mongoConnection;
        }
        public async Task<List<string>> ObtenerTablasAsync(string connectionString, string databaseName)
        {
            var tablas = new List<string>();
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand($"USE {databaseName}; SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES;", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tablas.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return tablas;
        }

        public async Task<List<Dictionary<string, object>>> ObtenerDatosDeTablaAsync(string connectionString, string databaseName, string tableName)
        {
            var datos = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand($"SELECT * FROM {databaseName}.dbo.{tableName};", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var fila = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                fila.Add(reader.GetName(i), reader.GetValue(i));
                            }
                            datos.Add(fila);
                        }
                    }
                }
            }
            return datos;
        }

        public async Task MigrarDatosAMongoDB(List<Dictionary<string, object>> datos)
        {
            var client = new MongoDB.Driver.MongoClient(_mongoConnectionString);
            var database = client.GetDatabase("DatosClientes");
            var collection = database.GetCollection<BsonDocument>("datosclientes");

            var documentos = datos.Select(dato => new BsonDocument(dato));

            await collection.InsertManyAsync(documentos);
        }
    }
}
