using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    // Diccionario base: español -> inglés (min. 10; se incluyen todas las sugeridas)
    static Dictionary<string, string> esEn = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["tiempo"] = "time",
        ["persona"] = "person",
        ["año"] = "year",
        ["camino"] = "way",
        ["forma"] = "way",
        ["día"] = "day",
        ["cosa"] = "thing",
        ["hombre"] = "man",
        ["mundo"] = "world",
        ["vida"] = "life",
        ["mano"] = "hand",
        ["parte"] = "part",
        ["niño"] = "child",
        ["niña"] = "child",
        ["ojo"] = "eye",
        ["mujer"] = "woman",
        ["lugar"] = "place",
        ["trabajo"] = "work",
        ["semana"] = "week",
        ["caso"] = "case",
        ["punto"] = "point",
        ["tema"] = "point",
        ["gobierno"] = "government",
        ["empresa"] = "company",
        ["compañía"] = "company"
    };

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        while (true)
        {
            ImprimirMenu();

            Console.Write("Seleccione una opción: ");
            string opcion = Console.ReadLine()?.Trim() ?? "";

            switch (opcion)
            {
                case "1":
                    TraducirFrase();
                    break;
                case "2":
                    AgregarPalabra();
                    break;
                case "0":
                    Console.WriteLine("¡Hasta luego!");
                    return;
                default:
                    Console.WriteLine("Opción no válida. Intente de nuevo.\n");
                    break;
            }
        }
    }

    static void ImprimirMenu()
    {
        Console.WriteLine("============== MENÚ ==============");
        Console.WriteLine("1. Traducir una frase (ES -> EN)");
        Console.WriteLine("2. Agregar palabras al diccionario");
        Console.WriteLine("0. Salir");
        Console.WriteLine("==================================");
    }

    static void TraducirFrase()
    {
        Console.Write("Ingrese una frase en español: ");
        string frase = Console.ReadLine() ?? "";

        // Tokeniza preservando signos de puntuación
        var tokens = Tokenizar(frase);

        for (int i = 0; i < tokens.Count; i++)
        {
            if (!tokens[i].EsPalabra) continue;

            string palabraOriginal = tokens[i].Texto;
            string palabraLimpia = QuitarDiacriticos(palabraOriginal.ToLower());

            // Intentamos dos formas de búsqueda: con y sin diacríticos
            string traduccion = null;

            if (esEn.TryGetValue(palabraOriginal.ToLower(), out var t1))
                traduccion = t1;
            else if (esEn.TryGetValue(palabraLimpia, out var t2))
                traduccion = t2;
            else
                traduccion = null;

            if (traduccion != null)
            {
                traduccion = AjustarMayusculas(palabraOriginal, traduccion);
                tokens[i] = new Token(true, traduccion);
            }
        }

        string resultado = string.Concat(tokens.Select(t => t.Texto));
        Console.WriteLine("Traducción: " + resultado + "\n");
    }

    static void AgregarPalabra()
    {
        Console.Write("Palabra en español: ");
        string es = (Console.ReadLine() ?? "").Trim();

        if (string.IsNullOrWhiteSpace(es))
        {
            Console.WriteLine("La palabra en español no puede estar vacía.\n");
            return;
        }

        Console.Write("Traducción al inglés: ");
        string en = (Console.ReadLine() ?? "").Trim();

        if (string.IsNullOrWhiteSpace(en))
        {
            Console.WriteLine("La traducción al inglés no puede estar vacía.\n");
            return;
        }

        // Guardamos dos claves: con y sin diacríticos (para facilitar los matches)
        string clave1 = es.ToLower();
        string clave2 = QuitarDiacriticos(clave1);

        esEn[clave1] = en;
        esEn[clave2] = en;

        Console.WriteLine($"Agregado/actualizado: \"{es}\" -> \"{en}\".\n");
    }

    // -------- utilidades --------
    static string QuitarDiacriticos(string texto)
    {
        var norm = texto.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in norm)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    static string AjustarMayusculas(string original, string traducida)
    {
        // Si original está TODO en mayúsculas
        if (original.ToUpper() == original) return traducida.ToUpper();

        // Si original tiene mayúscula inicial
        if (char.IsLetter(original.FirstOrDefault()) && char.IsUpper(original[0]))
            return char.ToUpper(traducida[0]) + traducida.Substring(1);

        return traducida;
    }

    // Token que distingue palabras de signos/espacios para recomponer la frase
    struct Token
    {
        public bool EsPalabra;
        public string Texto;
        public Token(bool esPalabra, string texto)
        {
            EsPalabra = esPalabra;
            Texto = texto;
        }
    }

    static List<Token> Tokenizar(string s)
    {
        // Separa palabras (letras) y no-palabras (espacios, signos)
        var lista = new List<Token>();
        var regex = new Regex(@"\p{L}+|[^\p{L}]+", RegexOptions.Multiline);

        foreach (Match m in regex.Matches(s))
        {
            string t = m.Value;
            bool esPalabra = Regex.IsMatch(t, @"^\p{L}+$");
            lista.Add(new Token(esPalabra, t));
        }
        return lista;
    }
}
