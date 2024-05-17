using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

/// <summary> Este handler esta diseñado para manejar específicamente 
/// la funcionalidad de “Show”, lo que sigue el principio de responsabilidad única 
/// (SRP). Al ser una subclase de BaseHandler y sobrescribir el método InternalHandle, 
/// demuestra polimorfismo. La clase crea instancias de ReplyKeyboardMarkup, lo que hace 
/// que siga el patrón de Creator. Además, la clase está diseñada de tal manera que puede 
/// ser extendida (por ejemplo, creando más subclases) sin necesidad de modificar BaseHandler, 
/// lo que indica que sigue el principio de abierto/cerrado (OCP).
/// </summary>

/// <summary>
/// La clase ShowPlayersHandler es una subclase de BaseHandler.
/// </summary>
public class ShowPlayersHandler : BaseHandler
{
    /// <summary>
    /// Constructor para la clase ShowPlayersHandler.
    /// </summary>
    /// <param name="next">El siguiente manejador en la cadena de responsabilidad.</param>
    public ShowPlayersHandler(BaseHandler next) : base(next)
    {
        /// <summary>
        /// Las palabras clave para este manejador son "Show". Este manejador 
        /// mostrará la lista de jugadores en la lista de espera.
        /// </summary>
        this.Keywords = new string[] { "Show" }; // Este handler mostrara la lista de jugadores en waiting list
    }


    /// <summary>
    /// Método interno para manejar los mensajes.
    /// </summary>
    /// <param name="message">El mensaje a manejar.</param>
    /// <param name="response">La respuesta generada por el manejador.</param>
    /// <param name="reply">El teclado de respuesta rápida generado por el manejador.</param>
    protected override void InternalHandle(Message message, out string response, out ReplyKeyboardMarkup reply)
    {
        PlayerAdmin admin = Singleton<PlayerAdmin>.Instance; // Obtener la instancia actual de playerAdmin

        // <summary>
        /// Si hay jugadores en la lista de espera o en la lista de jugadores listos, se imprime la lista de jugadores.
        /// </summary>
        if (admin.waitingPlayers.Count + admin.readyPlayers.Count > 0)// verifica que la lista de waiting players no este vacia
        {
            if (admin.readyPlayers.Count >= 2)
            {
                Match match = new Match(admin.readyPlayers[0], admin.readyPlayers[1]);
                match.currentMatches.Add(admin.readyPlayers[0]);
                match.currentMatches.Add(admin.readyPlayers[1]);
                response = admin.PrintWaitingPlayers() + "\n" + admin.PrintReadyPlayers() + "\n" + match.PrintCurrentMatches();
            }
            else
            {
                response = admin.PrintWaitingPlayers() + "\n" + admin.PrintReadyPlayers();
            }
        }
        else
        {
            /// <summary>
            /// Si no hay jugadores en la lista de espera, se informa al usuario.
            /// </summary>
            response = "No hay jugadores en la lista de espera.";
        }
        {

            /// <summary>
            /// Creación de un nuevo teclado con el botón "Help".
            /// </summary>

            reply = new ReplyKeyboardMarkup(new[]
            {
            new[]
            {
                new KeyboardButton("Help")
            }
        });
        }

    }
}
