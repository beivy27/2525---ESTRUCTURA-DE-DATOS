// Clase que representa un contacto con más datos  
public class Contacto  
{  
    public string Nombre { get; set; }  
    public string Telefono { get; set; }   

    public Contacto(string nombre, string telefono)  
    {  
        Nombre = nombre;  
        Telefono = telefono;  
    }  

    // Mostramos toda la información del contacto  
    public override string ToString()  
    {  
        return $"Nombre: {Nombre}\nTeléfono: {Telefono}n";  
    }  
}  
