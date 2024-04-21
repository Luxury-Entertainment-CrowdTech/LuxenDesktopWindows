using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DBMigratePro.Entities.Homomorphic
{
    public class DecryptResponse
    {
        [JsonPropertyName("plainText")]
        public string PlainText { get; set; } = default!;
    }
}
