using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

///<summary> Este handler cumple con la unica funcionalidad “VerTablero”, 
/// cumpliendo con el principio SRP. Es una subclase de BaseHandler y sobrescribe el método 
/// InternalHandle, lo que es un ejemplo de polimorfismo. Crea instancias de GameBoard y 
/// ReplyKeyboardMarkup, siguiendo el patrón de Creator, y está abierta para la extensión 
/// pero cerrada para la modificación, cumpliendo con el OCP.


/// <summary>
/// La clase SeeBoardHandler es una subclase de BaseHandler.
/// </summary>
public class SeeBoardHandler : BaseHandler
{
    /// <summary>
    /// Constructor para la clase SeeBoardHandler.
    /// </summary>
    /// <param name="next">El siguiente manejador en la cadena de responsabilidad.</param>
    public SeeBoardHandler(BaseHandler next) : base(next)
    {
        /// <summary>
        /// Las palabras clave para este manejador son "VerTablero".
        /// </summary>
        this.Keywords = new string[] { "VerTablero" };
    }

    /// <summary>
    /// Método interno para manejar los mensajes.
    /// </summary>
    /// <param name="message">El mensaje a manejar.</param>
    /// <param name="response">La respuesta generada por el manejador.</param>
    /// <param name="reply">El teclado de respuesta rápida generado por el manejador.</param>
    protected override void InternalHandle(Message message, out string response, out ReplyKeyboardMarkup reply)
    {
        try
        {
            /// <summary>
            /// Creación de un nuevo teclado con un boton de ayuda, ver barcos y ver tablero.
            /// </summary>
            reply = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton("Help"),
                    new KeyboardButton("VerBarcos"),
                    new KeyboardButton("VerTablero")
                }
            });

            /// <summary>
            /// Creación de una instancia del administrador de jugadores.
            /// </summary>
            PlayerAdmin admin = Singleton<PlayerAdmin>.Instance;
            /// <summary>
            /// Si el jugador actual no es el que envió el mensaje, se cambia al jugador que envió el mensaje.
            /// </summary>
            if (admin.currentPlayer.Id != message.From.Id.ToString())
            {
                admin.currentPlayer = admin.ReturnCurrentWaitingId(message.From.Id.ToString());
            }
            string name = message.From.FirstName; //tomamos el nombre de usuario y lo guardamos en name

            /// <summary>
            /// Se toma la elección final del tablero.
            /// </summary>
            var finalChoice = admin.finalBoardChoice;

            /// </summary>
            //tomamos el id de usuario y lo guardamos en id
            /// </summary>
            string id = message.From.Id.ToString();
            if (admin.PlayerBoardChoices.Count < 2)
            {
                response = "Espera por favor a que su rival elija su tablero";
                return;
            }
            else
            {
                /// <summary>
                /// Si no se ha hecho una elección final de tablero, se obtiene la elección final de tablero.
                /// </summary>
                if (finalChoice == "")
                {
                    admin.GetFinalBoardChoice();
                    finalChoice = admin.finalBoardChoice;
                    finalChoice = finalChoice.ToLower();
                    Console.WriteLine(finalChoice);
                }
                else
                {
                    finalChoice = finalChoice.ToLower();
                    Console.WriteLine(finalChoice);
                }
            }
            /// <summary>
            /// Dependiendo de la elección final del tablero, se crea un tablero de un tamaño específico.
            /// </summary>
            switch (finalChoice)
            {
                case "tablerochico":
                    admin.currentPlayer.AttackBoard = new GameBoard(5);
                    admin.currentPlayer.OwnBoard = new GameBoard(5);
                    response = $"El tablero es chico\n\n {admin.currentPlayer.OwnBoard.PrintBoard()} \n\n {admin.currentPlayer.AttackBoard.PrintBoard()}"
                                + "Para ver sus barcos escriba VerBarcos";
                    admin.refreshWaitingPlayer(admin.currentPlayer);

                    break;
                case "tableromediano":
                    admin.currentPlayer.OwnBoard = new GameBoard(7);
                    admin.currentPlayer.AttackBoard = new GameBoard(7);
                    response = $"El tablero es mediano\n\n {admin.currentPlayer.OwnBoard.PrintBoard()} \n\n {admin.currentPlayer.AttackBoard.PrintBoard()}"
                                + "Para ver sus barcos escriba VerBarcos";
                    admin.refreshWaitingPlayer(admin.currentPlayer);
                    break;
                case "tablerogrande":
                    admin.currentPlayer.OwnBoard = new GameBoard(9);
                    admin.currentPlayer.AttackBoard = new GameBoard(9);
                    response = $"El tablero es grande\n\n {admin.currentPlayer.OwnBoard.PrintBoard()} \n\n {admin.currentPlayer.AttackBoard.PrintBoard()}"
                                + "Para ver sus barcos escriba VerBarcos";
                    admin.refreshWaitingPlayer(admin.currentPlayer);
                    break;
                default:
                    response = "XXXXX";
                    break;
            }
        }
        catch (Exception ex)
        {
            response = $"Se produjo un error: {ex.Message}";
            reply = null;
        }
    }
}
