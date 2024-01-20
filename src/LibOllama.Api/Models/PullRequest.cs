namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///		Solicitud de pull
/// </summary>
public class PullRequest
{
	public PullRequest(string name, bool insecure)
	{
		Name = name;
		Insecure = insecure;
	}

	/// <summary>
	///		Nombre
	/// </summary>
	public string Name { get; }

	/// <summary>
	///		Indica si es inseguro
	/// </summary>
	public bool Insecure { get; }
}