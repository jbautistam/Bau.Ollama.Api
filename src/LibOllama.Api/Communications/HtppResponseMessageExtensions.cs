using System.Text.Json;

namespace Bau.Libraries.LibOllama.Api.Communications;

/// <summary>
///		Extensiones de <see cref="HttpResponseMessage"/>
/// </summary>
public static class HttpResponseMessageExtensions
{
	/// <summary>
	///		Obtiene un tipo del cuerpo del mensaje
	/// </summary>
    public static async Task<T?> GetToAsync<T>(this HttpResponseMessage responseMessage, CancellationToken cancellationToken)
    {
		string json = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

			// Convierte la cadena JSON en un objeto
			return JsonSerializer.Deserialize<T>(json, 
												 new JsonSerializerOptions
															{
																PropertyNameCaseInsensitive = true
															}
												);
	}

	/// <summary>
	///		Obtiene el cuerpo de la respuesta
	/// </summary>
    public static async Task<string> GetContentAsync(this HttpResponseMessage responseMessage, CancellationToken cancellationToken) => await responseMessage.Content.ReadAsStringAsync(cancellationToken);
}