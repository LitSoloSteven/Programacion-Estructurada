using System;
using System.Collections.Generic; // Para Queue<T>
using System.Linq; // Necesario para .Any() y .IsLetter

public class SimuladorFilaBanco
{
    public static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(" Simulación de Fila de Banco");
        Console.ResetColor();

        //  Declarar una cola de tipo string
        Queue<string> filaClientes = new Queue<string>();

        //  Permitir al usuario registrar (Enqueue) varios clientes
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n Registro de Clientes");
        Console.ResetColor();
        Console.WriteLine("Escriba el nombre del cliente y presione Enter.");
        Console.WriteLine("(Deje el nombre vacío y presione Enter para pasar al siguiente paso)");

        string nombreCliente;
        while (true)
        {
            Console.Write("Nombre del cliente: "); 
            nombreCliente = Console.ReadLine()??"";

            // Condición de salida
            if (string.IsNullOrWhiteSpace(nombreCliente))
            {
                break; 
            }

            // Llamamos al método de validación
            if (validacion(nombreCliente, filaClientes))
            {
                // Si es válido, lo agregamos 
                filaClientes.Enqueue(nombreCliente);
                Console.ForegroundColor = ConsoleColor.Green; // Éxito
                Console.WriteLine($"-> Cliente '{nombreCliente}' agregado a la fila.");
                Console.ResetColor();
            }
            // Si no es válido, el método 'validacion' ya imprimió el error en rojo.
        }

        Console.WriteLine("\n¡Registro de clientes finalizado!");
        PresionarEnterParaContinuar(); 

        
        //  Mostrar el estado actual de la cola
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(" Estado Actual de la Fila ");
        Console.ResetColor();
        MostrarEstadoFila(filaClientes); 
        PresionarEnterParaContinuar(); 


        //  Atender (Dequeue) al primer cliente y mostrar su nombre
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(" Atención de Cliente ");
        Console.ResetColor();
        
        if (filaClientes.Count > 0)
        {
            string clienteAtendido = filaClientes.Dequeue();
            Console.ForegroundColor = ConsoleColor.Green; // Éxito en atender
            Console.WriteLine($"Cliente atendido y saliendo de la fila: {clienteAtendido}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow; // Advertencia
            Console.WriteLine("No hay clientes en la fila para atender.");
            Console.ResetColor();
        }
        PresionarEnterParaContinuar(); 


        //  Mostrar la cola después de atender al cliente
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(" Estado de la Fila ");
        Console.ResetColor();
        Console.WriteLine("(Después de atender al primer cliente)");
        MostrarEstadoFila(filaClientes);
        PresionarEnterParaContinuar();

        
        //  Indicar si la cola está vacía
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(" Verificación de Fila Vacía ");
        Console.ResetColor();
        VerificarSiEstaVacia(filaClientes); 
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n Simulación Finalizada ");
        Console.ResetColor();
    }


    /// Método auxiliar que centraliza todas las validaciones
    /// para un nuevo nombre de cliente.
 
    public static bool validacion(string nombre, Queue<string> filaActual)
    {
        // Validación 1: Duplicados
        if (filaActual.Contains(nombre))
        {
            Console.ForegroundColor = ConsoleColor.Red; // Error
            Console.WriteLine($"Error: El cliente '{nombre}' ya está en la fila.");
            Console.ResetColor();
            return false;
        }

        // Validación 2: Que no sea solo un número
        if (double.TryParse(nombre, out _))
        {
            Console.ForegroundColor = ConsoleColor.Red; // Error
            Console.WriteLine("Error: El nombre no puede ser solo un número.");
            Console.ResetColor();
            return false;
        }

        // Validación 3: Longitud mínima
        if (nombre.Length < 2)
        {
            Console.ForegroundColor = ConsoleColor.Red; // Error
            Console.WriteLine("Error: El nombre debe tener al menos 2 caracteres.");
            Console.ResetColor();
            return false;
        }
        
        // Validación 4: Que contenga al menos una letra (evita "!!!")
        if (!nombre.Any(c => char.IsLetter(c)))
        {
            Console.ForegroundColor = ConsoleColor.Red; // Error
            Console.WriteLine("Error: El nombre debe contener al menos una letra.");
            Console.ResetColor();
            return false;
        }
        
        return true;
    }



    /// Método auxiliar para mostrar el estado actual de la cola.

    public static void MostrarEstadoFila(Queue<string> fila)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\nTotal de clientes en espera: {fila.Count}");
        Console.ResetColor();
        
        if (fila.Count > 0)
        {
            Console.WriteLine("Orden de clientes (Primero -> Último):");
            int i = 1;
            foreach (string cliente in fila)
            {
                Console.ForegroundColor = ConsoleColor.Gray; 
                Console.WriteLine($"  {i}. {cliente}");
                Console.ResetColor();
                i++;
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow; // Advertencia
            Console.WriteLine("[La fila está vacía]");
            Console.ResetColor();
        }
    }


    /// Método auxiliar para verificar si la cola está vacía.

    public static void VerificarSiEstaVacia(Queue<string> fila)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        if (fila.Count == 0)
        {
            Console.WriteLine("Estado: La cola SÍ está vacía.");
        }
        else
        {
            Console.WriteLine($"Estado: La cola NO está vacía (Quedan {fila.Count} clientes).");
        }
        Console.ResetColor();
    }


    /// Método auxiliar para pausar la ejecución hasta que el usuario presione Enter.

    public static void PresionarEnterParaContinuar()
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("\n(Presione Enter para continuar al siguiente paso...)");
        Console.ResetColor();
        Console.ReadLine();
    }
}