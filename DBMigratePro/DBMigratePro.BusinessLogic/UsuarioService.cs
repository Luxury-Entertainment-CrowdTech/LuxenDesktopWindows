using DBMigratePro.BusinessLogic.HomomorphicService;
using DBMigratePro.DataAccess;
using DBMigratePro.Entities;
using MongoDB.Driver;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMigratePro.BusinessLogic
{
    public class UsuarioService
    {
        private readonly MongoDbContext _dbContext;

        public UsuarioService(string connectionString, string databaseName)
        {
            _dbContext = new MongoDbContext(connectionString, databaseName);
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            var usuarios = await _dbContext.Usuarios.Find(_ => true).ToListAsync();
            var decryptService = new DecryptService();

            foreach (var usuario in usuarios)
            {
                // Suponiendo que estos campos están encriptados y necesitas desencriptarlos
                usuario.Nombre = await decryptService.DecryptAsync(usuario.Nombre);
                usuario.Apellido = await decryptService.DecryptAsync(usuario.Apellido);
                usuario.Email = await decryptService.DecryptAsync(usuario.Email);
                usuario.UsuarioName = await decryptService.DecryptAsync(usuario.UsuarioName);
                usuario.Contrasena = await decryptService.DecryptAsync(usuario.Contrasena);
                usuario.NumeroTelefono = await decryptService.DecryptAsync(usuario.NumeroTelefono);
                // Ajusta según los campos que necesitas desencriptar
            }

            return usuarios;
        }

        public async Task<List<Usuario>> GetUsuariosAsyncSinDesencriptar()
        {
            // Simplemente retorna la lista de usuarios sin realizar ninguna desencriptación
            return await _dbContext.Usuarios.Find(_ => true).ToListAsync();
        }

        public async Task MigrarUsuariosASqlAsync(string connectionString, List<Usuario> usuarios)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            string databaseName = builder.InitialCatalog;

            // Crear la conexión sin especificar la base de datos inicial
            builder.InitialCatalog = "master";
            using (var masterConnection = new SqlConnection(builder.ConnectionString))
            {
                await masterConnection.OpenAsync();

                // Verificar si la base de datos existe
                var verificarDb = $"IF DB_ID(N'{databaseName}') IS NULL CREATE DATABASE [{databaseName}]";
                using (var commandVerificarDb = new SqlCommand(verificarDb, masterConnection))
                {
                    await commandVerificarDb.ExecuteNonQueryAsync();
                }
            }

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Crear la tabla si no existe
                await CrearTablaSiNoExiste(connection);

                foreach (var usuario in usuarios)
                {
                    var comandoInsertar = @"INSERT INTO Usuarios (Id, FaceId, Hash3, Nombre, Apellido, Email, UsuarioName, NumeroTelefono, FechaCreacion, Activo) 
                                    VALUES (@Id, @FaceId, @Hash3, @Nombre, @Apellido, @Email, @UsuarioName, @NumeroTelefono, @FechaCreacion, @Activo)";

                    using (var command = new SqlCommand(comandoInsertar, connection))
                    {
                        command.Parameters.AddWithValue("@Id", usuario.Id);
                        command.Parameters.AddWithValue("@FaceId", usuario.FaceId);
                        command.Parameters.AddWithValue("@Hash3", usuario.Hash3);
                        command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                        command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                        command.Parameters.AddWithValue("@Email", usuario.Email);
                        command.Parameters.AddWithValue("@UsuarioName", usuario.UsuarioName);
                        command.Parameters.AddWithValue("@NumeroTelefono", usuario.NumeroTelefono);
                        command.Parameters.AddWithValue("@FechaCreacion", usuario.FechaCreacion);
                        command.Parameters.AddWithValue("@Activo", usuario.Activo);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public async Task CrearTablaSiNoExiste(SqlConnection connection)
        {
            var verificarTabla = @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Usuarios') 
                            BEGIN 
                                CREATE TABLE Usuarios(
                                    Id NVARCHAR(MAX), 
                                    FaceId NVARCHAR(MAX), 
                                    Hash3 NVARCHAR(MAX), 
                                    Nombre NVARCHAR(MAX), 
                                    Apellido NVARCHAR(MAX), 
                                    Email NVARCHAR(MAX), 
                                    UsuarioName NVARCHAR(MAX), 
                                    NumeroTelefono NVARCHAR(MAX), 
                                    FechaCreacion DATETIME, 
                                    Activo BIT
                                ) 
                            END";

            using (var command = new SqlCommand(verificarTabla, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
