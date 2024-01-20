namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///		Respuesta de una solicitud pusht
/// </summary>
public class PushResponse
{
	/// <summary>
	///		Estado
	/// </summary>
	public string Status { get; set; } = default!;

	/// <summary>
	///		Firma
	/// </summary>
	public string Digest { get; set; } = default!;

	/// <summary>
	///		Total
	/// </summary>
	public int Total { get; set; }
}