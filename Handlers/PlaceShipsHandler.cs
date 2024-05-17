using System.Security.Cryptography;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static PlaceShips;

/// <summary>El handler PlaceShipsHandler sigue el principio de responsabilidad única (SRP) 
/// y el principio de Expert, ya que su única responsabilidad es manejar el comando 
/// “ColocarBarcos” y tiene la información necesaria para hacerlo. Es un ejemplo de 
/// polimorfismo y sigue el principio de sustitución de Liskov (LSP), ya que puede ser 
/// sustituida por su clase base sin afectar el comportamiento del programa. También sigue el 
/// principio de Creator al crear instancias de ReplyKeyboardMarkup y KeyboardButton.
/// <summary>

/// <summary>
/// Handler PlaceShipsHandler que hereda de BaseHandler.
/// Esta clase se encarga de manejar el comando "ColocarBarcos".
/// </summary>
public class PlaceShipsHandler : BaseHandler
{
    /// <summary>
    /// Constructor de la clase PlaceShipsHandler.
    /// </summary>
    /// <param name="next">El siguiente manejador en la cadena de responsabilidad.</param>
    public PlaceShipsHandler(BaseHandler next) : base(next)
    {
        // Este manejador se encargará del comando "ColocarBarcos"
        this.Keywords = new string[] { "ColocarBarcos" };
    }

    /// <summary>
    /// Método para manejar internamente el mensaje.
    /// </summary>
    /// <param name="message">El mensaje a manejar.</param>
    /// <param name="response">La respuesta generada por el manejador.</param>
    /// <param name="reply">El teclado de respuesta generado por el manejador.</param>
    protected override void InternalHandle(Message message, out string response, out ReplyKeyboardMarkup reply)
    {

        // Obtiene la instancia del administrador de jugadores.
        PlayerAdmin admin = Singleton<PlayerAdmin>.Instance;
        Console.WriteLine(admin.currentPlayer.Id);

        if (admin.currentPlayer.Id != message.From.Id.ToString())//condicional para validar el usuario actual
        {
            admin.currentPlayer = admin.ReturnCurrentReadyId(message.From.Id.ToString());
        }
        // Extrae las coordenadas y la orientación del barco del mensaje.
        string completeCordinates = message.Text.Substring(14);
        string[] cordinates = completeCordinates.Split(" ");
        Orientation orientation = Orientation.Horizontal;
        // Valida si el mensaje tiene la estructura correcta
        if (cordinates.Length < 3)
        {
            response = "Debes ingresar los datos de la siguiente manera: \n" +
                        "ColocarBarcos tipo coordenadas orientación \n" +
                        "EJ: ColocarBarcos Lancha 21 vertical \n";
        }
        else
        {
            // Valida si el jugador tiene el barco que quiere colocar
            if (admin.currentPlayer.Barcos.Count != 0 && !(admin.currentPlayer.haveShip(cordinates[0])))
            {
                response = $"No cuenta con {cordinates[0]}, intente con otro barco";
            }
            else
            {
                // Establece la orientación del barco.
                if (cordinates[2] == "vertical")
                {
                    orientation = Orientation.Vertical;

                }
                // Intenta colocar el barco en el tablero.
                try
                {
                    // Valida si el jugador ya ha colocado todos sus barcos.
                    if (admin.currentPlayer.Barcos.Count == 0)
                    {
                        // Si ambos jugadores han colocado todos sus barcos, se puede iniciar el juego.
                        if (admin.ReturnCurrentNotReadyId(admin.currentPlayer.Id).ShipsReady && admin.readyPlayers.Count == 2)
                        {
                            Console.WriteLine(admin.ReturnCurrentNotReadyId(admin.currentPlayer.Id).Id);
                            response = "Ambos jugadores han terminado con sus barcos, ingrese attack y una coordenada para iniciar, por ejemplo 'attack 3 3' atacara la posición 3,3";
                        }
                        else
                        {
                            response = "Ya ha colocado todos sus barcos, espere a que el otro jugador termine";
                        }
                    }
                    else
                    {
                        // Crea una nueva instancia de PlaceShips y coloca el barco en el tablero.
                        PlaceShips placeShips = new PlaceShips(cordinates[0], cordinates[1], orientation);

                        if (admin.currentPlayer.OwnBoard.board[placeShips.CordinateX - 1, placeShips.CordinateY - 1] == "🟩")
                        {
                            response = "Ya hay un barco colocado en esta posición.";
                        }
                        else
                        {
                            admin.currentPlayer.PlaceShip(placeShips);
                            Console.WriteLine(admin.readyPlayers.Count);
                            if (admin.currentPlayer.Id == admin.readyPlayers[0].Id)
                            {
                                admin.readyPlayers[0] = admin.currentPlayer;
                            }
                            else if (admin.currentPlayer.Id == admin.readyPlayers[1].Id)
                            {
                                admin.readyPlayers[1] = admin.currentPlayer;
                            }
                            response = admin.currentPlayer.OwnBoard.PrintBoard();

                            // Si el jugador ha colocado todos sus barcos, se marca como listo.
                            if (admin.currentPlayer.Barcos.Count == 0)
                            {
                                admin.currentPlayer.ShipsReady = true;
                                admin.refreshReadyPlayer(admin.currentPlayer);
                                if (admin.ReturnCurrentNotReadyId(admin.currentPlayer.Id).ShipsReady && admin.readyPlayers.Count == 2)
                                {
                                    Console.WriteLine(admin.ReturnCurrentNotReadyId(admin.currentPlayer.Id).Id);
                                    response = response + "\nAmbos jugadores han terminado con sus barcos, ingrese attack y una coordenada para iniciar, por ejemplo 'attack 3 3' atacara la posición 3,3";
                                }
                                else
                                {
                                    response = response + "\nYa ha colocado todos sus barcos, espere a que el otro jugador termine";
                                }
                            }
                        }
                    }
                }
                //fin del try
                // Captura la excepción en caso de que las coordenadas estén fuera de rango.
                catch (System.IndexOutOfRangeException e)
                {
                    response = "las coordenadas ingresadas estan fuera de rango";
                }
            }
        }
        // Crea un nuevo teclado de respuesta con el botón de ayuda
        reply = new ReplyKeyboardMarkup(new[]
                {
                    new[]
                    {
                        new KeyboardButton("Help")
                    }
                });
        return;
    }
}