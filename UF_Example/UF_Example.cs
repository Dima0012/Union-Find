using System.Diagnostics;
using UnionFind;

// Hoshen-Kopelman implementation for a PBC square grid

namespace UF_Example;

internal static class MyClass
{
    private static void Main(string[] args)
    {
        var n = args.Length == 0 ? 10 : Convert.ToInt32(args[0]);
        
        //const int n = 80;
        var label = 1;

        var m = new int[n, n];
        var mLabels = new int[n, n];
        const double p = 0.5;

        var uf = new UnionFind<int>();
        var rng = new Random();
        var stopWatch = new Stopwatch();


        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
        {
            var r = rng.NextDouble();
            if (r < p)
            {
                m[i, j] = 1;
                mLabels[i, j] = label;
                label += 1;

                uf.MakeSet(mLabels[i, j]);
            }
            else
            {
                m[i, j] = 0;
                mLabels[i, j] = 0;
                uf.MakeSet(mLabels[i, j]);
            }
        }

        // Union Set
        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
            if (m[i, j] != 0)
            {
                // Look up
                if (m[Pbc(i - 1), j] != 0) uf.Union(mLabels[Pbc(i - 1), j], mLabels[i, j]);

                // Look left
                if (m[i, Pbc(j - 1)] != 0) uf.Union(mLabels[i, Pbc(j - 1)], mLabels[i, j]);
            }


        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
            mLabels[i, j] = uf.FindSet(mLabels[i, j]).Data;


        const string filepathLabels =
            "/Users/mdima/Desktop/Magistrale/Strutture Dati e Algoritmi/Union-Find/Cluster_testing/labels.txt";
        const string filepathClusters =
            "/Users/mdima/Desktop/Magistrale/Strutture Dati e Algoritmi/Union-Find/Cluster_testing/cluster.txt";


        using var outStreamLabels = new StreamWriter(filepathLabels);
        using var outStreamCluster = new StreamWriter(filepathClusters);


        for (var j = 0; j < n; j++)
        {
            for (var i = 0; i < n; i++)
            {
                outStreamLabels.Write(mLabels[i, j] + " ");
                outStreamCluster.Write(m[i, j] + " ");
            }

            outStreamLabels.WriteLine();
            outStreamCluster.WriteLine();
        }

        Console.WriteLine($"\nCluster and Labels matrices written successfully!\nGrid dimension is {n}x{n}.\n");

        // Periodic boundary conditions
        int Pbc(int i)
        {
            if (i >= n)
            {
                i -= n;
            }

            else if (i < 0)
            {
                i += n;
            }

            return i;
        }
    }
}