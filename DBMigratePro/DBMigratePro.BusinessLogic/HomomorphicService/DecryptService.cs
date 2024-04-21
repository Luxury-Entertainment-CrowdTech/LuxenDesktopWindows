using DBMigratePro.Entities.Homomorphic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DBMigratePro.BusinessLogic.HomomorphicService
{
    public class DecryptService
    {
        private readonly HttpClient _httpClient;
        private readonly string _decryptEndpoint = "https://homomorphicencryptionservice.luxen.club/Encryption/decrypt";

        public DecryptService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> DecryptAsync(string cipherText)
        {
            var request = new DecryptRequest
            {
                CipherTextString = cipherText
            };
            var jsonRequest = JsonSerializer.Serialize(request);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_decryptEndpoint, httpContent);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var decryptResponse = JsonSerializer.Deserialize<DecryptResponse>(jsonResponse);
                return decryptResponse?.PlainText ?? string.Empty;
            }

            throw new Exception("Failed to decrypt data.");
        }
    }
}
