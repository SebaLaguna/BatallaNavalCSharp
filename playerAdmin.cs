using System.Text;


/// <summary>
/// La clase playerAdmin implementa la interfaz IAdmin y es responsable de la gestión de los jugadores 
/// en el juego de batalla naval.
/// Esta clase se adhiere al principio de responsabilidad única (SRP) de SOLID, ya que su única 
/// responsabilidad es gestionar los jugadores.
/// También sigue el principio de inversión de dependencias (DIP) de SOLID, ya que depende de la 
/// abstracción de la interfaz IAdmin y de la clase Player en lugar de detalles concretos.
/// Además, sigue el principio GRASP de bajo acoplamiento, ya que los cambios en la interfaz IAdmin y en
/// la clase Player tienen un impacto mínimo en esta clase.
/// También sigue el principio GRASP de alta cohesión, ya que todas sus responsabilidades están 
/// estrechamente relacionadas con la gestión de los jugadores.
/// </summary>
public class PlayerAdmin 
{
    internal object message;

    public Player currentPlayer { get; set; }
    public Player opponentPlayer { get; set; }
    public String finalBoardChoice { get; set; }

    public enum Turn { first , second  } // enum para saber que jugador va primero

    public Turn turn { get; set; } // variable para saber que jugador va primero
    

    /// <summary>
    /// Lista de jugadores que están esperando para jugar un partido.
    /// </summary>
    public List<Player> waitingPlayers { get; set; } = new List<Player>();
    /// <summary>
    /// Lista de jugadores que están listos para jugar un partido.
    /// </summary>
    public List<Player> readyPlayers { get; set; } = new List<Player>();
    /// <summary>
    /// Lista de identificadores de jugadores.
    /// </summary>
    public List<string> IdPlayers { get; set; }
    /// <summary>
    /// Constructor de la clase playerAdmin. Inicializa las listas de jugadores en espera, jugadores listos e identificadores de jugadores.
    /// </summary>

    public Dictionary<string, string> PlayerBoardChoices { get; set; }
    = new Dictionary<string, string>();
    public PlayerAdmin()
    {
        waitingPlayers = new List<Player>();
        readyPlayers = new List<Player>();
        IdPlayers = new List<string>();
        PlayerBoardChoices = new Dictionary<string, string>();
        currentPlayer = new Player();
        opponentPlayer = new Player();
        finalBoardChoice = "";
        turn = Turn.first;
    }
    /// <summary>
    /// Método para registrar un jugador. Añade el jugador a la lista de jugadores en espera.
    /// </summary>
    /// <param name="player">Jugador a registrar.</param>
    /// <summary>
    /// Registra a un jugador en la lista de espera para jugar un partido de batalla naval.
    /// </summary>
    /// <param name="player">Jugador a registrar.</param>
    public void RegisterUser(Player player)
    {
        if (player.PlayerStatus == Player.Status.Waiting && !IdPlayers.Contains(player.Id))
        {
            waitingPlayers.Add(player);
            IdPlayers.Add(player.Id);
            Console.WriteLine($"{player.Name} -- {player.Id} has been added to the waiting list.");
        }
        else
        {
            Console.WriteLine($"{player.Name} -- {player.Id} is not in the waiting status or already registered and cannot be added to the waiting list.");
        }
    }

    /// <summary>
    /// Registra a un jugador en la lista de jugadores listos para iniciar un partido de batalla naval.
    /// </summary>
    /// <param name="player">Jugador a registrar como listo.</param>
    public void RegisterUserReady(Player player)
    {
        if (player.PlayerStatus == Player.Status.Waiting && waitingPlayers.Contains(player))
        {
            readyPlayers.Add(player);
            waitingPlayers.Remove(player);
            Console.WriteLine($"{player.Name} -- {player.Id} has been added to the ready list and removed from waitingPlayers.");
        }
        else
        {
            if (readyPlayers.Contains(player))
            {
                Console.WriteLine($"{player.Name} -- {player.Id} is already registered and cannot be added to the ready list.");
            }
        }
    }

    /// <summary>
    /// Método para conectar a los jugadores. Selecciona dos jugadores de la lista de jugadores en 
    /// espera y los añade a la lista de jugadores listos.
    /// </summary>
    /// <param name="player">Jugador que se utilizará para actualizar las listas de jugadores.</param>
    public void refreshAllPlayer(Player player)
    {
        refreshWaitingPlayer(player);
        refreshReadyPlayer(player);
    }

