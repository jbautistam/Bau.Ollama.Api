namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///		Respuesta de la llamada al listado de modelos
/// </summary>
public class ListModelsResponse
{ 
	/// <summary>
	///		Modelos
	/// </summary>
	public List<ListModelsResponseItem> Models { get; set; } = new();
}
