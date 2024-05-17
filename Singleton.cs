/// <summary>
/// La clase Singleton<T> es una implementación genérica del patrón de diseño Singleton.
/// Esta clase se adhiere al principio de responsabilidad única (SRP) de SOLID, ya que su única 
/// responsabilidad es garantizar que sólo exista una instancia de la clase T.
/// También sigue el principio GRASP de alta cohesión, ya que todas sus responsabilidades están 
/// estrechamente relacionadas con la gestión de una instancia de la clase T.
/// </summary>
public static class Singleton<T>
    where T : new()
    {
        // <summary>
        /// Instancia de la clase T.
        /// </summary>
        private static T instance;
        /// <summary>
        /// Propiedad para obtener la instancia de la clase T. Si la instancia es nula, crea una nueva 
        /// instancia.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }

                return instance;
            }
        }
    }
