namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///		Respuesta a la llamada de generación de "embedings"
/// </summary>
public class GenerateEmbeddingResponse
{
	/// <summary>
	///		Respuesta
	/// </summary>
	public List<double> Embedding { get; set; } = new();
}
