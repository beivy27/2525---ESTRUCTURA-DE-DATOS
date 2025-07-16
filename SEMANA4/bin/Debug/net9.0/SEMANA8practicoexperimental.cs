using System;
using System.Collections.Generic;

// Clase Persona que representa a cada asistente
public class Persona
{
    public string Nombre { get; set; }
    public int NumeroLlegada { get; set; }

    // Constructor
    public Persona(string nombre, int numeroLlegada)
    {
        Nombre = nombre;
        NumeroLlegada = numeroLlegada;
    }

    // Método para mostrar los datos de la persona
    public void MostrarInfo()
    {
        Console.WriteLine($"Persona: {Nombre}, Número de llegada: {NumeroLlegada}");
    }
}

public class Program
{
    static void Main(string[] args)
    {
        int capacidadAuditorio = 100; // Número total de asientos
        string[] asientos = new string[capacidadAuditorio]; // Array que representa los asientos
        Queue<Persona> colaIngreso = new Queue<Persona>(); // Cola para gestion de ingreso

        int contadorPersonas = 0; // Para numerar las llegadas
        int asientosOcupados = 0; // Contador de asientos ocupados

        // Simular la llegada de personas (por ejemplo, 10 personas)
        for (int i = 1; i <= 10; i++)
        {
            string nombre = $"Persona_{i}";
            Persona nuevaPersona = new Persona(nombre, i);
            colaIngreso.Enqueue(nuevaPersona); // Añadimos a la cola
            Console.WriteLine($"{nombre} llegó y fue colocada en la cola.");
        }

        Console.WriteLine("\nIniciando el proceso de asignación de asientos...\n");

        // Ahora atendemos a las personas en la cola mientras haya espacio en el auditorio
        while (colaIngreso.Count > 0 && asientosOcupados < capacidadAuditorio)
        {
            Persona atendiendo = colaIngreso.Dequeue(); // Sacamos a la persona de la cola (FIFO)
            // Asignamos un asiento
            asientos[asientosOcupados] = atendiendo.Nombre; // Asignamos el nombre a la posición libre
            Console.WriteLine($"Se asignó asiento {asientosOcupados + 1} a {atendiendo.Nombre}");
            asientosOcupados++; // Aumentamos en 1 el contador de asientos ocupados
        }

        // Mostrar distribución final de asientos
        Console.WriteLine("\nEstado final de los asientos:");
        for (int i = 0; i < asientos.Length; i++)
        {
            if (asientos[i] != null)
                Console.WriteLine($"Asiento {i + 1}: {asientos[i]}");
            else
                Console.WriteLine($"Asiento {i + 1}: Vacío");
        }
    }
}