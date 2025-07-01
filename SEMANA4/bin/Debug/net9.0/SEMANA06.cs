using System;

public class Nodo
{
    public int Dato;
    public Nodo Siguiente;

    public Nodo(int dato)
    {
        Dato = dato;
        Siguiente = null;
    }
}

public class ListaSimple
{
    private Nodo head;

    public ListaSimple()
    {
        head = null;
    }

    // Insertar al final
    public void InsertarFinal(int dato)
    {
        Nodo nuevo = new Nodo(dato);
        if (head == null)
        {
            head = nuevo;
        }
        else
        {
            Nodo actual = head;
            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }
            actual.Siguiente = nuevo;
        }
    }

    // ✅ Método para invertir la lista
    public void Invertir()
    {
        Nodo anterior = null;
        Nodo actual = head;
        Nodo siguiente;

        while (actual != null)
        {
            siguiente = actual.Siguiente;
            actual.Siguiente = anterior;
            anterior = actual;
            actual = siguiente;
        }

        head = anterior;
    }

    // Mostrar la lista
    public void Mostrar()
    {
        Nodo actual = head;
        Console.Write("Lista: ");
        while (actual != null)
        {
            Console.Write(actual.Dato + " -> ");
            actual = actual.Siguiente;
        }
        Console.WriteLine("null");
    }
}

public class Program
{
    public static void Main()
    {
        ListaSimple lista = new ListaSimple();

        lista.InsertarFinal(10);
        lista.InsertarFinal(20);
        lista.InsertarFinal(30);
        lista.InsertarFinal(40);
        lista.InsertarFinal(50);

        Console.WriteLine("Lista original:");
        lista.Mostrar();

        Console.WriteLine("Lista invertida:");
        lista.Invertir();
        lista.Mostrar();
    }
}
using System;

public class Nodo
{
    public int Dato;
    public Nodo Siguiente;

    public Nodo(int dato)
    {
        Dato = dato;
        Siguiente = null;
    }
}

public class ListaSimple
{
    private Nodo head;

    public ListaSimple()
    {
        head = null;
    }

    public void InsertarFinal(int dato)
    {
        Nodo nuevo = new Nodo(dato);
        if (head == null)
        {
            head = nuevo;
        }
        else
        {
            Nodo actual = head;
            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }
            actual.Siguiente = nuevo;
        }
    }

    public void BuscarFrecuencia(int valor)
    {
        Nodo actual = head;
        int contador = 0;

        while (actual != null)
        {
            if (actual.Dato == valor)
            {
                contador++;
            }
            actual = actual.Siguiente;
        }

        if (contador > 0)
        {
            Console.WriteLine($"El valor {valor} se encuentra {contador} veces en la lista.");
        }
        else
        {
            Console.WriteLine($"El valor {valor} no fue encontrado en la lista.");
        }
    }

    public void Mostrar()
    {
        Nodo actual = head;
        Console.Write("Lista: ");
        while (actual != null)
        {
            Console.Write(actual.Dato + " -> ");
            actual = actual.Siguiente;
        }
        Console.WriteLine("null");
    }
}

public class Program
{
    public static void Main()
    {
        ListaSimple lista = new ListaSimple();

        // Insertar elementos de ejemplo
        lista.InsertarFinal(5);
        lista.InsertarFinal(10);
        lista.InsertarFinal(15);
        lista.InsertarFinal(10);
        lista.InsertarFinal(20);
        lista.InsertarFinal(10);

        Console.WriteLine("Lista actual:");
        lista.Mostrar();

        Console.WriteLine("\nBúsqueda del número 10:");
        lista.BuscarFrecuencia(10);

        Console.WriteLine("\nBúsqueda del número 99:");
        lista.BuscarFrecuencia(99);
    }
}
