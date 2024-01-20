namespace Bau.Libraries.LibOllama.Api.Communications;

/// <summary>
///		Excepciones de comunicación con la API
/// </summary>
public class RestManagerException : Exception
{
	public RestManagerException() {}

	public RestManagerException(string message) : base(message) {}

	public RestManagerException(string message, Exception innerException) : base(message, innerException) {}
}
