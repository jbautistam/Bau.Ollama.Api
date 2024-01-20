namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///		Solicitud de push de un modelo
/// </summary>
public class PushRequest
{
	public PushRequest(string name, bool insecure, bool stream)
	{
		Name = name;
		Insecure = insecure;
		Stream = stream;
	}

	/// <summary>
	///		Nombre del modelo a introducir, de la forma: <namespace>/<model>:<tag>
	/// </summary>
	public string Name { get; }

	/// <summary>
	///		Indica si es inseguro
	/// </summary>
	public bool Insecure { get; }

	/// <summary>
	///		Indica cómo se va a hacer el stream del modelo
	/// </summary>
	public bool Stream { get; }
}