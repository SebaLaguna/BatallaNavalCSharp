using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

/// <summary> /// IncorrectCommandHandler sigue el principio de responsabilidad 
/// única (SRP) y el principio de Expert, ya que su única responsabilidad es manejar 
/// los comandos incorrectos y tiene la información necesaria para hacerlo. Es un ejemplo 
/// de polimorfismo y sigue el principio de sustitución de Liskov (LSP), ya que puede ser 
/// sustituida por su clase base sin afectar el comportamiento del programa. Sigue el principio 
/// de Creator al crear instancias de ReplyKeyboardMarkup y KeyboardButton, y también sigue el 
/// principio de OCP, ya que la clase BaseHandler está diseñada para ser extendida 
/// por subclases. /// </summary>

/// <summary>
/// Clase IncorrectCommandHandler que hereda de BaseHandler.
/// Esta clase se encarga de manejar los comandos incorrectos.
/// </summary>
public class IncorrectCommandHandler : BaseHandler
{
    /// <summary>
    /// Constructor de la clase IncorrectCommandHandler.
    /// </summary>
    /// <param name="next">El siguiente manejador en la cadena de responsabilidad.</param>
    public IncorrectCommandHandler(BaseHandler next) : base(next)
    {
        this.Keywords = new string[] { }; // Este handler manejará todos los comandos que no son manejados por otros handlers
    }

    // <summary>
    /// Método para determinar si este manejador puede manejar el mensaje dado.
    /// </summary>
    /// <param name="message">El mensaje a manejar.</param>
    /// <returns>Verdadero, ya que este manejador puede manejar cualquier mensaje.</returns>
    protected override bool CanHandle(Message message)
    {
        return true; // Este handler puede manejar cualquier mensaje
    }

    /// <summary>
    /// Método para manejar internamente el mensaje.
    /// </summary>
    /// <param name="message">El mensaje a manejar.</param>
    /// <param name="response">La respuesta generada por el manejador.</param>
    /// <param name="reply">El teclado de respuesta generado por el manejador.</param>
    protected override void InternalHandle(Message message, out string response, out ReplyKeyboardMarkup reply)
    {
        // Crear un nuevo teclado de respuesta con un botón de ayuda.
        reply = new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton("Help")
            }
        });
        // Generar una respuesta indicando que el comando es incorrecto y sugerir escribir help para ver la lista de comandos disponibles.
        response = "Has escrito un comando incorrecto, por favor escribe help para ver la lista de comandos disponibles";

    }
}