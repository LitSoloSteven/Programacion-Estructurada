using System;
using System.Collections.Generic; // Necesario para usar Queue<>

namespace GestionServidorWeb
{

    /// Esta clase simula el servidor web que gestiona las solicitudes
    /// utilizando una cola (Queue) para asegurar el orden FIFO.
    public class ServidorWeb
    {
        // La estructura de datos principal: una Cola de strings.
        // Es privada para protegerla y solo se puede modificar
        // a través de los métodos de la clase (encapsulación).
        private Queue<string> colaSolicitudes;

        /// Constructor de la clase. Se llama al crear un nuevo ServidorWeb.
        /// Inicializa la cola de solicitudes.
        public ServidorWeb()
        {
            colaSolicitudes = new Queue<string>();
        }

        /// Añade una nueva solicitud a la cola (Enqueue).
        /// <param name="descripcion">La descripción de la solicitud (ej. "Consulta de notas").</param>
        public void AgregarSolicitud(string descripcion)
        {
            // Validación: No permitir solicitudes vacías o nulas.
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                Console.WriteLine("Error: La descripción de la solicitud no puede estar vacía.");
                return; // Salimos del método si no es válida.
            }

            // Enqueue: Agrega el elemento al final de la cola.
            colaSolicitudes.Enqueue(descripcion);
            Console.WriteLine($"Solicitud recibida y encolada: '{descripcion}'");
            Console.WriteLine($"Total de solicitudes pendientes: {colaSolicitudes.Count}");
        }

        /// Procesa (atiende y elimina) la primera solicitud en la cola (Dequeue).
        public void ProcesarSiguienteSolicitud()
        {
            // Validación: Comprobar si hay elementos antes de intentar desencolar.
            if (colaSolicitudes.Count == 0)
            {
                Console.WriteLine("No hay solicitudes pendientes para procesar.");
                return; // Salimos del método.
            }

            // Dequeue: Remueve y devuelve el elemento al inicio de la cola.
            string solicitudAtendida = colaSolicitudes.Dequeue();
            Console.WriteLine($"Procesando solicitud: '{solicitudAtendida}'");
            Console.WriteLine($"Quedan {colaSolicitudes.Count} solicitudes pendientes.");
        }

        /// Muestra todas las solicitudes actualmente en la cola, sin modificarlas.

        public void MostrarSolicitudesPendientes()
        {
            // Validación: Comprobar si la cola está vacía.
            if (colaSolicitudes.Count == 0)
            {
                Console.WriteLine("La cola de solicitudes está vacía.");
                return;
            }

            Console.WriteLine(" Solicitudes Pendientes (en orden de llegada)");
            int posicion = 1;
            
            // Iteramos sobre la cola. Esto NO elimina los elementos.
            // Muestra desde el primero en entrar hasta el último.
            foreach (string solicitud in colaSolicitudes)
            {
                Console.WriteLine($"  {posicion}. {solicitud}");
                posicion++;
            }
            Console.WriteLine("--------------------------------------------------");
        }

        /// Devuelve la cantidad de solicitudes pendientes.
        /// <returns>Un entero con el número de elementos en la cola.</returns>
        public int ObtenerCantidadSolicitudes()
        {
            return colaSolicitudes.Count;
        }
    }

    /// Clase principal que contiene el menú interactivo (la simulación).

    class Program
    {
        static void Main(string[] args)
        {
            // Creamos una instancia de nuestro servidor.
            ServidorWeb miServidorUAM = new ServidorWeb();
            bool continuarSimulacion = true;

            Console.WriteLine("Iniciando simulación del Servidor Web de la UAM...");

            // Bucle principal del menú
            while (continuarSimulacion)
            {
                // Limpiamos la consola para un menú más limpio
                Console.Clear();
                Console.WriteLine("======================================");
                Console.WriteLine("  Gestión de Solicitudes del Servidor");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Agregar nueva solicitud");
                Console.WriteLine("2. Procesar siguiente solicitud (Atender)");
                Console.WriteLine("3. Mostrar solicitudes pendientes");
                Console.WriteLine("4. Verificar estado (¿Hay pendientes?)");
                Console.WriteLine("5. Salir de la simulación");
                Console.WriteLine("======================================");
                Console.Write("Por favor, elija una opción (1-5): ");

                string entradaUsuario = Console.ReadLine()??"";

                // Validación de la entrada del menú
                if (!int.TryParse(entradaUsuario, out int opcion))
                {
                    Console.WriteLine("Error: Opción no válida. Debe ingresar un número.");
                    PausarConsola();
                    continue; // Vuelve al inicio del bucle while
                }

                // Switch para manejar la opción elegida
                switch (opcion)
                {
                    case 1: // Agregar solicitud
                        Console.Write("Ingrese la descripción de la nueva solicitud: ");
                        string desc = Console.ReadLine()??"";
                        // La validación de string vacío la hace el método
                        miServidorUAM.AgregarSolicitud(desc);
                        break;

                    case 2: // Procesar solicitud
                        miServidorUAM.ProcesarSiguienteSolicitud();
                        break;

                    case 3: // Mostrar pendientes
                        miServidorUAM.MostrarSolicitudesPendientes();
                        break;

                    case 4: // Verificar estado
                        int cantidad = miServidorUAM.ObtenerCantidadSolicitudes();
                        if (cantidad > 0)
                        {
                            Console.WriteLine($"Estado: Hay {cantidad} solicitud(es) pendientes.");
                        }
                        else
                        {
                            Console.WriteLine("Estado: El servidor está libre. No hay solicitudes pendientes.");
                        }
                        break;

                    case 5: // Salir
                        continuarSimulacion = false;
                        Console.WriteLine("Cerrando la simulación del servidor...");
                        break;

                    default: // Opción numérica fuera de rango
                        Console.WriteLine("Error: Opción no válida. Elija un número entre 1 y 5.");
                        break;
                }

                if (continuarSimulacion)
                {
                    PausarConsola();
                }
            }

            Console.WriteLine("Simulación terminada.");
        }

        /// Pequeña función de utilidad para pausar la consola
        /// y permitir al usuario leer los mensajes.
        static void PausarConsola()
        {
            Console.WriteLine("\nPresione Enter para continuar...");
            Console.ReadLine();
        }
    }
}