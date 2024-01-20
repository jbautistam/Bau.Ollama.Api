using System.Text;

using Bau.Libraries.LibOllama.Api.Communications;
using Bau.Libraries.LibOllama.Api.Models;
using Bau.Libraries.LibOllama.Api.Streamer;

namespace Bau.Libraries.LibOllama.Api;

/// <summary>
///		Cliente de la api de Ollama
/// </summary>
public class OllamaManager
{
	public OllamaManager(Uri url, TimeSpan? timeout = null)
	{
		RestManager = new RestManager(url, timeout ?? TimeSpan.FromMinutes(2));
	}

	/// <summary>
	///		Crea un modelo
	/// </summary>
	public async Task CreateModelAsync(string name, string path, Action<CreateResponse> streamer, CancellationToken cancellationToken)
	{
		await CreateModelAsync(name, path, new ActionResponseStreamer<CreateResponse>(streamer), cancellationToken);
	}

	/// <summary>
	///		Crea un modelo
	/// </summary>
	public async Task CreateModelAsync(string name, string path, IResponseStreamer<CreateResponse> streamer, CancellationToken cancellationToken)
	{
		await CreateModelAsync(new CreateRequest(name, path, true), streamer, cancellationToken);
	}

	/// <summary>
	///		Crea un modelo
	/// </summary>
	public async Task CreateModelAsync(CreateRequest request, IResponseStreamer<CreateResponse> streamer, CancellationToken cancellationToken)
	{
		await RestManager.StreamPostAsync("/api/create", request, streamer, cancellationToken);
	}

	/// <summary>
	///		Borra un modelo
	/// </summary>
	public async Task DeleteModelAsync(string name, CancellationToken cancellationToken)
	{
		await RestManager.DeleteAndCheckAsync("/api/delete", new ModelRequest(name), cancellationToken);
	}

	/// <summary>
	///		Lista los modelos
	/// </summary>
	public async Task<ListModelsResponse?> GetModelsAsync(CancellationToken cancellationToken) 
	{
		return await RestManager.GetResponseDataAsync<ListModelsResponse>("/api/tags", cancellationToken);
	}

	/// <summary>
	///		Obtiene la información de un modelo
	/// </summary>
	public async Task<ShowResponse?> GetShowModelAsync(string name, CancellationToken cancellationToken)
	{
		return await RestManager.PostAndParseAsync<ShowResponse>("/api/show", new ModelRequest(name), cancellationToken);
	}

	/// <summary>
	///		Copia un modelo
	/// </summary>
	public async Task CopyModelAsync(string source, string destination, CancellationToken cancellationToken)
	{
		await CopyModelAsync(new CopyRequest(source, destination), cancellationToken);
	}

	/// <summary>
	///		Copia un modelo
	/// </summary>
	public async Task CopyModelAsync(CopyRequest request, CancellationToken cancellationToken)
	{
		await RestManager.PostAndCheckAsync("/api/copy", request, cancellationToken);
	}

	/// <summary>
	///		Descarga un modelo
	/// </summary>
	public async Task PullModelAsync(string model, Action<PullResponse> streamer, CancellationToken cancellationToken)
	{
		await PullModelAsync(model, new ActionResponseStreamer<PullResponse>(streamer), cancellationToken);
	}

	/// <summary>
	///		Pull de un modelo
	/// </summary>
	public async Task PullModelAsync(string model, IResponseStreamer<PullResponse> streamer, CancellationToken cancellationToken)
	{
		await PullModelAsync(new PullRequest(model, false), streamer, cancellationToken);
	}

	/// <summary>
	///		Pull de un modelo
	/// </summary>
	public async Task PullModelAsync(PullRequest request, IResponseStreamer<PullResponse> streamer, CancellationToken cancellationToken)
	{
		await RestManager.StreamPostAsync("/api/pull", request, streamer, cancellationToken);
	}

	/// <summary>
	///		Sube un modelo
	/// </summary>
	public async Task PushModelAsync(string name, Action<PushResponse> streamer, CancellationToken cancellationToken)
	{
		await PushModelAsync(name, new ActionResponseStreamer<PushResponse>(streamer), cancellationToken);
	}

