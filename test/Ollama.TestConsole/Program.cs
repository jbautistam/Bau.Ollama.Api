/*
	Instrucciones para ejecución en Docker: https://ollama.ai/blog/ollama-is-now-available-as-an-official-docker-image

	CPU only
		docker run -d -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama

	Nvidia GPU
		Install the Nvidia container toolkit.
		Run Ollama inside a Docker container
		docker run -d --gpus=all -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama
	Run a model
		Now you can run a model like Llama 2 inside the container.

		docker exec -it ollama ollama run llama2
*/

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