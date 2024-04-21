using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMigratePro.View.Models
{
    public class UsuarioViewModel
    {
        public string NombreCompleto { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string UsuarioName { get; set; } = default!;
        public string NumeroTelefono { get; set; } = default!;
        public DateTime FechaCreacion { get; set; } = default!;
    }
}
