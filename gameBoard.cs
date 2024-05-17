using System.Text;

/// <summary>
/// La clase GameBoard representa el tablero de juego para el juego de Batalla Naval.
/// Esta clase es responsable de crear y gestionar el tablero de juego, incluida la ubicación de los barcos
/// y el seguimiento de los ataques.
/// La clase cumple con el Principio de Responsabilidad Única (SRP) de SOLID, ya que su única responsabilidad
/// es gestionar el tablero de juego. Esto asegura que la clase tenga una única razón para cambiar, reduciendo
/// la complejidad del código y mejorando su mantenibilidad.
/// También sigue el principio GRASP de baja dependencia, ya que no depende de ninguna otra clase.
/// Esto significa que los cambios en otras clases tienen un impacto mínimo en esta clase, mejorando
/// la robustez y flexibilidad del código.
/// También sigue el principio GRASP de alta cohesión, ya que todas sus responsabilidades están
/// estrechamente relacionadas con la gestión del tablero de juego. Esto mejora la comprensibilidad y
/// la mantenibilidad del código.
/// </summary>
public class GameBoard
{
    /// <summary>
    /// Tamaño del tablero de juego.
    /// </summary>
    public int Size; // Tamaño del tablero de juego.

    /// <summary>
    /// Propiedad que representa el tablero de juego. Es una matriz bidimensional de cadenas que representa
    /// el estado actual del tablero.
    /// </summary>
    public string[,] board { get; private set; }

    /// <summary>
    /// Constructor de la clase GameBoard. Inicializa el tablero y lo llena con agua.
    /// </summary>
    /// <param name="Size">Tamaño del tablero de juego.</param>
    /// <exception cref="ArgumentException">Se lanza cuando el tamaño del tablero es menor o igual a cero.</exception>
    public GameBoard(int Size)
    {
        if (Size <= 0)
        {
            // ArgumentException: El tamaño del tablero debe ser mayor que cero.
            throw new ArgumentException("El tamaño del tablero debe ser mayor que cero.");
        }
        // Precondición: El tamaño debe ser mayor que cero para que el tablero sea válido.
        this.Size = Size;
        board = new string[Size, Size];
        CreateBoard();
    }

    /// <summary>
    /// Método para crear el tablero. Llena el tablero con agua, representada por el carácter '🟦'.
    /// Este método sigue el patrón de diseño de Creación, ya que se encarga de la creación del tablero de juego.
    /// </summary>
    private void CreateBoard()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                board[i, j] = "🟦"; // Símbolo de agua
            }
        }
    }

    /// <summary>
    /// Método para imprimir el tablero. Muestra el estado actual del tablero en la consola.
    /// </summary>
    /// <returns>Una representación de cadena del tablero de juego actual.</returns>
    public string PrintBoard()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("   ");
        for (int i = 0; i < Size; i++)
        {
            sb.Append((i + 1).ToString().PadLeft(4) + " ");
        }
        sb.AppendLine();
        for (int i = 0; i < Size; i++)
        {
            sb.Append((i + 1).ToString().PadLeft(4) + " ");
            for (int j = 0; j < Size; j++)
            {
                sb.Append(" " + board[i, j].ToString().PadLeft(2));
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
