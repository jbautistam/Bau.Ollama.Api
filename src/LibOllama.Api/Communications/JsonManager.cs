using System.Text.Json;

namespace Bau.Libraries.LibOllama.Api.Communications;

/// <summary>
///		Rutina de ayuda para la conversión de Json
/// </summary>
internal static class JsonManager
{
	/// <summary>
	///		Deserializa un tipo
	/// </summary>
	internal static T? Deserialize<T>(string json)
	{
		return JsonSerializer.Deserialize<T>(json, 
												new JsonSerializerOptions
														{
															PropertyNameCaseInsensitive = true
														}
											);
	}
}
