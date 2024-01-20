namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///		Solicitud para copiar un modelo
/// </summary>
public class CopyRequest
{
	public CopyRequest(string source, string destination)
	{
		Source = source;
		Destination = destination;
	}

	/// <summary>
	///		Origen
	/// </summary>
	public string Source { get; set; }

	/// <summary>
	///		Destino
	/// </summary>
	public string Destination { get; set; }
}