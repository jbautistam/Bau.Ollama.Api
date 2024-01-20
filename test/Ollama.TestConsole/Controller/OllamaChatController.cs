using Bau.Libraries.LibOllama.Api;
using Bau.Libraries.LibOllama.Api.Models;

namespace Ollama.TestConsole.Controller;

/// <summary>
///		Controlador del chat de Ollama
/// </summary>
public class OllamaChatController
{
	// Eventos públicos
	public event EventHandler<PromptResponseArgs>? ChatReceived;
	// Variables privadas
	private ConversationContext? _context = null;
	private EventStreamer _streamer;
	private string? _model;

	public OllamaChatController(string? url, TimeSpan? timeout = null)
	{
		// Url por defecto si no se ha pasado nada
		if (string.IsNullOrWhiteSpace(url))
			url = "http://localhost:11434";
		// Crea el manager y el streamer principal
		Manager = new OllamaManager(new Uri(url), timeout);
		_streamer = new EventStreamer(this);
	}

	/// <summary>
	///		Conecta al manager
	/// </summary>
	public async Task<bool> ConnectAsync(CancellationToken cancellationToken)
	{
		// Conecta si es necesario
		if (!Connected)
			try
			{
				ListModelsResponse? modelsResponse = await Manager.GetModelsAsync(cancellationToken);

					if (modelsResponse is not null && modelsResponse.Models.Count > 0)
					{
						// Guarda los modelos
						Models.Clear();
						Models.AddRange(modelsResponse.Models);
						// Indica que se ha conectado
						Connected = true;
					}
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine($"Error: {exception.Message}");
			}
		// Devuelve el valor que indica si se ha conectado
		return Connected;
	}

	/// <summary>
	///		Envía un mensaje a Ollama y obtiene la respuesta
	/// </summary>
	public async Task PromptAsync(string prompt, CancellationToken cancellationToken)
	{
		if (!string.IsNullOrWhiteSpace(prompt) && !ParseScapedPrompt(prompt.Trim()) && Connected && !string.IsNullOrWhiteSpace(Model))
		{
			// Trata la respuesta
			if (TreatResponseAsStream)
				_context = await Manager.StreamCompletionAsync(prompt, Model, _context, _streamer, cancellationToken);
			else
			{
				ConversationContextWithResponse response = await Manager.GetCompletionAsync(prompt, Model, _context, cancellationToken);

					// Escribe la respuesta
					RaiseResponseEvent(response.Response, true);
					// Guarda el contexto
					_context = new ConversationContext(response.Context);
			}
		}
	}

	/// <summary>
	///		Interpreta el prompt para tratar los prompts predefinidos
	/// </summary>
	private bool ParseScapedPrompt(string prompt)
	{
		bool scaped = true;

			// Interpreta el prompt entre los predefinidos
			if (prompt.StartsWith("/model", StringComparison.CurrentCultureIgnoreCase))
			{
				string model = prompt.Substring("/model".Length + 1);

					// Busca el modelo en la lista
					if (!string.IsNullOrWhiteSpace(model))
					{
						bool found = false;

							// Quita los espacios
							model = model.Trim();
							// Busca el modelo
							foreach (ListModelsResponseItem item in Models)
								if (item.Name.StartsWith(model))
								{
									// Cambia el modelo
									Model = item.Name;
									// Indica que se ha cambiado
									found = true;
									RaiseResponseEvent($"You are talking to {Model} now.", true);
								}
							// Indica al usuario que no ha podido encontrar el modelo buscado
							if (!found)
								RaiseResponseEvent($"Can't find the model {model}. You are talking to {Model} now.", true);
					}
			}
			else // Indica que este es un valor desconocido
				scaped = false;
			// Devuelve el valor que indica si se ha tratado el prompt
			return scaped;
	}

	/// <summary>
	///		Trata el stream recibido
	/// </summary>
	internal void TreatStream(GenerateCompletionResponseStream? stream)
	{
		if (stream is not null)
			RaiseResponseEvent(stream.Response, stream.Done);
		else
			RaiseResponseEvent("Stream lost", true);
	}

	/// <summary>
	///		Lanza el evento de respuesta
	/// </summary>
	private void RaiseResponseEvent(string message, bool isEnd)
	{
		ChatReceived?.Invoke(this, new PromptResponseArgs(message, isEnd));
	}

	/// <summary>
	///		Manager de Ollama
	/// </summary>
	internal OllamaManager Manager { get; }

	/// <summary>
	///		Indica si está conectado
	/// </summary>
	public bool Connected { get; private set; }

	/// <summary>
	///		Modelos del motor
	/// </summary>
	public List<ListModelsResponseItem> Models { get; } = new();

	/// <summary>
	///		Modelo actual
	/// </summary>
	public string? Model 
	{ 
		get 
		{
			// Si no se ha definido el modelo, se coge el primero
			if (string.IsNullOrWhiteSpace(_model) && Models.Count > 0)
				_model = Models[0].Name;
			// Deuvelve el modelo
			return _model;
		}
		private set { _model = value; }
	}

	/// <summary>
	///		Indica si se debe tratar la respuesta como un stream o como contenido completo (si se debe leer por palabra o no)
	/// </summary>
	public bool TreatResponseAsStream { get; set; } = true;
}