    /// <summary>
    /// Actualiza la información de un jugador en la lista de jugadores en espera.
    /// </summary>
    /// <param name="player">Jugador cuya información se actualizará en la lista de jugadores en espera.</param>
    public void refreshWaitingPlayer(Player player)
    {
        for (int i = 0; i < waitingPlayers.Count; i++)
        {
            if (waitingPlayers[i].Id == player.Id)
            {
                waitingPlayers[i] = player;
            }
        }
    }

    /// <summary>
    /// Actualiza la información de un jugador en la lista de jugadores listos.
    /// </summary>
    /// <param name="player">Jugador cuya información se actualizará en la lista de jugadores listos.</param>
    public void refreshReadyPlayer(Player player)
    {
        for (int i = 0; i < readyPlayers.Count; i++)
        {
            if (readyPlayers[i].Id == player.Id)
            {
                readyPlayers[i] = player;
            }
        }
    }

    public string PrintWaitingPlayers() // metodo para imprimir la lista WaitingPlayers
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Waiting Players: \n");
        foreach (Player player in waitingPlayers)
        {
            sb.Append($"{player.Name} - {player.Id}, ");
        }
        return sb.ToString().TrimEnd(',', ' ');
    }
    /// <summary>
    /// Método para imprimir la lista de jugadores listos para iniciar un partido de batalla naval.
    /// </summary>
    /// <returns>Una cadena que representa la lista de jugadores listos.</returns>
    public string PrintReadyPlayers()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Ready Players: \n");
        foreach (Player player in readyPlayers)
        {
            sb.Append($"{player.Name} - {player.Id}, ");
        }
        return sb.ToString().TrimEnd(',', ' ');
    }

    /// <summary>
    /// Retorna el jugador actual en la lista de jugadores en espera con el ID proporcionado.
    /// </summary>
    /// <param name="id">ID del jugador.</param>
    /// <returns>El jugador con el ID proporcionado en la lista de jugadores en espera.</returns>
    public Player ReturnCurrentWaitingId(string id)
    {
        return waitingPlayers.Find(player => player.Id == id);
    }

    /// <summary>
    /// Retorna el jugador actual en la lista de jugadores listos con el ID proporcionado.
    /// </summary>
    /// <param name="id">ID del jugador.</param>
    /// <returns>El jugador con el ID proporcionado en la lista de jugadores listos.</returns>
    public Player ReturnCurrentReadyId(string id)
    {
        return readyPlayers.Find(player => player.Id == id);
    }

    /// <summary>
    /// Retorna el jugador actual en la lista de jugadores no listos con el ID proporcionado.
    /// </summary>
    /// <param name="id">ID del jugador.</param>
    /// <returns>El jugador con el ID proporcionado en la lista de jugadores no listos.</returns>
    public Player ReturnCurrentNotReadyId(string id)
    {
        if (readyPlayers[0] == readyPlayers.Find(player => player.Id == id))
        {
            return readyPlayers[0];
        }
        else
        {
            return readyPlayers[1];
        }
    }


    /// <summary>
    /// Determina la elección final del tablero basándose en las elecciones de los jugadores.
    /// </summary>
    /// <remarks>
    /// Asegúrate de que ambos jugadores hayan hecho su elección antes de llamar a este método.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si ambos jugadores no han hecho su elección antes de determinar la elección final del tablero.
    /// </exception>
    public void GetFinalBoardChoice()
    {
        // Asegúrate de que ambos jugadores hayan hecho su elección
        if (PlayerBoardChoices.Count < 2)
        {
            throw new InvalidOperationException("Ambos jugadores deben hacer su elección antes de determinar la elección final del tablero.");
        }

        // Obtiene las elecciones de los jugadores
        var choices = PlayerBoardChoices.Values.ToList();
        var firstChoice = choices[0];
        var secondChoice = choices[1];

        // Si las elecciones son iguales, usa esa elección como la opción final
        if (firstChoice == secondChoice)
        {
            finalBoardChoice = firstChoice;
        }
        else
        {
            // Si son diferentes, se elige el tablero que eligió el jugador que se unió primero a la partida
            finalBoardChoice = firstChoice;
        }
    }



}





