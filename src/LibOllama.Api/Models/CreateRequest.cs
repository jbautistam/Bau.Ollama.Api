namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///		Solicitud de creación de un modelo
/// </summary>
public class CreateRequest
{
	public CreateRequest(string name, string path, bool stream)
	{
		Name = name;
		Path = path;
		Stream = stream;
	}

	/// <summary>
	///		Nombre del modelo
	/// </summary>
	public string Name { get; }

	/// <summary>
	///		Nombre del archivo de modelo
	/// </summary>
	public string Path { get; }

	/// <summary>
	///		Forma en que se devuelven los datos: false para obtener un único objeto de respuesta o true para un stream de objetos
	/// </summary>
	public bool Stream { get; }
}