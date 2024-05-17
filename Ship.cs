/// <summary>
/// La clase Ship representa un barco en el juego de batalla naval.
/// Esta clase se adhiere al principio de responsabilidad única (SRP) de SOLID, ya que su única responsabilidad es gestionar el estado y el comportamiento de un barco.
/// También sigue el principio de inversión de dependencias (DIP) de SOLID, ya que no depende de ninguna otra clase concreta.
/// Además, sigue el principio GRASP de bajo acoplamiento, ya que los cambios en otras clases tienen un impacto mínimo en esta clase.
/// También sigue el principio GRASP de alta cohesión, ya que todas sus responsabilidades están estrechamente relacionadas con la gestión de un barco.
/// </summary>
public class Ship
{
    /// <summary>
    /// Longitud del barco.
    /// </summary>
    public string Type { get; private set; }
    /// <summary>
    /// Vida del barco. Inicialmente es igual a la longitud del barco.
    /// </summary>
    public int Life { get; private set; }
    /// <summary>
    /// Constructor de la clase Ship. Inicializa la longitud y la vida del barco.
    /// </summary>
    /// <param name="length">Longitud del barco.</param>

    public List<string> Positions { get; set; }
    public Ship(string type ,int life)
    {
        Type = type;
        Life = life;
        Positions = new List<string>();
    }

    public void Hit()
    {
        Life--;
    }

    public void AddPosition(string position)
    {
        Positions.Add(position);
    }

    
    
}
