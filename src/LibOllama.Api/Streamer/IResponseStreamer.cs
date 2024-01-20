namespace Bau.Libraries.LibOllama.Api.Streamer;

/// <summary>
///     Interface para lector de una respuesta en stream
/// </summary>
public interface IResponseStreamer<T>
{
    /// <summary>
    ///     Trata los datos de un bloque de stream
    /// </summary>
    void Stream(T? stream);
}
