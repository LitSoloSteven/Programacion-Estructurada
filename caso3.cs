using System;
using System.Collections.Generic; // Necesario para usar List<>
using System.Linq; // Necesario para las búsquedas (Where, FirstOrDefault)

namespace GestionCatalogoSoftware
{
    /// <summary>
    /// Representa una sola pieza de software en nuestro catálogo.
    /// Esta es nuestra "entidad" o "modelo".
    /// </summary>
    public class Software
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Version { get; set; }

        /// <summary>
        /// Constructor para facilitar la creación de nuevos objetos Software.
        /// </summary>
        public Software(int id, string nombre, string version)
        {
            Id = id;
            Nombre = nombre;
            Version = version;
        }

        /// <summary>
        /// Sobrescribimos el método ToString() para que, cuando
        /// imprimamos un objeto 'Software', se muestre de forma legible.
        /// </summary>
        public override string ToString()
        {
            // El 'PadRight(30)' ayuda a alinear el texto para que se vea ordenado.
            return $"[ID: {Id.ToString().PadRight(3)}] - {Nombre.PadRight(25)} (Versión: {Version})";
        }
    }

    /// <summary>
    /// Clase que administra toda la lógica del catálogo.
    /// Contiene la Lista y los métodos para manipularla.
    /// </summary>
    public class CatalogoSoftware
    {
        // La estructura de datos principal: una Lista de objetos Software.
        private List<Software> listaDeSoftware;

        // Un contador simple para asignar IDs únicos automáticamente.
        private int proximoIdDisponible;

        /// <summary>
        /// Constructor de la clase. Inicializa la lista y el contador de ID.
        /// </summary>
        public CatalogoSoftware()
        {
            listaDeSoftware = new List<Software>();
            proximoIdDisponible = 1; // Empezamos a contar desde 1
        }

        /// <summary>
        /// Agrega un nuevo programa al catálogo.
        /// </summary>
        /// <param name="nombre">Nombre del software (ej. "Visual Studio")</param>
        /// <param name="version">Versión del software (ej. "2022 Community")</param>
        public void AgregarSoftware(string nombre, string version)
        {
            // Validación: No permitir entradas vacías.
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(version))
            {
                Console.WriteLine("Error: El nombre y la versión no pueden estar vacíos.");
                return;
            }

            // Validación (Opcional): Verificar si ya existe exactamente el mismo software.
            // Usamos LINQ (.Any) para ver si "alguno" cumple la condición.
            bool yaExiste = listaDeSoftware.Any(sw => 
                sw.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase) && 
                sw.Version.Equals(version, StringComparison.OrdinalIgnoreCase));

            if (yaExiste)
            {
                Console.WriteLine($"Error: El software '{nombre}' versión '{version}' ya existe en el catálogo.");
                return;
            }

            // Si pasa las validaciones, lo creamos y agregamos.
            Software nuevoSoftware = new Software(proximoIdDisponible, nombre, version);
            listaDeSoftware.Add(nuevoSoftware);

            // Incrementamos el ID para el próximo que se agregue.
            proximoIdDisponible++;

            Console.WriteLine($"Éxito: Se ha agregado '{nuevoSoftware.Nombre}' al catálogo.");
        }

        /// <summary>
        /// Muestra todos los programas actualmente en el catálogo.
        /// </summary>
        public void MostrarCatalogoCompleto()
        {
            // Validación: Comprobar si la lista está vacía.
            if (listaDeSoftware.Count == 0)
            {
                Console.WriteLine("El catálogo está actualmente vacío. No hay software para mostrar.");
                return;
            }

            Console.WriteLine("--- Catálogo de Software de la Facultad ---");
            // Iteramos sobre la lista y usamos el ToString() que definimos.
            foreach (Software sw in listaDeSoftware)
            {
                Console.WriteLine(sw.ToString());
            }
            Console.WriteLine("-------------------------------------------");
        }

        /// <summary>
        /// Busca programas por nombre. Muestra todos los que coincidan.
        /// </summary>
        /// <param name="terminoBusqueda">El nombre (o parte del nombre) a buscar.</param>
        public void BuscarSoftwarePorNombre(string terminoBusqueda)
        {
            // Validación: No buscar si el término está vacío.
            if (string.IsNullOrWhiteSpace(terminoBusqueda))
            {
                Console.WriteLine("Error: El término de búsqueda no puede estar vacío.");
                return;
            }

            // Usamos LINQ (.Where) para encontrar TODOS los elementos que cumplan la condición.
            // StringComparison.OrdinalIgnoreCase hace que la búsqueda no distinga mayúsculas/minúsculas.
            List<Software> resultados = listaDeSoftware.Where(sw => 
                sw.Nombre.Contains(terminoBusqueda, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            // Validación: Comprobar si la búsqueda no arrojó resultados.
            if (resultados.Count == 0)
            {
                Console.WriteLine($"No se encontraron programas que contengan el término '{terminoBusqueda}'.");
                return;
            }

            Console.WriteLine($"--- Resultados de la búsqueda para '{terminoBusqueda}' ---");
            foreach (Software sw in resultados)
            {
                Console.WriteLine(sw.ToString());
            }
            Console.WriteLine("--------------------------------------------------");
        }

        /// <summary>
        /// Elimina un software del catálogo usando su ID único.
        /// </summary>
        /// <param name="id">El ID del software a eliminar.</param>
        public void EliminarSoftware(int id)
        {
            // Usamos LINQ (.FirstOrDefault) para encontrar el primer (y único)
            // software que tenga ese ID.
            Software softwareAEliminar = listaDeSoftware.FirstOrDefault(sw => sw.Id == id);

            // Validación: Comprobar si el ID existe.
            // Si FirstOrDefault no encuentra nada, devuelve 'null'.
            if (softwareAEliminar == null)
            {
                Console.WriteLine($"Error: No se encontró ningún software con el ID {id}.");
                return;
            }

            // Si lo encontramos, lo eliminamos de la lista.
            listaDeSoftware.Remove(softwareAEliminar);
            Console.WriteLine($"Éxito: Se ha eliminado '{softwareAEliminar.Nombre}' (ID: {id}) del catálogo.");
        }
    }


    /// <summary>
    /// Clase principal con el menú interactivo para el usuario.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Creamos una instancia de nuestro administrador de catálogo.
            CatalogoSoftware miCatalogo = new CatalogoSoftware();
            bool continuarPrograma = true;

            // Datos de ejemplo para que el catálogo no esté vacío
            miCatalogo.AgregarSoftware("Visual Studio", "2022 Enterprise");
            miCatalogo.AgregarSoftware("AutoCAD", "2024");
            miCatalogo.AgregarSoftware("SQL Server", "2019 Developer");
            PausarConsola();


            while (continuarPrograma)
            {
                Console.Clear();
                Console.WriteLine("===============================================");
                Console.WriteLine("  Catálogo de Software - Fac. Arq. e Ing.");
                Console.WriteLine("===============================================");
                Console.WriteLine("1. Agregar nuevo software");
                Console.WriteLine("2. Eliminar software (por ID)");
                Console.WriteLine("3. Buscar software por nombre");
                Console.WriteLine("4. Mostrar catálogo completo");
                Console.WriteLine("5. Salir");
                Console.WriteLine("===============================================");
                Console.Write("Seleccione una opción: ");

                string entrada = Console.ReadLine();

                // Validación de la opción del menú
                if (!int.TryParse(entrada, out int opcion))
                {
                    Console.WriteLine("Error: Opción no válida. Ingrese un número del 1 al 5.");
                    PausarConsola();
                    continue; // Vuelve al inicio del while
                }

                switch (opcion)
                {
                    case 1: // Agregar
                        Console.Write("Ingrese el nombre del nuevo software: ");
                        string nombre = Console.ReadLine()??"";
                        Console.Write("Ingrese la versión de (ej. 2024): ");
                        string version = Console.ReadLine()??"";
                        miCatalogo.AgregarSoftware(nombre, version);
                        break;

                    case 2: // Eliminar
                        Console.Write("Ingrese el ID del software que desea eliminar: ");
                        string idInput = Console.ReadLine()??"";
                        
                        // Validación específica para el ID
                        if (!int.TryParse(idInput, out int idParaEliminar))
                        {
                            Console.WriteLine("Error: El ID debe ser un número.");
                        }
                        else
                        {
                            miCatalogo.EliminarSoftware(idParaEliminar);
                        }
                        break;

                    case 3: // Buscar
                        Console.Write("Ingrese el nombre (o parte del nombre) a buscar: ");
                        string termino = Console.ReadLine()??"";
                        miCatalogo.BuscarSoftwarePorNombre(termino);
                        break;

                    case 4: // Mostrar todo
                        miCatalogo.MostrarCatalogoCompleto();
                        break;

                    case 5: // Salir
                        continuarPrograma = false;
                        Console.WriteLine("Cerrando el gestor de catálogo...");
                        break;

                    default: // Opción fuera de rango
                        Console.WriteLine("Error: Opción no válida. Elija un número entre 1 y 5.");
                        break;
                }

                if (continuarPrograma)
                {
                    PausarConsola();
                }
            }

            Console.WriteLine("Programa finalizado.");
        }

        /// <summary>
        /// Función de utilidad para pausar la consola.
        /// </summary>
        static void PausarConsola()
        {
            Console.WriteLine("\nPresione Enter para continuar...");
            Console.ReadLine();
        }
    }
}