	/// <summary>
	///		Sube un modelo
	/// </summary>
	public async Task PushModelAsync(string name, IResponseStreamer<PushResponse> streamer, CancellationToken cancellationToken)
	{
		await PushModelAsync(new PushRequest(name, false, true), streamer, cancellationToken);
	}

	/// <summary>
	///		Sube un modelo
	/// </summary>
	public async Task PushModelAsync(PushRequest request, IResponseStreamer<PushResponse> streamer, CancellationToken cancellationToken)
	{
		await RestManager.StreamPostAsync("/api/push", request, streamer, cancellationToken);
	}

	/// <summary>
	///		Genera un embedding
	/// </summary>
	public async Task<GenerateEmbeddingResponse?> GenerateEmbeddingsAsync(string model, string prompt, string? options, CancellationToken cancellationToken)
	{
		return await GenerateEmbeddingsAsync(new GenerateEmbeddingRequest(model, prompt, options), cancellationToken);
	}

	/// <summary>
	///		Genera un embedding
	/// </summary>
	public async Task<GenerateEmbeddingResponse?> GenerateEmbeddingsAsync(GenerateEmbeddingRequest request, CancellationToken cancellationToken)
	{
		return await RestManager.PostAndParseAsync<GenerateEmbeddingResponse>("/api/embeddings", request, cancellationToken);
	}

	/// <summary>
	///		Completa un prompt
	/// </summary>
	public async Task<ConversationContext> StreamCompletionAsync(string prompt, string model, ConversationContext? context,
																 Action<GenerateCompletionResponseStream> streamer, CancellationToken cancellationToken)
	{
		return await StreamCompletionAsync(prompt, model, context, new ActionResponseStreamer<GenerateCompletionResponseStream>(streamer), cancellationToken);
	}

	/// <summary>
	///		Completa un prompt
	/// </summary>
	public async Task<ConversationContext> StreamCompletionAsync(string prompt, string model, ConversationContext? context, 
																 IResponseStreamer<GenerateCompletionResponseStream> streamer, CancellationToken cancellationToken)
	{
		return await GenerateCompletionAsync(new GenerateCompletionRequest(model, prompt, true, context?.Context), streamer, cancellationToken);
	}

	/// <summary>
	///		Completa un prompt
	/// </summary>
	public async Task<ConversationContextWithResponse> GetCompletionAsync(string prompt, string model, ConversationContext? context, CancellationToken cancellationToken)
	{
		GenerateCompletionRequest request = new(model, prompt, false, context?.Context);
		StringBuilder builder = new();
		ConversationContext result = await GenerateCompletionAsync(request, 
																   new ActionResponseStreamer<GenerateCompletionResponseStream>(status => builder.Append(status.Response)), 
																   cancellationToken);

			// Devuelve la respuesta
			return new ConversationContextWithResponse(builder.ToString(), result.Context);
	}

	/// <summary>
	///		Completa un prompt
	/// </summary>
	public async Task<ConversationContext> GenerateCompletionAsync(GenerateCompletionRequest generateRequest, IResponseStreamer<GenerateCompletionResponseStream> streamer,
																   CancellationToken cancellationToken)
	{
		// Genera el promp
		await foreach (string line in RestManager.StreamLinesPostAsync("/api/generate", generateRequest, generateRequest.Stream, cancellationToken))
		{
			GenerateCompletionResponseStream? streamedResponse = JsonManager.Deserialize<GenerateCompletionResponseStream>(line);

				// Trata la respuesta
				streamer.Stream(streamedResponse);
				// Trata el final
				if (streamedResponse?.Done ?? false)
				{
					GenerateCompletionDoneResponseStream? doneResponse = JsonManager.Deserialize<GenerateCompletionDoneResponseStream>(line);

						// Devuelve el contexto
						return new ConversationContext(doneResponse?.Context ?? new List<long>());
				}
		}
		// Si ha llegado hasta aquí devuelve una lista vaíca
		return new ConversationContext(new List<long>());
	}

	/// <summary>
	///		Manager de comunicaciones Rest
	/// </summary>
	private RestManager RestManager { get; }
}