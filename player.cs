using System.Security;
using System.Text;
using Telegram.Bot.Types;

/// <summary>
/// La clase Player representa a un jugador en el juego de batalla naval.
/// Esta clase se adhiere al principio de responsabilidad única (SRP) de SOLID, ya que su única 
/// responsabilidad es gestionar el estado y el comportamiento de un jugador.
/// También sigue el principio GRASP de alta cohesión, ya que todas sus responsabilidades están 
/// estrechamente relacionadas con las actividades de un jugador.
/// </summary>
public class Player
{
    /// <summary>
    /// Enumeración para el estado del jugador.
    /// </summary>
    public enum Status
    {
        Ready,
        Waiting
    }
    //<summary>
    ///variable booleana para controlar que el jugador ya asigno todos sus barcos
    ///</summary>
    public bool ShipsReady { get; set; }
    /// <summary>
    /// Nombre del jugador.
    /// </summary>
    public PlayerAdmin.Turn turn { get; set; }
    
    public string Name { get; set; }
    /// <summary>
    /// ID del jugador.
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Tablero propio del jugador.
    /// </summary>
    public GameBoard? OwnBoard { get; set; }
    /// <summary>
    /// Tablero de ataque del jugador.
    /// </summary>
    public GameBoard? AttackBoard { get; set; }
    /// <summary>
    /// Estado del jugador.
    /// </summary>
    public Status PlayerStatus { get; set; }
    public List<string> Barcos { get; set; }

    public List<Ship> Fleet { get; set; }
    /// <summary>
    /// Constructor de la clase Player. Inicializa el nombre del jugador, la lista de barcos y el 
    /// estado del jugador.
    /// </summary>
    /// <param name="name">El nombre del jugador.</param>
    public Player()
    {
        Barcos = new List<string>();
        PlayerStatus = Status.Waiting;
        OwnBoard = new GameBoard(1);
        AttackBoard = new GameBoard(1);
        ShipsReady = false;
        Fleet = new List<Ship>();
    }

    /// <summary>
    /// Método para atacar a otro jugador.
    /// </summary>
    /// <param name="opponent">Jugador oponente.</param>
    /// <param name="x">Coordenada x del ataque.</param>
    /// <param name="y">Coordenada y del ataque.</param>
    public string Attack(Player opponent, int x, int y)
    {
        if (opponent.IsSunk(x - 1, y - 1))
        {
            return "Ese barco ya esta hundido";
        }
        else
        {
            x -= 1;
            y -= 1;
            // Asegúrate de que las coordenadas estén dentro del tablero
            if (x < 0 || x >= opponent.OwnBoard.board.Length ||
                y < 0 || y >= opponent.OwnBoard.board.Length)
            {
                return "Las coordenadas están fuera del tablero.";
            }

            // Comprueba si el ataque golpeó un barco
            if (opponent.OwnBoard.board[x, y] == "🟩")
            {
                // Marca la posición como golpeada
                opponent.OwnBoard.board[x, y] = "🟥";
                AttackBoard.board[x, y] = "🟥";
                foreach (Ship ship in opponent.Fleet)
                {
                    foreach (var item in ship.Positions)
                    {
                        if (item == $"{x},{y}")
                        {
                            ship.Hit();
                        }
                    }
                }
                if (opponent.IsSunk(x, y))
                {
                    return "¡Hundiste un barco!";
                }
                else
                {
                    return "¡Golpeaste un barco!";
                }
            }
            else if (opponent.OwnBoard.board[x, y] == "🟥")
            {
                return "Ya habías atacado en esta posición.";
            }
            else
            {
                // Marca la posición como agua
                opponent.OwnBoard.board[x, y] = "🟪";
                AttackBoard.board[x, y] = "🟪";
                return "Fallaste. Golpeaste el agua.";
            }
        }


    }

    /// <summary>
    /// Genera una cadena que representa la lista de tipos de barcos disponibles.
    /// </summary>
    /// <returns>
    /// Una cadena que contiene los tipos de barcos disponibles, cada uno en una nueva línea.
    /// </returns>
    /// <remarks>
    /// Este método itera sobre la lista de tipos de barcos disponibles y los agrega a una cadena, separados por nuevas líneas.
    /// La cadena resultante representa los tipos de barcos que aún no han sido colocados en el tablero del jugador.
    /// </remarks>
    /// <returns>Una cadena que contiene los tipos de barcos disponibles, cada uno en una nueva línea.</returns>
    public string PrintShip()
    {
        StringBuilder respuesta = new StringBuilder();
        for (int i = 0; i < Barcos.Count; i++)
        {
            respuesta.Append(Barcos[i]).Append("\n");
        }
        return respuesta.ToString();
    }

