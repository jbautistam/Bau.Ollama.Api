namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///		Dto del nombre de un modelo
/// </summary>
public class ModelRequest
{
	public ModelRequest(string name)
	{
		Name = name;
	}

	/// <summary>
	///		Nombre del modelo a mostrar
	/// </summary>
	public string Name { get; }
}