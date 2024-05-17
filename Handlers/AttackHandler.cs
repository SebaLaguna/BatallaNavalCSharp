using System.Security.Cryptography;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static PlaceShips;
using Telegram.Bot;

/// <summary>
/// Clase para manejar los ataques en el juego.
/// </summary>
public class AttackHandler : BaseHandler
{
    private TelegramBotClient bot;

    /// <summary>
    /// Constructor de la clase AttackHandler.
    /// </summary>
    /// <param name="bot">Cliente del bot de Telegram.</param>
    /// <param name="next">Siguiente manejador en la cadena de responsabilidad.</param>
    public AttackHandler(TelegramBotClient bot, BaseHandler next) : base(next)
    {
        //Aaigna el cliente del bot de Telegram
        this.bot = bot;
        // Establece las palabras clave para este manejador
        this.Keywords = new string[] { "Attack" };
    }

    /// <summary>
    /// Maneja el ataque de un jugador.
    /// </summary>
    /// <param name="message">Mensaje del jugador.</param>
    /// <param name="response">Respuesta al jugador.</param>
    /// <param name="reply">Teclado de respuesta.</param>
    protected override void InternalHandle(Message message, out string response, out ReplyKeyboardMarkup reply)
    {

        // Inicializa el teclado de respuesta
        reply = new ReplyKeyboardMarkup(new[]
            {
            new[]
            {
                new KeyboardButton("Help")
            }
        });

        // Obtiene la instancia del administrador de jugadores
        PlayerAdmin admin = Singleton<PlayerAdmin>.Instance;
        // Imprime el ID del jugador actual
        Console.WriteLine(admin.currentPlayer.Id);
        //condicional para validar el usuario actual. Comprueba si el id del jugador actual es diferente del ID del jugador que envió el mensaje.
        if (admin.currentPlayer.Id != message.From.Id.ToString())
        {
            // Si es diferente, actualiza el jugador actual
            admin.currentPlayer = admin.ReturnCurrentReadyId(message.From.Id.ToString());
        }
        //Obtiene las coordenadas del ataque del mensaje
        string completeCordinates = message.Text.Substring(7);
        string[] cordinates = completeCordinates.Split(" ");

        // Determina quién es el oponente
        Player opponent = admin.readyPlayers[0].Id == admin.currentPlayer.Id ? admin.readyPlayers[1] : admin.readyPlayers[0];
        admin.opponentPlayer = opponent;

        // Comprueba si las coordenadas son válidas
        if (cordinates.Length < 2)
        {
            // Si no son válidas, envía un mensaje de error
            response = "Debes ingresar las coordenadas de la siguiente manera: \n" +
                        "Atacar coordenadas \n" +
                        "EJ: Attack 2 1 \n";
        }
        else
        {
            // Si son válidas, parsea las coordenadas
            int row = int.Parse(cordinates[0]);
            int col = int.Parse(cordinates[1]);

            // Se asegura de que las coordenadas estén dentro del tablero
            if (row < 0 || row >= admin.currentPlayer.AttackBoard.board.Length ||
                col < 0 || col >= admin.currentPlayer.AttackBoard.board.Length)
            {
                // Si están fuera del tablero, envía un mensaje de error
                response = "Las coordenadas están fuera del tablero. Inténtalo de nuevo.";
            }
            else
            {   
                // Asegurarse que el primero que estuvo ready sea el primero en atacar
                if (admin.turn == admin.currentPlayer.turn)
                {
                    // Ataca la posición especificada
                    string result = admin.currentPlayer.Attack(admin.opponentPlayer, row, col);

                    // Comprueba si el juego ha terminado
                    if (admin.currentPlayer.CheckGameOver(admin.opponentPlayer))
                    {
                        response = "¡Felicidades! Has hundido todas las naves del oponente y ganaste el juego.";

                        // Envía un mensaje al oponente indicando que ha perdido el juego
                        bot.SendTextMessageAsync(
                            chatId: admin.opponentPlayer.Id,
                            text: "Todas tus naves han sido hundidas. Has perdido el juego."
                        );

                    }
                    else
                    {
                        // Actualiza el tablero de ataque del jugador
                        response = admin.currentPlayer.AttackBoard.PrintBoard() + "\n\n" +
                        admin.currentPlayer.OwnBoard.PrintBoard() + "\nResultado del ataque: " + result;
                        bot.SendTextMessageAsync(
                            chatId: admin.opponentPlayer.Id,
                            text: admin.opponentPlayer.AttackBoard.PrintBoard() + "\n\n"
                                + admin.opponentPlayer.OwnBoard.PrintBoard() + "\n\n"
                                + "El enemigo te ha atacado en la posición " + row + "," + col + "\n"
                        );
                    }
                    admin.turn = admin.opponentPlayer.turn;
                }
                else
                {
                    response = "No es tu turno, espera a que el oponente realize el ataque correspondiente";
                }
            }

        }
    }
}



