using Telegram.Bot.Types;
/// <summary>
/// Esta categoría describe una función denominada Run. En cualquier contexto donde se requiera una 
/// instancia de esta categoría, según el principio de sustitución de Liskov, es posible introducir una 
/// subcategoría que también implementará la función Run. El programa continuará operando, pero la 
/// ejecución del comando dependerá de la implementación específica del método Run, en consonancia con 
/// el principio de polimorfismo.
/// </summary>
public abstract class Command
{
    /// <summary>
    /// Este método ejecuta el comando basado en un mensaje dado. Run usa el patrón Polymorphism porque 
    /// se define en esta clase como abstracto y los 
    /// sucesores lo implmentan de forma diferente según el comando.
    /// </summary>
    /// <param name="message"> El mensaje que se utilizará para ejecutar el comando. </param>
    /// <returns> Retorna una cadena que representa el resultado de la ejecución del comando. La 
    /// implementación específica de este método depende de la clase derivada. </returns>
    public abstract string Run(Message message);
    /// <summary>
    /// Este método determina si un comando puede ejecutarse en base a un mensaje dado. Al ser un 
    /// método virtual, permite la sobrecarga en las clases derivadas, lo que es una forma de 
    /// polimorfismo. Además, al proporcionar un método que puede ser sobrescrito por las clases 
    /// derivadas para cambiar su comportamiento, se está siguiendo el Principio Abierto/Cerrado 
    /// (Open/Closed Principle, OCP) de SOLID.
    /// </summary>
    /// <param name="message"> El mensaje que se evaluará para determinar si el comando puede 
    /// ejecutarse. </param>
    /// <returns> Retorna 'false' por defecto, lo que significa que el comando no puede ejecutarse. Las 
    /// clases derivadas pueden sobrescribir este método para proporcionar su propia lógica de 
    /// determinación.</returns>
    public virtual bool CanRun(Message message)
    {
        return false;
    }
}