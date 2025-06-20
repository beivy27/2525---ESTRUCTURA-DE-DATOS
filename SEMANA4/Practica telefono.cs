using System; using System.Collections.Generic;
////////////////////////////////////////// // Clase Contacto: Representa un contacto ////////////////////////////////////////// 
public class Contacto { // Propiedades del contacto public string Nombre { get; set; } public string NumeroTelefono { get; set; } public string Direccion { get; set; } public string Email { get; set; }
// Constructor
public Contacto(string nombre, string numeroTelefono, string direccion, string email)
{
    Console.WriteLine($"Creando contacto: {nombre}");
    this.Nombre = nombre;
    this.NumeroTelefono = numeroTelefono;
    this.Direccion = direccion;
    this.Email = email;
}

// Método para mostrar los datos del contacto
public void MostrarContacto()
{
    Console.WriteLine("--------------");
    Console.WriteLine("Información del contacto:");
    Console.WriteLine($"Nombre: {Nombre}");
    Console.WriteLine($"Teléfono: {NumeroTelefono}");
    Console.WriteLine($"Dirección: {Direccion}");
    Console.WriteLine($"Email: {Email}");
    Console.WriteLine("--------------\n");
}
}
////////////////////////////////////////// // Clase Agenda: Gestiona los contactos ////////////////////////////////////////// 
public class Agenda { private List contactos;
public Agenda()
{
    contactos = new List<Contacto>();
    Console.WriteLine("Agenda creada exitosamente.\n");
}

// Añadir contacto
public void AgregarContacto(Contacto nuevoContacto)
{
    contactos.Add(nuevoContacto);
    Console.WriteLine($"Contacto '{nuevoContacto.Nombre}' agregado.\n");
}

// Mostrar todos los contactos
public void VisualizarContactos()
{
    if (contactos.Count == 0)
    {
        Console.WriteLine("La agenda está vacía.\n");
        return;
    }
    Console.WriteLine("Listado de Contactos:");
    foreach (var contacto in contactos)
    {
        contacto.MostrarContacto();
    }
}

// Buscar contacto por nombre
public Contacto BuscarPorNombre(string nombre)
{
    foreach (var contacto in contactos)
    {
        if (contacto.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"Contacto '{nombre}' encontrado.\n");
            return contacto;
        }
    }
    Console.WriteLine($"No se encontró un contacto con el nombre '{nombre}'.\n");
    return null;
}

// Eliminar contacto por nombre
public void EliminarContacto(string nombre)
{
    Contacto contactoAEliminar = BuscarPorNombre(nombre);
    if (contactoAEliminar != null)
    {
        contactos.Remove(contactoAEliminar);
        Console.WriteLine($"Contacto '{nombre}' eliminado.\n");
    }
    else
    {
        Console.WriteLine($"El contacto '{nombre}' no existe.\n");
    }
}

// Modificar contacto
public void ModificarContacto(string nombre)
{
    Contacto contactoAModificar = BuscarPorNombre(nombre);
    if (contactoAModificar != null)
    {
        Console.WriteLine("Ingrese los nuevos datos:");

        Console.WriteLine("Nuevo nombre:");
        contactoAModificar.Nombre = Console.ReadLine();

        Console.WriteLine("Nuevo teléfono:");
        contactoAModificar.NumeroTelefono = Console.ReadLine();

        Console.WriteLine("Nueva dirección:");
        contactoAModificar.Direccion = Console.ReadLine();

        Console.WriteLine("Nuevo email:");
        contactoAModificar.Email = Console.ReadLine();

        Console.WriteLine($"El contacto '{nombre}' ha sido actualizado.\n");
    }
    else
    {
        Console.WriteLine($"El contacto '{nombre}' no existe.\n");
    }
}
}
////////////////////////////////////////// // Programa principal ////////////////////////////////////////// class Program { static void Main(string[] args) { // Crear instancia de la agenda Agenda agenda = new Agenda();
    int opcion;

    do
    {
        // Mostrar menú de opciones
        Console.WriteLine("----- Gestión de Agenda Telefónica -----");
        Console.WriteLine("1. Agregar contacto");
        Console.WriteLine("2. Mostrar todos los contactos");
        Console.WriteLine("3. Buscar contacto");
        Console.WriteLine("4. Eliminar contacto");
        Console.WriteLine("5. Modificar contacto");
        Console.WriteLine("6. Salir");
        Console.Write("Seleccione una opción (1-6): ");

        // Leer opción
        string entrada = Console.ReadLine();
        bool operacionValida = int.TryParse(entrada, out opcion);

        if (!operacionValida)
        {
            Console.WriteLine("Por favor, ingrese un número válido.\n");
            continue;
        }

        switch (opcion)
        {
            case 1:
                // Añadir contacto
                Console.WriteLine("Ingrese el nombre:");
                string nombre = Console.ReadLine();

                Console.WriteLine("Ingrese el teléfono:");
                string telefono = Console.ReadLine();

                Console.WriteLine("Ingrese la dirección:");
                string direccion = Console.ReadLine();

                Console.WriteLine("Ingrese el email:");
                string email = Console.ReadLine();

                // Crear y agregar contacto
                Contacto nuevoContacto = new Contacto(nombre, telefono, direccion, email);
                agenda.AgregarContacto(nuevoContacto);
                break;

            case 2:
                // Mostrar todos los contactos
                agenda.VisualizarContactos();
                break;

            case 3:
                // Buscar contacto
                Console.WriteLine("Ingrese el nombre para buscar:");
                string nombreBuscar = Console.ReadLine();
                var contactoEncontrado = agenda.BuscarPorNombre(nombreBuscar);
                if (contactoEncontrado != null)
                    contactoEncontrado.MostrarContacto();
                break;

            case 4:
                // Eliminar contacto
                Console.WriteLine("Ingrese el nombre del contacto a eliminar:");
                string nombreEliminar = Console.ReadLine();
                agenda.EliminarContacto(nombreEliminar);
                break;

            case 5:
                // Modificar contacto
                Console.WriteLine("Ingrese el nombre del contacto a modificar:");
                string nombreModificar = Console.ReadLine();
                agenda.ModificarContacto(nombreModificar);
                break;

            case 6:
                Console.WriteLine("Gracias por usar la agenda. ¡Hasta pronto!");
                break;

            default:
                Console.WriteLine("Opción no válida, intente nuevamente.\n");
                break;
        }
        Console.WriteLine(); // Línea en blanco para separar
    } while (opcion != 6);


