namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///		Respuesta a una solicitud de pull
/// </summary>
public class PullResponse
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
	public long Total { get; set; }

	/// <summary>
	///		Tamaño subido
	/// </summary>
	public long Completed { get; set; }

	/// <summary>
	///		Porcentaje
	/// </summary>
	public double Percent => Total == 0 ? 100.0 : Completed * 100 / Total;
}