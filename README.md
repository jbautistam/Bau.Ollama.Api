# Bau.Ollama.Api

La API de **Ollama** es una interfaz de programación de aplicaciones (API) que permite acceder a modelos de 
lenguaje de gran escala (LLM), como *Llama 2* y *Code Llama*, de forma local. 

Esta librería permite el acceso a la API de **Ollama** utilizando C#.

## Preparación

Antes de utilizar la API, debemos preparar el entorno. En mi caso, suelo utilizar Docker que me parece la forma más cómoda de
tener **Ollama** instalada en local.

En primer lugar, descargamos la imagen de **Ollama**:

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


## Ejemplo

Dentro del repositorio, encontramos una [consola de test](https://github.com/jbautistam/Bau.Ollama.Api/tree/main/test/Ollama.TestConsole) para comunicarnos con la API de Ollama.

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
