using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using JsonEditor.Models;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace JsonEditor.Utilities
{
    public class JsonFileService
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public async Task<SPF_Data> LoadAsync(string filePath)
        {
            using FileStream stream = File.OpenRead(filePath);
            return await JsonSerializer.DeserializeAsync<SPF_Data>(stream, _options);
        }

        public async Task SaveAsync(string filePath, SPF_Data data)
        {
            using FileStream stream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(stream, data, _options);
        }
    }
}