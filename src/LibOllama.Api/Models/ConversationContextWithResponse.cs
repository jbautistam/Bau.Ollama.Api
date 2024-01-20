namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///		Contexto de la conversación con la respuesta
/// </summary>
public class ConversationContextWithResponse : ConversationContext
{
	public ConversationContextWithResponse(string response, List<long> context) : base(context)
	{
		Response = response;
	}

	/// <summary>
	///		Respuesta
	/// </summary>
	public string Response { get; }
}