using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

public class Program
{
    // Catalog with >= 10 titles
    private static readonly List<string> Catalog = new List<string>
    {
        "National Geographic",
        "Scientific American",
        "Time",
        "The Economist",
        "Nature",
        "Wired",
        "Forbes",
        "Vogue",
        "New Yorker",
        "Popular Science",
        "MIT Technology Review",
        "Harvard Business Review"
    };

    public static void Main()
    {
        // Simple text UI (ASCII only)
        Title();
        while (true)
        {
            Menu();
            Console.Write("Choose an option: ");
            string opt = (Console.ReadLine() ?? "").Trim();

            if (opt == "1") SearchTitle();
            else if (opt == "2") ShowCatalog();
            else if (opt == "3") AddTitle();
            else if (opt == "4") { Console.WriteLine("Bye!"); return; }
            else Console.WriteLine("Invalid option.\n");
        }
    }

    private static void Title()
    {
        Console.WriteLine("===============================================");
        Console.WriteLine("   Magazine Catalog - Search (C# Console)");
        Console.WriteLine("===============================================\n");
    }

    private static void Menu()
    {
        Console.WriteLine("1) Search a title (iterative or recursive)");
        Console.WriteLine("2) Show catalog");
        Console.WriteLine("3) Add title");
        Console.WriteLine("4) Exit");
    }

    private static void ShowCatalog()
    {
        if (Catalog.Count == 0) { Console.WriteLine("\nEmpty catalog.\n"); return; }
        Console.WriteLine("\nCatalog (total: {0}):", Catalog.Count);
        for (int i = 0; i < Catalog.Count; i++)
            Console.WriteLine((i + 1).ToString() + ". " + Catalog[i]);
        Console.WriteLine();
    }

    private static void AddTitle()
    {
        Console.Write("\nEnter new magazine title: ");
        string s = (Console.ReadLine() ?? "").Trim();
        if (s.Length == 0) { Console.WriteLine("Empty title. Not added.\n"); return; }
        Catalog.Add(s);
        Console.WriteLine("\"" + s + "\" added.\n");
    }

    private static void SearchTitle()
    {
        Console.Write("\nTitle to search: ");
        string target = (Console.ReadLine() ?? "").Trim();
        if (target.Length == 0) { Console.WriteLine("Empty input.\n"); return; }

        Console.WriteLine("\nChoose algorithm:");
        Console.WriteLine("  I) Iterative (linear)");
        Console.WriteLine("  R) Recursive (binary on ordered copy)");
        Console.Write("Option (I/R): ");
        string kind = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

        bool found;
        if (kind == "I")
        {
            found = LinearSearchIterative(Catalog, target);
        }
        else if (kind == "R")
        {
            // Make ordered copy using normalized key
            List<string> ordered = new List<string>(Catalog);
            ordered.Sort(CompareNormalized);
            found = BinarySearchRecursive(ordered, target);
        }
        else
        {
            Console.WriteLine("Invalid option.\n");
            return;
        }

        // Messages required by the assignment:
        Console.WriteLine(found ? "Encontrado\n" : "No encontrado\n");
    }

    // ---- Search algorithms ----
    private static bool LinearSearchIterative(List<string> data, string target)
    {
        string kt = NormalizeKey(target);
        foreach (string t in data)
            if (NormalizeKey(t) == kt) return true;
        return false;
    }

    private static bool BinarySearchRecursive(List<string> ordered, string target)
    {
        string kt = NormalizeKey(target);
        return BSR(ordered, kt, 0, ordered.Count - 1);
    }

    private static bool BSR(List<string> arr, string kt, int low, int high)
    {
        if (low > high) return false;
        int mid = low + ((high - low) / 2);
        string kmid = NormalizeKey(arr[mid]);
        int cmp = string.CompareOrdinal(kmid, kt);
        if (cmp == 0) return true;
        if (cmp > 0) return BSR(arr, kt, low, mid - 1);
        return BSR(arr, kt, mid + 1, high);
    }

    // ---- Utilities ----
    private static int CompareNormalized(string a, string b)
    {
        return string.CompareOrdinal(NormalizeKey(a), NormalizeKey(b));
    }

    // Uppercase + remove accents/diacritics; trim spaces
    private static string NormalizeKey(string s)
    {
        if (s == null) return "";
        string formD = s.Normalize(NormalizationForm.FormD);
        StringBuilder sb = new StringBuilder(formD.Length);
        for (int i = 0; i < formD.Length; i++)
        {
            char ch = formD[i];
            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (uc != UnicodeCategory.NonSpacingMark) sb.Append(ch);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC).ToUpperInvariant().Trim();
    }
}
