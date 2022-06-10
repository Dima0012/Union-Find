using UnionFind;

// Hoshen-Kopelman implementation for a PBC square grid

const int n = 4;

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
var mLabels = HoshenKopelman(m, n, uf);

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

int[,] HoshenKopelman(int[,] C, int n, UnionFind<int> unionFind)
{
    var L = new int[n, n];
    var label = 1;

    for (var i = 0; i < n; i++)
    for (var j = 0; j < n; j++)
    {
        if (C[i, j] != 0) // Occupied
        {
            var occupied = C[i, j];
            var left = C[Pbc(i - 1), j];
            var leftLabel = L[Pbc(i - 1), j];
            var above = C[i, Pbc(j - 1)];
            var aboveLabel = L[i, Pbc(j - 1)];
            
            
            if (left == 0 && above == 0 || leftLabel == 0 && aboveLabel == 0) // Neither neighbors occupied nor labeled
            {
                L[i, j] = label;
                unionFind.MakeSet(L[i, j]);
                label += 1;
            }

            if (left != 0 && leftLabel != 0 && above == 0) // Left neighbor occupied and labeled
            {
                L[i, j] = unionFind.FindSet(leftLabel).Data; // Copy label
            }

            if (left == 0 && above != 0 && aboveLabel != 0) // Above neighbor occupied and labeled
            {
                L[i, j] = unionFind.FindSet(aboveLabel).Data; // Copy label
            }

            else if ( leftLabel != 0 && aboveLabel != 0 ) // Both neighbor occupied and labeled
            {
                unionFind.Union(leftLabel, aboveLabel);
                L[i, j] = unionFind.FindSet(above).Data; // Copy label
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