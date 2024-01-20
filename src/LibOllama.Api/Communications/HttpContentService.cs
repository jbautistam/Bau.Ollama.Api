namespace Bau.Libraries.LibOllama.Api.Communications;

/// <summary>
///		Servicio para obtener el contenido de una URL
/// </summary>
public class HttpContentService
{
	/// <summary>
	///		Obtiene el contenido de una URL
	/// </summary>
	internal async Task<string> GetContentAsync(string url, string bearerToken, CancellationToken cancellationToken)
	{
		using (HttpClient client = GetClient(url))
		{
			// Añade la cabecera de autorización
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
			// Devuelve el contenido leido de la URL
			return await client.GetStringAsync(url, cancellationToken);
		}
	}

	/// <summary>
	///		Obtiene el cliente de HTTP
	/// </summary>
	private HttpClient GetClient(string url)
	{
		return new HttpClient
						{
							BaseAddress = new Uri(url),
							Timeout = TimeSpan.FromMinutes(2)
						};
	}
}
