// Clase que gestiona la agenda  
public class Agenda
{
    private Contacto[] contactos;
    private int contador = 0;

    public Agenda()
    {
        contactos = new Contacto[10];
        Console.WriteLine("Agenda inicializada.\n");
    }

    // Agregar un nuevo contacto  
    public void Agregar(Contacto contact)
    {
        contactos[contador++] = contact;
        Console.WriteLine($"Contacto agregado.\n");
    }

    // Mostrar todos los contactos  
    public void Mostrar()
    {
        Console.WriteLine("------- Lista de contactos -------");

        foreach (var c in contactos)
        {
            Console.WriteLine(c);
        }
    }
}
  