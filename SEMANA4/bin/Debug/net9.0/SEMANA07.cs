using System;
using System.Collections.Generic;

class BalanceoParentesis
{
    /// <summary>
    /// Verifica si una expresión matemática tiene paréntesis, llaves y corchetes balanceados.
    /// </summary>
    /// <param name="expresion">Cadena con la fórmula a verificar</param>
    /// <returns>True si está balanceada, False en caso contrario</returns>
    static bool EstaBalanceada(string expresion)
    {
        Stack<char> pila = new Stack<char>();
        Dictionary<char, char> pares = new Dictionary<char, char>()
        {
            { ')', '(' },
            { ']', '[' },
            { '}', '{' }
        };

        foreach (char c in expresion)
        {
            if (pares.ContainsValue(c)) // Si es un par de apertura
            {
                pila.Push(c);
            }
            else if (pares.ContainsKey(c)) // Si es un par de cierre
            {
                if (pila.Count == 0 || pila.Pop() != pares[c])
                {
                    return false; // No hay apertura o no coincide
                }
            }
        }

        return pila.Count == 0;
    }

    static void Main()
    {
        Console.WriteLine("Ingrese una fórmula matemática:");
        string entrada = Console.ReadLine();

        bool resultado = EstaBalanceada(entrada);

        if (resultado)
            Console.WriteLine("✅ Fórmula balanceada.");
        else
            Console.WriteLine("❌ Fórmula desbalanceada.");
    }
}
using System;

class TorresHanoi
{
    /// <summary>
    /// Mueve los discos entre las torres siguiendo las reglas del problema.
    /// </summary>
    /// <param name="n">Número de discos</param>
    /// <param name="origen">Torre de origen</param>
    /// <param name="auxiliar">Torre auxiliar</param>
    /// <param name="destino">Torre destino</param>
    static void ResolverHanoi(int n, char origen, char auxiliar, char destino)
    {
        if (n == 1)
        {
            Console.WriteLine($"Mover disco 1 de {origen} a {destino}");
            return;
        }

        ResolverHanoi(n - 1, origen, destino, auxiliar);
        Console.WriteLine($"Mover disco {n} de {origen} a {destino}");
        ResolverHanoi(n - 1, auxiliar, origen, destino);
    }

    static void Main()
    {
        Console.WriteLine("Ingrese número de discos:");
        int discos = int.Parse(Console.ReadLine());

        Console.WriteLine("\nPasos para resolver Torres de Hanoi:\n");
        ResolverHanoi(discos, 'A', 'B', 'C');
    }
}
