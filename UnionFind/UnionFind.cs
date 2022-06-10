using System.Collections;

namespace UnionFind;

/// <summary>
///     Node object for the Union-Find data structure.
/// </summary>
/// <typeparam name="T"> The type of data memorized in the Node </typeparam>
public class Node<T> where T : IComparable<T>
{
    /// <summary>
    ///     Constructor for the Node object. It initialize the Data with the argument passed;
    ///     sets the Node's Parent as itself and sets the Node's Rank to 0.
    /// </summary>
    public Node(T data)
    {
        Data = data;
        Parent = this;
        Rank = 0;
    }

    public T Data { get; }
    public Node<T> Parent { get; set; }
    public int Rank { get; set; }
}

/// <summary>
///     Union-Find data structure for memorization od disjoint sets.
/// </summary>
/// <typeparam name="T"> The generic data-type to be memorized</typeparam>
public class UnionFind<T> : IEnumerable<T> where T : IComparable<T>
{
    public UnionFind()
    {
        Nodes = new Dictionary<T, Node<T>>();
    }

    private Dictionary<T, Node<T>> Nodes { get; }

    public int Count => Nodes.Count;

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Nodes.Keys.GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Nodes.Keys.GetEnumerator();
    }

    /// <summary>
    ///     Removes all keys and values from the dictionary.
    /// </summary>
    public void Clear()
    {
        Nodes.Clear();
    }

    /// <summary>
    ///     Checks if data is present in the dictionary.
    /// </summary>
    private bool HasData(T data)
    {
        return Nodes.ContainsKey(data);
    }

    /// <summary>
    ///     Makes a new set out of data, creating a new Node with default initialization and returns true.
    ///     If the data is already present, returns false.
    /// </summary>
    public bool MakeSet(T data)
    {
        if (HasData(data)) return false;
        Nodes.Add(data, new Node<T>(data));
        return true;
    }

    /// <summary>
    ///     Performs a union operation of dataA, dataB by invoking Link on the corresponding Nodes.
    ///     Returns false if one of the data is not present in the sets.
    /// </summary>
    public bool Union(T dataA, T dataB)
    {
        // if (!HasData(dataA) || !HasData(dataB) )
        // {
        //     return false;
        // }
        var x = Nodes[dataA];
        var y = Nodes[dataB];
        Link(FindSet(x), FindSet(y));
        return true;
    }

    /// <summary>
    ///     Links two Nodes by rank.
    /// </summary>
    private static void Link(Node<T> x, Node<T> y)
    {
        if (x.Rank > y.Rank)
        {
            y.Parent = x;
        }
        else
        {
            x.Parent = y;
            if (x.Rank == y.Rank) y.Rank += 1;
        }
    }

    /// <summary>
    ///     Returns the root for the Node x. Also applies path compression to the Node's path
    /// </summary>
    private static Node<T> FindSet(Node<T> x)
    {
        if (x != x.Parent) x.Parent = FindSet(x.Parent);

        return x.Parent;
    }

    public Node<T> FindSet(T data)
    {
        var x = Nodes[data];
        if (x != x.Parent) x.Parent = FindSet(x.Parent);

        return x.Parent;
    }
}