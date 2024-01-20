namespace Bau.Libraries.LibOllama.Api.Streamer;

/// <summary>
///		Clase base para tratamiento de respuestas en stream
/// </summary>
public class ActionResponseStreamer<T> : IResponseStreamer<T>
{
	public ActionResponseStreamer(Action<T> responseHandler)
    {
		ResponseHandler = responseHandler;
	}

	/// <summary>
	///		Trata el bloque del stream
	/// </summary>
	public void Stream(T? stream)
	{
		if (stream is not null)
			ResponseHandler(stream);
	}

	/// <summary>
	///		Manejador de la respuesta
	/// </summary>
	public Action<T> ResponseHandler { get; }
}
