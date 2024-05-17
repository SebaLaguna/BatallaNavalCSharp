/// <summary>
/// La clase PlaceShips representa un barco que se va a colocar en el tablero del juego.
/// Esta clase es responsable de almacenar la información sobre el tipo de barco, su orientación y sus coordenadas en el tablero.
/// </summary>
public class PlaceShips
{
    /// <summary>
    /// Tipo de barco.
    /// </summary>
    public string Tipo { get; set; }

    /// <summary>
    /// Coordenada X del barco en el tablero.
    /// </summary>
    public int CordinateX { get; private set; }

    /// <summary>
    /// Coordenada Y del barco en el tablero.
    /// </summary>
    public int CordinateY { get; private set; }

    /// <summary>
    /// Orientación del barco en el tablero.
    /// </summary>
    public enum Orientation { Horizontal, Vertical }

    public Orientation orientation { get; set; }

    /// <summary>
    /// Constructor de la clase PlaceShips. Inicializa el tipo de barco, sus coordenadas y su orientación.
    /// </summary>
    /// <param name="tipo">Tipo de barco.</param>
    /// <param name="cordinates">Coordenadas del barco en el tablero.</param>
    /// <param name="orientation">Orientación del barco en el tablero.</param>
    public PlaceShips(string tipo, string cordinates, Orientation orientation)
    {
        Tipo = tipo;
        SetCordinates(cordinates);
        this.orientation = orientation;
    }

    /// <summary>
    /// Método para establecer las coordenadas del barco en el tablero.
    /// </summary>
    /// <param name="cordinates">Coordenadas del barco en el tablero.</param>
    public void SetCordinates(string cordinates)
    {
        CordinateX = int.Parse(cordinates.Substring(0, 1));
        CordinateY = int.Parse(cordinates.Substring(1, 1));
    }
}