    /// <summary>
    /// Método para colocar los barcos en el tablero del jugador.
    /// </summary>
    public void PlaceShip(PlaceShips placeShips)
    {
        // Convierte la coordenada de inicio en índices de fila y columna
        int row = placeShips.CordinateX - 1;
        int col = placeShips.CordinateY - 1;

        // Determina la longitud del barco basándose en su tipo
        int shipLength;
        switch (placeShips.Tipo)
        {
            case "lancha":
                shipLength = 2;
                break;
            case "cañonera":
                shipLength = 3;
                break;
            case "destructor":
                shipLength = 4;
                break;
            case "portaaviones":
                shipLength = 5;
                break;
            default:
                throw new ArgumentException("Tipo de barco no válido");
        }
        Fleet.Add(new Ship(placeShips.Tipo, shipLength));

        // Coloca el barco en el tablero
        for (int i = 0; i < shipLength; i++)
        {
            if (placeShips.orientation == PlaceShips.Orientation.Horizontal)
            {
                OwnBoard.board[row, col + i] = "🟩";
                foreach (Ship ship in Fleet)
                {
                    if (ship.Type == placeShips.Tipo)
                    {
                        ship.AddPosition($"{row-1},{col + i-1}");
                    }
                }
            }
            else // Orientación vertical
            {
                OwnBoard.board[row + i, col] = "🟩";
                foreach (Ship ship in Fleet)
                {
                    if (ship.Type == placeShips.Tipo)
                    {
                        ship.AddPosition($"{row + i-1},{col-1}");
                    }
                }
            }

        }
        for (int i = 0; i < Barcos.Count; i++)
        {
            if (Barcos[i].ToLower().Contains(placeShips.Tipo))
            {
                Barcos.RemoveAt(i);
            }
        }
        
    }
    /// <summary>
    /// Verifica si el jugador tiene un tipo específico de barco.
    /// </summary>
    /// <param name="barco">El tipo de barco a verificar.</param>
    /// <returns>
    /// <c>true</c> si el jugador tiene al menos un barco del tipo especificado; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método busca en la lista de barcos del jugador para determinar si tiene al menos un barco del tipo especificado.
    /// </remarks>
    /// <param name="barco">Tipo de barco a verificar.</param>
    /// <returns>True si el jugador tiene al menos un barco del tipo especificado; de lo contrario, false.</returns>
    public bool haveShip(string barco)
    {
        // Itera sobre la lista de barcos para verificar si el jugador tiene el tipo de barco especificado
        for (int i = 0; i < Barcos.Count; i++)
        {
            if (Barcos[i].ToLower().Contains(barco))
            {
                return true; // El jugador tiene al menos un barco del tipo especificado
            }
        }
        return false; // El jugador no tiene barcos del tipo especificado
    }

    /// <summary>
    /// Método para solicitar el inicio del juego.
    /// </summary>
    public void RequestStart()
    {
        PlayerStatus = Status.Ready;
    }

    /// <summary>
    /// Verifica si hay un barco hundido en las coordenadas especificadas.
    /// </summary>
    /// <param name="x">La coordenada X en el tablero.</param>
    /// <param name="y">La coordenada Y en el tablero.</param>
    /// <returns>
    /// <c>true</c> si hay un barco hundido en las coordenadas especificadas; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método itera sobre la flota de barcos del jugador para determinar si hay un barco en las coordenadas especificadas
    /// y si ese barco ha sido completamente hundido.
    /// </remarks>
    /// <param name="x">Coordenada X en el tablero.</param>
    /// <param name="y">Coordenada Y en el tablero.</param>
    /// <returns>True si hay un barco hundido en las coordenadas especificadas; de lo contrario, false.</returns>
    public bool IsSunk(int x, int y)
    {
        // Itera sobre la flota de barcos del jugador
        foreach (Ship ship in Fleet)
        {
            // Itera sobre las posiciones del barco
            foreach (var item in ship.Positions)
            {
                // Verifica si las coordenadas especificadas coinciden con alguna posición del barco
                if (item == $"{x},{y}")
                {
                    // Verifica si el barco ha sido completamente hundido
                    if (ship.Life == 0)
                    {
                        return true; // Hay un barco hundido en las coordenadas especificadas
                    }
                }
            }
        }
        return false; // No hay un barco hundido en las coordenadas especificadas
    }



   /// <summary>
    /// Verifica si el juego ha terminado al comprobar si todas las naves del oponente han sido hundidas.
    /// </summary>
    /// <param name="opponent">El jugador oponente.</param>
    /// <returns>
    /// <c>true</c> si todas las naves del oponente han sido hundidas; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método itera sobre el tablero propio del oponente para determinar si hay alguna posición que aún contiene un barco.
    /// Si no hay barcos en el tablero del oponente, se considera que todas las naves han sido hundidas y el juego ha terminado.
    /// </remarks>
    /// <param name="opponent">El jugador oponente.</param>
    /// <returns>True si todas las naves del oponente han sido hundidas; de lo contrario, false.</returns>
    public bool CheckGameOver(Player opponent)
    {
        // Comprueba si todas las naves del oponente han sido hundidas
        for (int i = 0; i < opponent.OwnBoard.board.GetLength(0); i++)
        {
            for (int j = 0; j < opponent.OwnBoard.board.GetLength(1); j++)
            {
                if (opponent.OwnBoard.board[i, j] == "🟩") // Asume que "🟩" representa un barco
                {
                    return false; // Todavía hay al menos un barco no hundido
                }
            }
        }

        // Si todas las naves del oponente han sido hundidas, el juego ha terminado
        return true;
    }

}

