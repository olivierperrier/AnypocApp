using System.Text.Json.Serialization;

namespace AnypocApp.Models;

[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(KeyboardLayout))]
internal partial class KeyboardLayoutJsonContext : JsonSerializerContext
{
}
