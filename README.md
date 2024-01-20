# Bau.Ollama.Api

Get up and running with large language models, locally.
Run Llama 2, Code Llama, and other models. Customize and create your own.

Get up and running with large language models locally.

macOS
Download

Windows
Coming soon! For now, you can install Ollama on Windows via WSL2.

Linux & WSL2
curl https://ollama.ai/install.sh | sh
Manual install instructions

Docker
The official Ollama Docker image ollama/ollama is available on Docker Hub.

Quickstart
To run and chat with Llama 2:

ollama run llama2
Model library
Ollama supports a list of open-source models available on ollama.ai/library

Here are some example open-source models that can be downloaded:

Model	Parameters	Size	Download
Llama 2	7B	3.8GB	ollama run llama2
Mistral	7B	4.1GB	ollama run mistral
Dolphin Phi	2.7B	1.6GB	ollama run dolphin-phi
Phi-2	2.7B	1.7GB	ollama run phi
Neural Chat	7B	4.1GB	ollama run neural-chat
Starling	7B	4.1GB	ollama run starling-lm
Code Llama	7B	3.8GB	ollama run codellama
Llama 2 Uncensored	7B	3.8GB	ollama run llama2-uncensored
Llama 2 13B	13B	7.3GB	ollama run llama2:13b
Llama 2 70B	70B	39GB	ollama run llama2:70b
Orca Mini	3B	1.9GB	ollama run orca-mini
Vicuna	7B	3.8GB	ollama run vicuna
LLaVA	7B	4.5GB	ollama run llava
Note: You should have at least 8 GB of RAM available to run the 7B models, 16 GB to run the 13B models, and 32 GB to run the 33B models.

https://github.com/jmorganca/ollama

Modelos https://ollama.ai/library

Ollama is now available as an official Docker image
We are excited to share that Ollama is now available as an official Docker sponsored open-source image, making it simpler to get 
up and running with large language models using Docker containers.

With Ollama, all your interactions with large language models happen locally without sending private data to third-party services.

On the Mac
Ollama handles running the model with GPU acceleration. It provides both a simple CLI as 
well as a REST API for interacting with your applications.

To get started, simply download and install Ollama.

We recommend running Ollama alongside Docker Desktop for macOS in order 
for Ollama to enable GPU acceleration for models.

On Linux
Ollama can run with GPU acceleration inside Docker containers for Nvidia GPUs.

To get started using the Docker image, please use the commands below.

CPU only
docker run -d -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama

Nvidia GPU
Install the Nvidia container toolkit.

Run Ollama inside a Docker container
docker run -d --gpus=all -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama

Run a model
Now you can run a model like Llama 2 inside the container.

docker exec -it ollama ollama run llama2
More models can be found on the Ollama library.

	Run a model
		Now you can run a model like Llama 2 inside the container.

		docker exec -it ollama ollama run llama2


```csharp
CancellationToken cancellationToken = CancellationToken.None;
Ollama.TestConsole.Controller.OllamaChatController manager = new("http://localhost:11434", null);

// Asigna el manejador de eventos
manager.ChatReceived += (sender, args) => Console.Write(args.Message);
// Indica que se traten la respuesta según se reciba
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
				// Cambia el color para la salida del 
				Console.ForegroundColor = ConsoleColor.Cyan;
				// Llama a Ollama para obtener la respuesta
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

// Obtiene una entrada del usuario
string? GetPrompt()
{
	Console.ForegroundColor = ConsoleColor.White;
	Console.Write("> ");
	return Console.ReadLine();
}
```