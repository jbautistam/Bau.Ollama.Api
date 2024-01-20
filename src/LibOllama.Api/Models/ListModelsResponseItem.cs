namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///		Dtos de un modelo
/// </summary>
public class ListModelsResponseItem
{
	/// <summary>
	///		Nombre
	/// </summary>
	public string Name { get; set; } = default!;

	/// <summary>
	///		Fecha de modificación
	/// </summary>
	public DateTime ModifiedAt { get; set; } = default!;

	/// <summary>
	///		Tamaño
	/// </summary>
	public long Size { get; set; } = default!;

	/// <summary>
	///		Firma digital
	/// </summary>
	public string Digest { get; set; } = default!;
}
