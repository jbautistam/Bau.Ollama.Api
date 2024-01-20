namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///		Respuesta a la Api de mostrar modelo
/// </summary>
public class ShowResponse
{
	/// <summary>
	///		Licencia
	/// </summary>
	public string License { get; set; } = default!;

	/// <summary>
	///		Archivo de modelo
	/// </summary>
	public string Modelfile { get; set; } = default!;

	/// <summary>
	///		Parámetros
	/// </summary>
	public string Parameters { get; set; } = default!;

	/// <summary>
	///		Plantilla
	/// </summary>
	public string Template { get; set; } = default!;
}