namespace UnionFind;

/// <summary>
/// Node object for the Union-Find data structure.
/// </summary>
/// <typeparam name="T"> The type of data memorized in the Node </typeparam>
public class Node<T>
{
    public T Data { get; set; }
    public Node<T> Parent { get; set; }
    public int Rank { get; set; }

    /// <summary>
    /// Constructor for the Node object. It initialize the Data with the argument passed;
    /// sets the Node's Parent as itself and sets the Node's Rank to 0.
    /// </summary>
    /// <param name="data"> Data to be memorized in the node. </param>
    public Node(T data)
    {
        Data = data;
        Parent = this;
        Rank = 0;
    }
}

public class UnionFind<T> where T : notnull
{
    Dictionary<T, Node<T>> _nodes;

    public UnionFind()
    {
        _nodes = new Dictionary<T, Node<T>>();
    }

    public bool HasData(T data)
    {
        return _nodes.ContainsKey(data);
    }

    public void MakeSet(T data)
    {
        // check if data is already present, if so skip
        if (HasData(data))
        {
            _nodes.Add(data, new Node<T>(data));
        }
    }

    public void Union(T dataA, T dataB)
    {
        var x = _nodes[dataA];
        var y = _nodes[dataB];
        Link(FindSet(x), FindSet(y));
    }

    public void Link(Node<T> x, Node<T> y)
    {
        // get nodes from dictionary
        // var x = _nodes[dataA];
        // var y = _nodes[dataB];

        if (x.Rank > y.Rank)
        {
            y.Parent = x;
        }
        else
        {
            x.Parent = y;
            if (x.Rank == y.Rank)
            {
                y.Rank += 1;
            }
        }
    }

    public Node<T> FindSet(Node<T> x)
    {
        if (x != x.Parent)
        {
            x.Parent = FindSet(x.Parent);
        }

        return x.Parent;
    }
}