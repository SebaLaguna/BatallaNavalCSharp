using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;


/// <summary> El handler SeeShipsHandler tiene como unica responsabilidad mostrar los barcos del 
/// jugador actual “VerBarcos”, cumpliendo con el principio SRP. Es una subclase de BaseHandler y sobrescribe
///  el método InternalHandle, lo que es un ejemplo de polimorfismo. Crea instancias de GameBoard 
/// y ReplyKeyboardMarkup, siguiendo el patrón de Creator, y está abierta para la extensión pero 
/// cerrada para la modificación, cumpliendo con el OCP. 
/// </summary>

/// <summary>
/// La clase SeeShipsHandler es una subclase de BaseHandler.
/// </summary>
public class SeeShipsHandler : BaseHandler
{
    /// <summary>
    /// Constructor para la clase SeeShipsHandler.
    /// </summary>
    /// <param name="next">El siguiente manejador en la cadena de responsabilidad.</param>
    public SeeShipsHandler(BaseHandler next) : base(next)
    {
        /// <summary>
        /// Las palabras clave para este manejador son "VerBarcos".
        /// </summary>
        this.Keywords = new string[] { "VerBarcos" };
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
                new KeyboardButton("Help"),
                new KeyboardButton("VerBarcos"),
                new KeyboardButton("VerTablero"),
            }
        });

        /// <summary>
        /// Creación de una instancia del administrador de jugadores.
        /// </summary>
        PlayerAdmin admin = Singleton<PlayerAdmin>.Instance;
        // var board = Singleton<PlayerAdmin>.Instance.PlayerBoardChoices[message.From.Id.ToString()];
        // board = board.ToLower();

        /// <summary>
        /// Si el jugador actual no es el que envió el mensaje, se cambia al jugador que envió el mensaje.
        /// </summary>
        if (admin.currentPlayer.Id != message.From.Id.ToString())//condicional para validar el usuario actual
        {
            admin.currentPlayer = admin.ReturnCurrentWaitingId(message.From.Id.ToString());
        }
        /// <summary>
        /// Creación de los barcos con emojis.
        /// </summary>
        string barco2 = string.Concat(Enumerable.Repeat("🟩", 2));
        string barco3 = string.Concat(Enumerable.Repeat("🟩", 3));
        string barco4 = string.Concat(Enumerable.Repeat("🟩", 4));
        string barco5 = string.Concat(Enumerable.Repeat("🟩", 5));
        if (admin.PlayerBoardChoices.Count < 2)
        {
            response = "Espere por favor a que su rival elija su tablero";
        }
        else
        {

            /// <summary>
            /// Si no se han hecho suficientes elecciones de tablero, se le pide al usuario que espere.
            /// </summary>

            // Obtiene la elección final del tablero
            var finalChoice = admin.finalBoardChoice;
            finalChoice = finalChoice.ToLower();
            Console.WriteLine(finalChoice);

            /// <summary>
            /// Dependiendo de la elección final del tablero, se crean los barcos correspondientes.
            /// </summary>

            switch (finalChoice)
            {
                case "tablerochico":
                    if (admin.currentPlayer.Barcos.Count == 0)
                    {
                        admin.currentPlayer.Barcos.Add($"Cañonera: {barco3}\n\n");
                        admin.currentPlayer.Barcos.Add($"Lancha: {barco2}\n\n");
                        admin.currentPlayer.Barcos.Add($"Lancha: {barco2}\n\n");
                        admin.refreshAllPlayer(admin.currentPlayer);
                    }
                    response = $"Esta es su flota \n\n" +
                                admin.currentPlayer.PrintShip() +
                                "Para continuar debe cambiar su estado con el comando ready";
                    break;
                case "tableromediano":
                    if (admin.currentPlayer.Barcos.Count == 0)
                    {
                        admin.currentPlayer.Barcos.Add($"Destructor: {barco4}\n\n");
                        admin.currentPlayer.Barcos.Add($"Cañonera: {barco3}\n\n");
                        admin.currentPlayer.Barcos.Add($"Lancha: {barco2}\n\n");
                        admin.currentPlayer.Barcos.Add($"Lancha: {barco2}\n\n");
                        admin.refreshAllPlayer(admin.currentPlayer);
                    }
                    response = $"Esta es su flota \n\n" +
                               admin.currentPlayer.PrintShip() +
                               "Para continuar debe cambiar su estado con el comando ready";
                    break;
                case "tablerogrande":
                    if (admin.currentPlayer.Barcos.Count == 0)
                    {
                        admin.currentPlayer.Barcos.Add($"Portaaviones: {barco5}\n\n");
                        admin.currentPlayer.Barcos.Add($"Destructor: {barco4}\n\n");
                        admin.currentPlayer.Barcos.Add($"Cañonera: {barco3}\n\n");
                        admin.currentPlayer.Barcos.Add($"Lancha: {barco2}\n\n");
                        admin.currentPlayer.Barcos.Add($"Lancha: {barco2}\n\n");
                        admin.refreshAllPlayer(admin.currentPlayer);
                    }
                    response = $"Esta es su flota \n\n" +
                               admin.currentPlayer.PrintShip() +
                                "Para continuar debe cambiar su estado con el comando ready";
                    break;
                default:
                    response = "No se ha elegido un tablero";
                    break;
            }
        }
    }
}
