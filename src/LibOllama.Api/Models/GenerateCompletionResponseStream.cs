namespace Bau.Libraries.LibOllama.Api.Models;

/// <summary>
///     Stream de respuesta a <see cref="GenerateCompletionRequest"/>
/// </summary>
public class GenerateCompletionResponseStream
{
    /// <summary>
    ///     Modelo
    /// </summary>
    public string Model { get; set; } = default!;

    /// <summary>
    ///     Fecha de creación
    /// </summary>
    public string CreatedAt { get; set; } = default!;

    /// <summary>
    ///     Respuesta
    /// </summary>
    public string Response { get; set; } = default!;

    /// <summary>
    ///     Indica si ha terminado
    /// </summary>
    public bool Done { get; set; }
}