using Bau.Libraries.LibOllama.Api.Models;
using Bau.Libraries.LibOllama.Api.Streamer;

namespace Ollama.TestConsole.Controller;

/// <summary>
///		Intérprete de las salidas por stream de Ollama
/// </summary>
internal class EventStreamer : IResponseStreamer<GenerateCompletionResponseStream>
{
	internal EventStreamer(OllamaChatController manager)
	{
		Manager = manager;
	}

	/// <summary>
	///		Trata el stream
	/// </summary>
	public void Stream(GenerateCompletionResponseStream? stream)
	{
		Manager.TreatStream(stream);
	}

	/// <summary>
	///		Manager de Ollama
	/// </summary>
	internal OllamaChatController Manager { get; }
}
