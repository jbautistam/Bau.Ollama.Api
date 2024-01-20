namespace Ollama.TestConsole.Controller;

/// <summary>
///		Argumentos del mensaje de respuesta del prompt
/// </summary>
public class PromptResponseArgs : EventArgs
{
	public PromptResponseArgs(string message, bool isEnd)
	{
		Message = message;
		IsEnd = isEnd;
	}

	/// <summary>
	///		Mensaje recibido
	/// </summary>
	public string Message { get; }

	/// <summary>
	///		Indica si ha finalizado la respuesta
	/// </summary>
	public bool IsEnd { get; }
}
