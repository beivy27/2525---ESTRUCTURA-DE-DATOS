using System;
using System.Collections.Generic;

class Curso
{
    public List<string> Asignaturas { get; set; }

    public Curso()
    {
        Asignaturas = new List<string> { "Matemáticas", "Física", "Química", "Historia", "Lengua" };
    }

    public void MostrarAsignaturas()
    {
        Console.WriteLine("Asignaturas del curso:");
        foreach (string asignatura in Asignaturas)
        {
            Console.WriteLine(asignatura);
        }
    }
}

class Program
{
    static void Main()
    {
        Curso curso = new Curso();
        curso.MostrarAsignaturas();
    }
}
using System;
using System.Collections.Generic;

class Curso
{
    public List<string> Asignaturas { get; set; }

    public Curso()
    {
        Asignaturas = new List<string> { "Matemáticas", "Física", "Química", "Historia", "Lengua" };
    }

    public void EstudiarAsignaturas()
    {
        foreach (string asignatura in Asignaturas)
        {
            Console.WriteLine($"Yo estudio {asignatura}");
        }
    }
}

class Program
{
    static void Main()
    {
        Curso curso = new Curso();
        curso.EstudiarAsignaturas();
    }
}

using System;
using System.Collections.Generic;

class Curso
{
    public List<string> Asignaturas { get; set; }
    public Dictionary<string, double> Notas { get; set; }

    public Curso()
    {
        Asignaturas = new List<string> { "Matemáticas", "Física", "Química", "Historia", "Lengua" };
        Notas = new Dictionary<string, double>();
    }

    public void IngresarNotas()
    {
        foreach (string asignatura in Asignaturas)
        {
            Console.Write($"Ingrese la nota para {asignatura}: ");
            double nota = double.Parse(Console.ReadLine());
            Notas[asignatura] = nota;
        }
    }

    public void MostrarNotas()
    {
        foreach (var item in Notas)
        {
            Console.WriteLine($"En {item.Key} has sacado {item.Value}");
        }
    }
}

class Program
{
    static void Main()
    {
        Curso curso = new Curso();
        curso.IngresarNotas();
        Console.WriteLine("\nResumen de notas:");
        curso.MostrarNotas();
    }
}
using System;
using System.Collections.Generic;

class Loteria
{
    public List<int> Numeros { get; set; }

    public Loteria()
    {
        Numeros = new List<int>();
    }

    public void IngresarNumeros()
    {
        Console.WriteLine("Ingrese 6 números ganadores de la lotería:");
        for (int i = 0; i < 6; i++)
        {
            Console.Write($"Número {i + 1}: ");
            int num = int.Parse(Console.ReadLine());
            Numeros.Add(num);
        }
    }

    public void MostrarOrdenados()
    {
        Numeros.Sort();
        Console.WriteLine("Números ordenados: " + string.Join(", ", Numeros));
    }
}

class Program
{
    static void Main()
    {
        Loteria loteria = new Loteria();
        loteria.IngresarNumeros();
        loteria.MostrarOrdenados();
    }
}
using System;
using System.Collections.Generic;

class Reversa
{
    public List<int> Numeros { get; set; }

    public Reversa()
    {
        Numeros = new List<int>();
        for (int i = 1; i <= 10; i++)
            Numeros.Add(i);
    }

    public void MostrarInverso()
    {
        Numeros.Reverse();
        Console.WriteLine("Números en orden inverso: " + string.Join(", ", Numeros));
    }
}

class Program
{
    static void Main()
    {
        Reversa rev = new Reversa();
        rev.MostrarInverso();
    }
}
