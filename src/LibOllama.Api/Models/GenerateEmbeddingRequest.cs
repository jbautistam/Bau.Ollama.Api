namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///		Solicitud de generación de "embeddings"
/// </summary>
public class GenerateEmbeddingRequest
{
	public GenerateEmbeddingRequest(string model, string prompt, string? options)
	{
		Model = model;
		Prompt = prompt;
		Options = options;
	}

	/// <summary>
	///		Modelo
	/// </summary>
	public string Model { get; }

	/// <summary>
	///		Prompt
	/// </summary>
	public string Prompt { get; }

	/// <summary>
	///		Opciones
	/// </summary>
	public string? Options { get; }
}