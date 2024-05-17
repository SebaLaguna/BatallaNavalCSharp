using System.Collections.Generic;
using System.Text;

/// <summary>
/// La clase <c>Match</c> representa un partido de batalla naval entre dos jugadores.
/// Esta clase se adhiere al principio de responsabilidad única (SRP) de SOLID, ya que su única 
/// responsabilidad es gestionar el flujo de un partido entre dos jugadores.
/// Además, sigue el principio GRASP de bajo acoplamiento, ya que los cambios en la clase Player tienen 
/// un impacto mínimo en esta clase.
/// También sigue el principio GRASP de alta cohesión, ya que todas sus responsabilidades están 
/// estrechamente relacionadas con la gestión de un partido.
/// </summary>
public class Match
{
    /// <summary>
    /// Primer jugador en el partido.
    /// </summary>
    public Player player1;

    /// <summary>
    /// Segundo jugador en el partido.
    /// </summary>
    public Player player2;

    /// <summary>
    /// Lista de jugadores actuales en el partido.
    /// </summary>
    public List<Player> currentMatches = new List<Player>();

    /// <summary>
    /// Constructor de la clase <c>Match</c>.
    /// </summary>
    /// <param name="player1">Primer jugador en el partido.</param>
    /// <param name="player2">Segundo jugador en el partido.</param>
    public Match(Player player1, Player player2)
    {
        this.player1 = player1;
        this.player2 = player2;
        currentMatches = new List<Player>();
    }

    /// <summary>
    /// Método para imprimir la lista de jugadores actuales en el partido.
    /// </summary>
    /// <returns>Una cadena que representa la lista de jugadores actuales en el partido.</returns>
    public string PrintCurrentMatches()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Current match: \n");
        foreach (Player player in currentMatches)
        {
            sb.Append($"{player.Name} - {player.Id}, ");
        }
        return sb.ToString().TrimEnd(',', ' ');
    }
}
