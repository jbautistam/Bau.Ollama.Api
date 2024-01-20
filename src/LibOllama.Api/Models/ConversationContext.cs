namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///		Contexto de la conversación
/// </summary>
public class ConversationContext
{
	public ConversationContext(List<long> context)
	{
		Context = context;
	}

	/// <summary>
	///		Contexto
	/// </summary>
	public List<long> Context { get; } = new();
}