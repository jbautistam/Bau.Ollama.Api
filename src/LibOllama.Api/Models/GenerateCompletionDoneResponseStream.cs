namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///		Respuesta de finalización de <see cref="GenerateCompletionRequest"/>
/// </summary>
public class GenerateCompletionDoneResponseStream : GenerateCompletionResponseStream
{
	/// <summary>
	///		Contexto
	/// </summary>
	public List<long> Context { get; set; } = new();

	/// <summary>
	///		Duración total
	/// </summary>
	public long TotalDuration { get; set; }

	/// <summary>
	///		Duración de la carga
	/// </summary>
	public long LoadDuration { get; set; }

	/// <summary>
	///		Número de elementos de la evaluación del prompt
	/// </summary>
	public int PromptEvalCount { get; set; }

	/// <summary>
	///		Tiempo de la evaluación del prompt
	/// </summary>
	public long PromptEvalDuration { get; set; }

	/// <summary>
	///		Evaluación del prompt
	/// </summary>
	public int EvalCount { get; set; }

	/// <summary>
	///		Duración del prompt
	/// </summary>
	public long EvalDuration { get; set; }
}