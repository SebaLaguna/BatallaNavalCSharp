using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;



/// <summary>
/// Clase para manejar la elección del tablero en el juego.
/// </summary>
public class BoardHandler : BaseHandler
{

    /// <summary>
    /// Constructor de la clase BoardHandler.
    /// </summary>
    /// <param name="next">Siguiente manejador en la cadena de responsabilidad.</param>
    public BoardHandler(BaseHandler next) : base(next)
    {
        // Establece las palabras clave para este manejador
        this.Keywords = new string[] { "TableroChico", "TableroMediano", "TableroGrande" };
    }

    /// <summary>
    /// Maneja la elección del tablero de un jugador.
    /// </summary>
    /// <param name="message">Mensaje del jugador.</param>
    /// <param name="response">Respuesta al jugador.</param>
    /// <param name="reply">Teclado de respuesta.</param>
    protected override void InternalHandle(Message message, out string response, out ReplyKeyboardMarkup reply)
    {
        // Obtiene la instancia del administrador de jugadores
        PlayerAdmin admin = Singleton<PlayerAdmin>.Instance;
        // Comprueba si el jugador ya ha elegido un tablero
        if (admin.PlayerBoardChoices.ContainsKey(message.From.Id.ToString()))
        {
            // Inicializa el teclado de respuesta
            reply = new ReplyKeyboardMarkup(new[]
        {
                new[]
                {
                    new KeyboardButton("VerTablero"),
                    new KeyboardButton("Help")
                }
            });
            // Si ya ha elegido un tablero, envía un mensaje indicando que no puede elegir otro
            response = "Ya has elegido un tablero. No puedes elegir otro.";

            return;
        }
        // Guardar la elección del tablero del jugador en el diccionario
        admin.PlayerBoardChoices[message.From.Id.ToString()] = message.Text;

        reply = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton("VerTablero"),
                    new KeyboardButton("Help")
                }
            });
        // Prepara la respuesta
        response = $"Has elegido el {message.Text}. Tu elección ha sido guardada."
                    + "\n" + "Para continuar escribe el comando vertablero";

    }
}