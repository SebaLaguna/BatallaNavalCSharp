using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

///<summary> Este handler esta diseñada para manejar específicamente 
/// la funcionalidad de “start”, lo que indica que sigue el principio de responsabilidad 
/// única (SRP). Al ser una subclase de BaseHandler y sobrescribir el método InternalHandle, 
/// demuestra polimorfismo. La clase crea instancias de ReplyKeyboardMarkup, lo
/// que sigue el patrón de Creator. Además, la clase está diseñada de tal 
/// manera que puede ser extendida (por ejemplo, creando más subclases) sin necesidad 
/// de modificar BaseHandler, lo que indica que sigue el principio de abierto/cerrado (OCP). 
/// La clase tiene una alta cohesión, ya que todos sus métodos y propiedades están 
/// estrechamente relacionados con la funcionalidad de “start”.

/// <summary>
/// La clase StartHandler es una subclase de BaseHandler.
/// </summary>
public class StartHandler : BaseHandler
{
    /// <summary>
    /// Constructor para la clase StartHandler.
    /// </summary>
    /// <param name="next">El siguiente manejador en la cadena de responsabilidad.</param>
    public StartHandler(BaseHandler next) : base(next)
    {
        /// <summary>
        /// Las palabras clave para este manejador son "/start".
        /// </summary>
        this.Keywords = new string[] { "/start" };
    }

    /// <summary>
    /// Método interno para manejar los mensajes.
    /// </summary>
    /// <param name="message">El mensaje a manejar.</param>
    /// <param name="response">La respuesta generada por el manejador.</param>
    /// <param name="reply">El teclado de respuesta rápida generado por el manejador.</param>
    protected override void InternalHandle(Message message, out string response, out ReplyKeyboardMarkup reply)
    {
        /// <summary>
        /// Creación de un nuevo teclado de respuesta rápida.
        /// </summary>
        reply = new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton("Help")
            }
        });
        // <summary>
        /// Se genera una respuesta de bienvenida para el usuario.
        /// </summary>
        response = "Bienvenido a la Batalla Naval" + "\n"
                + "Para ver la lista de comandos ingrese help"
                + "\n" + "Y disfrute del juego";

    }
}
