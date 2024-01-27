# Bau.Ollama.Api

Desde que aparecieron las inteligencias artificiales generativas y los LLM, supe que era algo que quería probar pero el problema era la instalación
en mi ordenador local: la mayoría de esas IA son bastante complicadas de instalar y mantener. Hasta que conocí **Ollama**.

**[Ollama](https://ollama.ai)** es un sistema LLM que nos permite utilizar **Docker** para instalarla en local. Además dispone de una API Rest bastante
sencilla que nos permite acceder a modelos de lenguaje de gran escala (LLM), como *Llama 2* y *Code Llama*, de forma local. 

Esta librería permite el acceso a la API de **Ollama** utilizando C# basada en [OllamaSharp](https://github.com/awaescher/OllamaSharp), con 
el mismo funcionamiento.

En realidad, prácticamente lo único que he hecho ha sido mejorar la estructura interna de la librería extrayendo clases
y cambiando el sistema de comunicaciones con `HttpClient` para que sea más compacta.

En este vídeo, podéis ver un ejemplo de uso de **Ollama** para obtener información de **Entity Framework** (aunque los modelos de lenguaje son
bastante generales, no tienen problema en mostrar información sobre código):

<video width="320" height="240" controls>
  <source src="docs/Ollama.mp4" type="video/mp4">
</video>

https://github.com/jbautistam/Bau.Ollama.Api/docs/Ollama.mp4

## Preparación

Antes de utilizar la API, debemos preparar el entorno.

En primer lugar, descargamos la imagen Docker de **Ollama**:

```sh
docker run -d -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama
```

Antes de poder llamar a la API, debemos indicar que se ejecute un modelo con el siguiente comando:

```sh
docker exec -it ollama ollama run llama2
```

Este comando ejecuta **Ollama** con el modelo *Llama 2*, pero **Ollama** nos permite ejecutar diferentes modelos de lenguaje. 

En concreto, en el momento de crear esta API, se podían utilizar estos modelos 
(la lista actualizada está en la web de [Ollama](https://ollama.ai/library)):

| Modelo             | Parámetros | Tamaño | Comando descarga en Docker   |
|--------------------|:----------:|:------:|------------------------------|
| Llama 2            | 7B         | 3.8GB  | ollama run llama2            |
| Mistral            | 7B         | 4.1GB  | ollama run mistral           |
| Dolphin Phi        | 2.7B       | 1.6GB  | ollama run dolphin-phi       |
| Phi-2              | 2.7B       | 1.7GB  | ollama run phi               |
| Neural Chat        | 7B         | 4.1GB  | ollama run neural-chat       |
| Starling           | 7B         | 4.1GB  | ollama run starling-lm       |
| Code Llama         | 7B         | 3.8GB  | ollama run codellama         |
| Llama 2 Uncensored | 7B         | 3.8GB  | ollama run llama2-uncensored |
| Llama 2 13B        | 13B        | 7.3GB  | ollama run llama2:13b        |
| Llama 2 70B        | 70B        | 39GB   | ollama run llama2:70b        |
| Orca Mini          | 3B         | 1.9GB  | ollama run orca-mini         |
| Vicuna             | 7B         | 3.8GB  | ollama run vicuna            |
| LLaVA              | 7B         | 4.5GB  | ollama run llava             |

**Nota:** para ejecutar los modelos 7B necesitarás 8GB de memoria RAM, los modelos 13B necesitan 16GB de memoria RAM y
para los modelos 33B necesitarás 32GB de memoria.

Así, por ejemplo, para ejecutar el modelo **Mistral** debemos ejecutar el siguiente comando:

```sh
docker exec -it ollama ollama run mistral
```

Si queremos que el cliente de Docker acceda a la GPU de NVidia, debemos instalar *Nvidia container toolkit* y ejecutar el siguiente comando:

```sh
docker run -d --gpus=all -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama
```

## Utilización de la librería

Una vez tengamos **Ollama** instalado y ejecutando en local, podemos utilizar el código de la librería para mandar prompts a la API y recibir las
respuestas.

Por supuesto, si tenemos un servidor accesible en la Web con **Ollama** instalado también podemos acceder a la API utilizando su URL.

### Conexión con Ollama

```csharp
using Bau.Libraries.LibOllama.Api;
using Bau.Libraries.LibOllama.Api.Models;

// Crea el manager
OllamaManager manager = new(new Uri("http://localhost:11434"), TimeSpan.FromMinutes(5));
```

### Visualización de los modelos

El método `GetModelsAsync` obtiene la lista de modelos existentes en nuestro local (o en la dirección establecida al crear `OllamaManager`):

```csharp
ListModelsResponse? modelsResponse = await manager.GetModelsAsync(cancellationToken);

	if (modelsResponse is not null && modelsResponse.Models.Count > 0)
		foreach (Bau.Libraries.LibOllama.Api.Models.ListModelsResponseItem item in manager.Models)
			Console.WriteLine($"\t{item.Name}");
	else
		Console.WriteLine("No models found");
```

### Prompt

Existen dos formas de enviar un prompt a **Ollama** y obtener la respuesta.

La primera de ellas, es utilizar el método `GetCompletionAsync`:

```csharp
string prompt = "Hello";
string model = "llama2";
ConversationContext? context = null;
ConversationContextWithResponse response = await Manager.GetCompletionAsync(prompt, model, context, cancellationToken);

	// Guarda el contexto
	context = new ConversationContext(response.Context);
```

Este método recibe el `prompt` que es el texto que deseamos que responda el LLM, `model` identifica el modelo y `context` es el contexto de la conversación (que
debemos guardar para posteriores consultas si queremos que **Ollama** recuerde el contexto).

La propiedad `Response` de `ConversationContextWithResponse` contiene la respuesta de **Ollama** a nuestra solicitud (la respuesta al `prompt` para entendernos).

La otra forma es utilizar el modelo de `Stream` de la API para recoger la respuesta según se vaya
procesando. Este método se llama `StreamCompletionAsync` y recibe prácticamente los mismos parámetros:

```csharp
_context = await Manager.StreamCompletionAsync(prompt, Model, _context, _streamer, cancellationToken);
```

El parámetro adicional `_streamer` es la clase que va a tratar la respuesta según la vaya recibiendo de la API. En este caso
guardamos el contexto para poder reutilizarlo durante la conversación en una variable global del controlador:

```csharp
private ConversationContext? _context = null;
private EventStreamer _streamer;
```

La clase `EventStreamer` es la que trata los datos recibidos de la API implementando la interface `IResponseStream` de
la API. En el ejemplo está definida como:

```csharp
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
```

Esta clase, llama a `ObamaChatController` según va recibiendo la respuesta de **Ollama**.
El método `TreatStream` de ese controlador, simplemente lanza un evento para que la aplicación principal
puede escribir el resultado en la consola:

```csharp
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
```

## Ejemplo

Dentro del repositorio, encontramos una [consola de test](https://github.com/jbautistam/Bau.Ollama.Api/tree/main/test/Ollama.TestConsole) 
como prueba para comunicarnos con la API de Ollama.

Aquí copio una sección de `program.cs`:

```csharp
CancellationToken cancellationToken = CancellationToken.None;
Ollama.TestConsole.Controller.OllamaChatController manager = new("http://localhost:11434", null);

// Asigna el manejador de eventos
manager.ChatReceived += (sender, args) => Console.Write(args.Message);
// Indica que se trate la respuesta como un Stream
manager.TreatResponseAsStream = true;

Console.Clear();
Console.ForegroundColor = ConsoleColor.Gray;
Console.WriteLine("Loading Ollama models ...");

if (!await manager.ConnectAsync(cancellationToken))
	Console.WriteLine("Can't connect to Ollama or can't find models");
else
{
	string? prompt;

		// Muestra los modelos activos
		if (manager.Models.Count() > 1)
		{
			Console.WriteLine("These models have been loaded:");
			foreach (Bau.Libraries.LibOllama.Api.Models.ListModelsResponseItem item in manager.Models)
				Console.WriteLine($"\t{item.Name}");
		}
		// Muestra el modelo que se está utilizando
		Console.WriteLine($"You are talking to {manager.Model} now.");
		// Procesa los input del usuario
		prompt = GetPrompt();
		while (!string.IsNullOrEmpty(prompt) && !prompt.Equals("bye", StringComparison.CurrentCultureIgnoreCase))
		{
			// Si realmente ha escrito algo
			if (!string.IsNullOrWhiteSpace(prompt))
			{
				// Cambia el color para la salida de la consola
				Console.ForegroundColor = ConsoleColor.Cyan;
				// Llama a la API para obtener la respuesta
				try
				{
					await manager.PromptAsync(prompt, cancellationToken);
				}
				catch (Exception exception)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Error when call to Ollama");
					Console.WriteLine($"Error: {exception.Message}");
				}
				// Salta de línea
				Console.WriteLine();
				// Obtiene un nuevo prompt
				prompt = GetPrompt();
			}
		}
		// Finaliza el proceso
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine("Bye, bye");
}
```
