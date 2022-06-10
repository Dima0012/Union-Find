using UnionFind;

// Hoshen-Kopelman implementation for a PBC square grid

const int n = 10;

var m = new int[n, n];
const double p = 0.5;

var uf = new UnionFind<int>();
var rng = new Random();


for (var i = 0; i < n; i++)
for (var j = 0; j < n; j++)
{
    var r = rng.NextDouble();
    if (r < p)
    {
        m[i, j] = 1;
    }
    else
    {
        m[i, j] = 0;
    }
}

// RUn HK
var mLabels = HoshenKopelman(m);

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

Console.WriteLine("Cluster and Labels matrices written successfully!");

// ****************** ======= ******************

int[,] HoshenKopelman(int[,] C)
{
    var L = new int[n, n];
    var label = 1;

    for (var i = 0; i < n; i++)
    for (var j = 0; j < n; j++)
    {
        if (C[i, j] != 0) // Occupied
        {
            if (C[Pbc(i - 1), j] == 0 && C[i, Pbc(j - 1)] == 0) // Neither neighbors occupied
            {
                L[i, j] = label;
                uf.MakeSet(L[i, j]);
                label += 1;
            }

            if (C[Pbc(i - 1), j] != 0 && L[Pbc(i - 1), j] != 0 && C[i, Pbc(j - 1)] == 0) // Left neighbor occupied and labeled
            {
                L[i, j] = uf.FindSet(L[Pbc(i - 1), j]).Data; // Copy label
            }

            if (C[Pbc(i - 1), j] == 0 && C[i, Pbc(j - 1)] != 0 && L[i, Pbc(j - 1)] != 0) // Above neighbor occupied and labeled
            {
                L[i, j] = uf.FindSet(L[i, Pbc(j - 1)]).Data; // Copy label
            }

            else if ( L[Pbc(i - 1), j] != 0 && L[i, Pbc(j - 1)] != 0 )
            {
                uf.Union(L[Pbc(i - 1), j], L[i, Pbc(j - 1)]);
                L[i, j] = uf.FindSet(L[i - 1, j]).Data; // Copy label
            }
        }
    }

    return L;
}


// Periodic boundary conditions
int Pbc(int i)
{
    switch (i)
    {
        case >= n:
            i -= n;
            break;
        case < 0:
            i += n;
            break;
    }

    return i;
}