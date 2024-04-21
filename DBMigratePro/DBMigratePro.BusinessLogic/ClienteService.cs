using DBMigratePro.DataAccess;
using DBMigratePro.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMigratePro.BusinessLogic
{
    public class ClienteService
    {
        private readonly MongoDbContext _dbContext;

        public ClienteService(string connectionString, string databaseName)
        {
            _dbContext = new MongoDbContext(connectionString, databaseName);
        }

        public async Task<List<Cliente>> ObtenerClientesAsync()
        {
            return await _dbContext.Clientes.Find(_ => true).ToListAsync();
        }

        public async Task<List<Cliente>> ObtenerClientesPorGestorBDAsync (int gestorBD)
        {
            return await _dbContext.Clientes.Find(cliente => cliente.GestorBD == gestorBD).ToListAsync();
        }

        public async Task RegistrarCliente(Cliente cliente)
        {
            // Aquí puedes implementar cualquier lógica adicional necesaria antes de la inserción
            await _dbContext.Clientes.InsertOneAsync(cliente);
        }
    }
}
