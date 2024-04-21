using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMigratePro.Entities
{
    public class Usuario
    {
        [BsonId] // Indica que esta propiedad es el identificador del documento
        [BsonRepresentation(BsonType.ObjectId)] // Indica que aunque es un ObjectId en la DB, se manejará como string en C#
        public string Id { get; set; } = default!;

        [BsonElement("faceId")]
        public string FaceId { get; set; } = default!;

        [BsonElement("hash3")]
        public string Hash3 { get; set; } = default!;

        [BsonElement("nombre")]
        public string Nombre { get; set; } = default!;

        [BsonElement("apellido")]
        public string Apellido { get; set; } = default!;

        [BsonElement("email")]
        public string Email { get; set; } = default!;

        // Nota: Cambiado el nombre de 'usuario' a 'UsuarioName' para evitar conflictos
        // Asegúrate de que el nombre en BsonElement coincida con el campo en tu documento MongoDB
        [BsonElement("usuario")]
        public string UsuarioName { get; set; } = default!;

        [BsonElement("contrasena")]
        public string Contrasena { get; set; } = default!;

        // MongoDB almacena las fechas en un formato específico, asegúrate de que el mapeo sea correcto
        [BsonElement("fechaCreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [BsonElement("activo")]
        public bool Activo { get; set; }

        [BsonElement("numeroTelefono")]
        public string NumeroTelefono { get; set; } = default!;
    }
}
