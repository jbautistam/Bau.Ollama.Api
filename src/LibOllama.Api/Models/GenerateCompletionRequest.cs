namespace Bau.Libraries.LibOllama.Api.Models;

///	<summary>
///		Solicitud de generación
/// </summary>
public class GenerateCompletionRequest
{
	public GenerateCompletionRequest(string model, string prompt, bool stream, List<long>? contextIds)
	{
		Model = model;
		Prompt = prompt;
		Stream = stream;
		Context = contextIds ?? new List<long>();
	}

	/// <summary>
	///		Nombre del modelo
	/// </summary>
	public string Model { get; set; } = default!;

	/// <summary>
	///		Prompt para el que se va a generar la respuesta
	/// </summary>
	public string Prompt { get; set; } = default!;

	/// <summary>
	///		Parámetros adicionales (dependen del Modelfile como la temperatura)
	/// </summary>
	public string Options { get; set; } = default!;

	/// <summary>
	///		Prompt del sistema (sobrescribe lo que se define en Modelfile)
	/// </summary>
	public string System { get; set; } = default!;

	/// <summary>
	///		Prompt completo o plantilla (sobrescribe lo que se define en Modelfile)
	/// </summary>
	public string Template { get; set; } = default!;

	/// <summary>
	///		Parámetro del contexto devuelto a una solicitud de generación anterior, se puede utilizar para mantener la memoria de la conversación
	/// </summary>
	public List<long> Context { get; set; } = new();

	/// <summary>
	///		Si es false la respuesta se obtiene como un único objeto, si no, se obtiene un stream de objetos
	/// </summary>
	public bool Stream { get; set; }
}