using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

///<summary> El handler RegisterHandler cumple con el principio de responsabilidad única (SRP) y 
/// el principio de Expert, ya que su única responsabilidad es manejar el comando “register”. 
/// Es un ejemplo de polimorfismo y Sigue el principio de Creator al crear instancias 
/// de Player, ReplyKeyboardMarkup y KeyboardButton, y puede seguir el principio de OCP, ya que 
/// la clase BaseHandler está diseñada para ser extendida por subclases. 
/// <summary>

/// <summary>
/// Clase RegisterHandler que hereda de BaseHandler.
/// Esta clase se encarga de manejar el comando "register".
/// </summary>
public class RegisterHandler : BaseHandler
{
    /// <summary>
    /// Constructor de la clase RegisterHandler.
    /// </summary>
    /// <param name="next">El siguiente manejador en la cadena 
    /// de responsabilidad.</param>
    public RegisterHandler(BaseHandler next) : base(next)
    {
        // Este manejador se encargará del comando "register"
        this.Keywords = new string[] { "register" };
    }

    /// <summary>
    /// Método para manejar internamente el mensaje.
    /// </summary>
    /// <param name="message">El mensaje a manejar.</param>
    /// <param name="response">La respuesta generada por el manejador.</param>
    /// <param name="reply">El teclado de respuesta generado por el manejador.</param>

    protected override void InternalHandle(Message message, out string response, out ReplyKeyboardMarkup reply)
    {
        // crea una instancia de player para poder usar los metodos de player
        Player player = new Player();
        player.Name = message.From.FirstName;
        player.Id = message.From.Id.ToString();
        PlayerAdmin playerAdmin = Singleton<PlayerAdmin>.Instance; // crea una instancia de player admin para poder usar el metodo RegisterUser
        playerAdmin.RegisterUser(player); // registra al jugador en la lista de waiting players
        playerAdmin.currentPlayer = player; // le asigna el jugador actual a player

        // Crea un nuevo teclado de respuesta con opciones de tamaño de tablero y un botón de ayuda.
        reply = new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton("TableroChico"),
                new KeyboardButton("TableroMediano"),
                new KeyboardButton("TableroGrande"),
                new KeyboardButton("Help")
            }
        });
        // Genera una respuesta indicando que el jugador ha sido registrado y le pide 
        //que elija el tamaño del tablero.
        response = $"Has sido registrado con el nombre: {player.Name} y con el ID: {player.Id}\n" +
                    "Ahora se encuentra en la Waiting List\n" +
                    "A continuación elige el tamaño del tablero";
        ;
    }
}
