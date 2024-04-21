using DBMigratePro.Entities.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMigratePro.Entities
{
    public class Cliente
    {
        [BsonId] // Indica que esta propiedad es el identificador del documento
        [BsonRepresentation(BsonType.ObjectId)] // Indica que aunque es un ObjectId en la DB, se manejará como string en C#
        public string Id { get; set; } = default!;
        public string Nombre { get; set; } = default!;
        public int Tipo { get; set; }
        public string RucDniPasaporte { get; set; } = default!;
        public int GestorBD { get; set; }
        public string CadenaConexion { get; set; } = default!;
        public List<string> BasesDeDatos { get; set; } = new List<string>();
        
    }
}
