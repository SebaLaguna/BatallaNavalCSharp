using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

/// <summary> El handler ReadyHandler sigue el principio de responsabilidad única (SRP) y el 
/// principio de Expert, ya que su única responsabilidad es manejar el comando “ready” y 
/// tiene la información necesaria para hacerlo. Es un ejemplo de polimorfismo y sigue el 
/// principio de sustitución de Liskov (LSP), ya que puede ser sustituida por su clase base 
/// sin afectar el comportamiento del programa. Sigue el principio de Creator al crear 
/// instancias de ReplyKeyboardMarkup y KeyboardButton.
/// <summary>

/// <summary>
/// Clase ReadyHandler que hereda de BaseHandler.
/// Esta clase se encarga de manejar el comando "ready".
/// </summary>
public class ReadyHandler : BaseHandler
{
    /// <summary>
    /// Constructor de la clase ReadyHandler.
    /// </summary>
    /// <param name="next">El siguiente manejador en la cadena de responsabilidad.</param>
    public ReadyHandler(BaseHandler next) : base(next)
    {
        // Este manejador se encargará del comando "ready"
        this.Keywords = new string[] { "ready" };
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

        // Valida si el jugador actual es el que envió el mensaje.
        if (admin.currentPlayer.Id != message.From.Id.ToString())
        {
            admin.currentPlayer = admin.ReturnCurrentWaitingId(message.From.Id.ToString());
        }


        // Valida si el jugador actual existe.
        if (admin.currentPlayer != null)
        {
            // Valida si el jugador ya está en la lista de jugadores listos
            if (admin.readyPlayers.Contains(admin.currentPlayer))
            {
                response = "Ya estas en la lista de readyPlayers, porfavor espera a que comience el juego";
                reply = new ReplyKeyboardMarkup(new[]
                {
                    new[]
                    {
                        new KeyboardButton("Help")
                    }
                });
                return;
            }
            else
            {
                // Registra al jugador como listo y genera una respuesta.
                Player player = admin.currentPlayer;
                admin.RegisterUserReady(player);
                if (admin.readyPlayers.Count == 1)// validador que le da al primero que ingrese a la lista de readyPlayers el turno de atacar primero
                {
                    admin.currentPlayer.turn = PlayerAdmin.Turn.first;
                    response = "Su estado ha cambiado a ready y se encuentra en la lista readyPlayers \n" +
                            "para continuar debe colocar los barcos con el comando colocar (nombre del barco) (coordenada inicial) (orientación) \n" +
                            "Ejemplo: colocar lancha 22 horizontal \n" + 
                            "TIENES EL PRIMER TURNO PARA ATACAR";
                }
                else 
                {
                    admin.currentPlayer.turn = PlayerAdmin.Turn.second;
                    response = "Su estado ha cambiado a ready y se encuentra en la lista readyPlayers \n" +
                            "para continuar debe colocar los barcos con el comando colocar (nombre del barco) (coordenada inicial) (orientación) \n" +
                            "Ejemplo: colocar lancha 22 horizontal \n" + 
                            "TIENES EL SEGUNDO TURNO PARA ATACAR";
                }
                admin.refreshReadyPlayer(admin.currentPlayer);
                
            }
        }
        else
        {
            // Si el jugador no existe, se le pide que se registre.
            Console.WriteLine($"{message.From.FirstName} \n" +
                            $"{message.From.Id.ToString()} no esta registrado aun, no puede cambiar su estado a ready");
            response = "Debes registrarte primero para poder jugar  \n" +
                        "Escribe register para registrarte";
        }

        // Crea un nuevo teclado de respuesta con el botón de ayuda
        reply = new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton("Help")
            }
        });

    }
}
