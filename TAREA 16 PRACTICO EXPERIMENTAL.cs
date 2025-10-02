using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GrafosCentralidad
{
    class Graph
    {
        public int N { get; private set; }
        public bool Directed { get; private set; }
        private readonly List<int>[] adj;
        private readonly List<int>[] radj;

        public Graph(int n, bool directed)
        {
            if (n <= 0) throw new ArgumentException("N debe ser > 0");
            N = n;
            Directed = directed;
            adj = new List<int>[N];
            radj = new List<int>[N];
            for (int i = 0; i < N; i++)
            {
                adj[i] = new List<int>();
                radj[i] = new List<int>();
            }
        }

        public void AddEdge(int u, int v)
        {
            if (u < 0 || v < 0 || u >= N || v >= N)
                throw new ArgumentOutOfRangeException("Arista fuera de rango");
            adj[u].Add(v);
            radj[v].Add(u);
            if (!Directed)
            {
                adj[v].Add(u);
                radj[u].Add(v);
            }
        }

        public IEnumerable<int> Neighbors(int u) { return adj[u]; }
        public IEnumerable<int> InNeighbors(int v) { return radj[v]; }
        public IEnumerable<int> Nodes() { for (int i = 0; i < N; i++) yield return i; }

        public static Graph FromCsv(string path, bool directed, bool hasHeader, char sep)
        {
            if (!File.Exists(path)) throw new FileNotFoundException("No se encontró: " + path);
            var lines = File.ReadAllLines(path);
            int start = hasHeader ? 1 : 0;
            int maxIdx = -1;
            for (int i = start; i < lines.Length; i++)
            {
                var ln = lines[i].Trim();
                if (ln.Length == 0) continue;
                var parts = ln.Split(sep);
                if (parts.Length < 2) continue;
                int u, v;
                if (int.TryParse(parts[0].Trim(), out u) && int.TryParse(parts[1].Trim(), out v))
                {
                    if (u > maxIdx) maxIdx = u;
                    if (v > maxIdx) maxIdx = v;
                }
            }
            if (maxIdx < 0) throw new Exception("CSV sin aristas válidas");
            Graph g = new Graph(maxIdx + 1, directed);
            for (int i = start; i < lines.Length; i++)
            {
                var ln = lines[i].Trim();
                if (ln.Length == 0) continue;
                var parts = ln.Split(sep);
                if (parts.Length < 2) continue;
                int u, v;
                if (int.TryParse(parts[0].Trim(), out u) && int.TryParse(parts[1].Trim(), out v))
                {
                    g.AddEdge(u, v);
                }
            }
            return g;
        }
    }

    static class Centralities
    {
        public static void Degree(Graph g, out double[] degOut, out double[] degIn, out double[] degTot)
        {
            int n = g.N;
            degOut = new double[n];
            degIn = new double[n];
            degTot = new double[n];
            double norm = (n > 1) ? 1.0 / (n - 1) : 0.0;

            foreach (int u in g.Nodes())
            {
                int outc = 0;
                foreach (int v in g.Neighbors(u)) outc++;
                degOut[u] = outc * norm;
                foreach (int v in g.Neighbors(u)) degIn[v] += norm;
            }
            for (int i = 0; i < n; i++)
            {
                double rawOut = degOut[i] * (n - 1);
                double rawIn  = degIn[i]  * (n - 1);
                degTot[i] = (rawOut + rawIn) * norm;
            }
        }

        public static double[] Closeness(Graph g)
        {
            int n = g.N;
            double[] clos = new double[n];
            for (int s = 0; s < n; s++)
            {
                int[] dist = new int[n];
                for (int i = 0; i < n; i++) dist[i] = int.MaxValue;
                Queue<int> q = new Queue<int>();
                dist[s] = 0;
                q.Enqueue(s);
                while (q.Count > 0)
                {
                    int u = q.Dequeue();
                    foreach (int v in g.Neighbors(u))
                    {
                        if (dist[v] == int.MaxValue)
                        {
                            dist[v] = dist[u] + 1;
                            q.Enqueue(v);
                        }
                    }
                }
                long sum = 0;
                int reach = 0;
                for (int i = 0; i < n; i++)
                {
                    if (i == s) continue;
                    if (dist[i] != int.MaxValue) { sum += dist[i]; reach++; }
                }
                if (reach == 0 || sum == 0) clos[s] = 0.0;
                else
                {
                    double factor = (double)reach / (n - 1);
                    clos[s] = factor * ((double)reach / sum);
                }
            }
            return clos;
        }

        public static double[] Betweenness(Graph g)
        {
            int n = g.N;
            double[] CB = new double[n];
            for (int s = 0; s < n; s++)
            {
                Stack<int> S = new Stack<int>();
                List<int>[] P = new List<int>[n];
                for (int i = 0; i < n; i++) P[i] = new List<int>();
                double[] sigma = new double[n];
                int[] dist = new int[n];
                for (int i = 0; i < n; i++) dist[i] = -1;

                sigma[s] = 1.0; dist[s] = 0;

                Queue<int> Q = new Queue<int>();
                Q.Enqueue(s);
                while (Q.Count > 0)
                {
                    int v = Q.Dequeue();
                    S.Push(v);
                    foreach (int w in g.Neighbors(v))
                    {
                        if (dist[w] < 0) { dist[w] = dist[v] + 1; Q.Enqueue(w); }
                        if (dist[w] == dist[v] + 1) { sigma[w] += sigma[v]; P[w].Add(v); }
                    }
                }
                double[] delta = new double[n];
                while (S.Count > 0)
                {
                    int w = S.Pop();
                    foreach (int v in P[w])
                    {
                        if (sigma[w] > 0.0) delta[v] += (sigma[v] / sigma[w]) * (1.0 + delta[w]);
                    }
                    if (w != s) CB[w] += delta[w];
                }
            }
            if (n > 2)
            {
                double scale = g.Directed ? 1.0 / ((n - 1.0) * (n - 2.0))
                                          : 2.0 / ((n - 1.0) * (n - 2.0));
                for (int v = 0; v < n; v++) CB[v] *= scale;
            }
            else { for (int v = 0; v < n; v++) CB[v] = 0.0; }
            return CB;
        }

        public static double[] Eigenvector(Graph g, int maxIter, double tol)
        {
            int n = g.N;
            double[] x = new double[n];
            double inv = 1.0 / Math.Sqrt(n);
            for (int i = 0; i < n; i++) x[i] = inv;

            for (int it = 0; it < maxIter; it++)
            {
                double[] y = new double[n];
                for (int v = 0; v < n; v++)
                {
                    double sum = 0.0;
                    foreach (int u in g.InNeighbors(v)) sum += x[u];
                    sum += 0.0; // evita warnings
                    y[v] = sum;
                }
                double norm = 0.0;
                for (int i = 0; i < n; i++) norm += y[i] * y[i];
                norm = Math.Sqrt(norm);
                if (norm == 0.0) break;
                for (int i = 0; i < n; i++) y[i] /= norm;

                double diff = 0.0;
                for (int i = 0; i < n; i++)
                {
                    double d = Math.Abs(y[i] - x[i]);
                    if (d > diff) diff = d;
                }
                x = y;
                if (diff < tol) break;
            }
            return x;
        }
    }

    static class Util
    {
        public static List<(int node, double value)> TopK(double[] values, int k)
        {
            var list = new List<(int, double)>();
            for (int i = 0; i < values.Length; i++) list.Add((i, values[i]));
            list.Sort((a, b) => b.Item2.CompareTo(a.Item2));
            if (k < list.Count) list = list.GetRange(0, k);
            return list;
        }

        public static void WriteCsv(string path, int n, double[] degOut, double[] degIn, double[] degTot,
                                    double[] clos, double[] bet, double[] eig)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine("nodo,deg_out,deg_in,deg_total,closeness,betweenness,eigenvector");
                for (int i = 0; i < n; i++)
                {
                    sw.WriteLine(
                        i.ToString(CultureInfo.InvariantCulture) + "," +
                        degOut[i].ToString(CultureInfo.InvariantCulture) + "," +
                        degIn[i].ToString(CultureInfo.InvariantCulture) + "," +
                        degTot[i].ToString(CultureInfo.InvariantCulture) + "," +
                        clos[i].ToString(CultureInfo.InvariantCulture) + "," +
                        bet[i].ToString(CultureInfo.InvariantCulture) + "," +
                        eig[i].ToString(CultureInfo.InvariantCulture)
                    );
                }
            }
        }

        // Exporta un .dot (Graphviz) con tamaño de nodo proporcional a 'metric'
        public static void WriteDot(string path, Graph g, double[] metric, string metricName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
            double min = double.MaxValue, max = double.MinValue;
            foreach (var v in metric) { if (v < min) min = v; if (v > max) max = v; }
            double range = (max - min);
            if (range <= 1e-12) range = 1.0;

            using (var sw = new StreamWriter(path))
            {
                string kind = g.Directed ? "digraph" : "graph";
                string arrow = g.Directed ? "->" : "--";
                sw.WriteLine(kind + " G {");
                sw.WriteLine("  rankdir=LR; node [shape=circle, style=filled, fillcolor=\"#87CEFA\", fontcolor=black];");

                for (int i = 0; i < g.N; i++)
                {
                    double norm = (metric[i] - min) / range;     // 0..1
                    double size = 0.5 + 2.5 * norm;              // escala 0.5..3.0
                    sw.WriteLine($"  \"{i}\" [label=\"{i}\\n{metricName}={metric[i]:0.000}\", width={size:0.###}, fixedsize=true];");
                }

                for (int u = 0; u < g.N; u++)
                {
                    foreach (var v in g.Neighbors(u))
                    {
                        if (!g.Directed && u > v) continue; // evitar duplicados en no dirigido
                        sw.WriteLine($"  \"{u}\" {arrow} \"{v}\";");
                    }
                }

                sw.WriteLine("}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // ===== Configuración =====
                bool directed  = true;                  // false para grafo no dirigido
                bool usarCsv   = false;                 // true para leer CSV
                string rutaCsv = "data/aristas.csv";    // u,v por línea (0-index). Si hay encabezado, ajusta csvHeader=true
                bool csvHeader = false;
                int   kTop     = 3;                     // Top-K a mostrar por métrica
                string metricDot = "eigenvector";       // métrica para el .dot: "eigenvector" | "betweenness" | "closeness" | "deg_total"

                Graph g;
                if (usarCsv) g = Graph.FromCsv(rutaCsv, directed, csvHeader, ',');
                else
                {
                    g = new Graph(4, directed);
                    g.AddEdge(0, 1);
                    g.AddEdge(0, 2);
                    g.AddEdge(1, 2);
                    g.AddEdge(2, 0);
                    g.AddEdge(2, 3);
                    g.AddEdge(3, 3);
                }

                int n = g.N;

                // ===== Tiempos y cálculos =====
                var t0 = DateTime.UtcNow;
                double[] degOut, degIn, degTot;
                Centralities.Degree(g, out degOut, out degIn, out degTot);
                var t1 = DateTime.UtcNow;

                var clos = Centralities.Closeness(g);
                var t2 = DateTime.UtcNow;

                var bet  = Centralities.Betweenness(g);
                var t3 = DateTime.UtcNow;

                var eig  = Centralities.Eigenvector(g, 1000, 1e-9);
                var t4 = DateTime.UtcNow;

                // ===== Mostrar tabla =====
                Console.WriteLine("N = {0} | Directed = {1}", n, g.Directed);
                Console.WriteLine("Nodo\tDegOut\tDegIn\tDegTot\tCloseness\tBetweenness\tEigenvector");
                for (int i = 0; i < n; i++)
                {
                    Console.WriteLine(
                        "{0}\t{1:0.000}\t{2:0.000}\t{3:0.000}\t{4:0.000000}\t{5:0.000000}\t{6:0.000000}",
                        i, degOut[i], degIn[i], degTot[i], clos[i], bet[i], eig[i]);
                }

                // ===== Tiempos =====
                Console.WriteLine();
                Console.WriteLine("Tiempos (ms):");
                Console.WriteLine("Grado          : {0:0.###}", (t1 - t0).TotalMilliseconds);
                Console.WriteLine("Cercanía       : {0:0.###}", (t2 - t1).TotalMilliseconds);
                Console.WriteLine("Intermediación : {0:0.###}", (t3 - t2).TotalMilliseconds);
                Console.WriteLine("Eigenvector    : {0:0.###}", (t4 - t3).TotalMilliseconds);

                // ===== Top-K por métrica =====
                Console.WriteLine();
                PrintTopK("Top-K DegOut", Util.TopK(degOut, kTop));
                PrintTopK("Top-K DegIn",  Util.TopK(degIn,  kTop));
                PrintTopK("Top-K DegTot", Util.TopK(degTot, kTop));
                PrintTopK("Top-K Closeness", Util.TopK(clos, kTop));
                PrintTopK("Top-K Betweenness", Util.TopK(bet, kTop));
                PrintTopK("Top-K Eigenvector", Util.TopK(eig, kTop));

                // ===== Exportar CSV =====
                Directory.CreateDirectory("docs");
                var outCsv = Path.Combine("docs", "centralidades.csv");
                Util.WriteCsv(outCsv, n, degOut, degIn, degTot, clos, bet, eig);
                Console.WriteLine("\nCSV generado en: " + outCsv);

                // ===== Exportar DOT (Graphviz) =====
                string dotMetricName = metricDot;
                double[] metricForDot = eig;
                if (metricDot == "betweenness") metricForDot = bet;
                else if (metricDot == "closeness") metricForDot = clos;
                else if (metricDot == "deg_total") metricForDot = degTot;

                var outDot = Path.Combine("docs", $"grafo_{metricDot}.dot");
                Util.WriteDot(outDot, g, metricForDot, metricDot);
                Console.WriteLine("DOT generado en: " + outDot);
                Console.WriteLine("Para renderizar (si tienes Graphviz): dot -Tpng \"{0}\" -o \"{1}\"",
                                  outDot, Path.Combine("docs", $"grafo_{metricDot}.png"));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                Environment.Exit(1);
            }
        }

        static void PrintTopK(string title, List<(int node, double value)> pairs)
        {
            Console.WriteLine(title + ":");
            foreach (var p in pairs)
                Console.WriteLine($"  nodo={p.node} valor={p.value:0.000000}");
        }
    }
}
