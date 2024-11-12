namespace stuff.graph.mwst.net;

internal class UnionFind
{
    private readonly Dictionary<long, long> parent;
    private readonly Dictionary<long, int> rank;

    public UnionFind(IEnumerable<long> elements)
    {
        parent = [];
        rank = [];

        foreach (var element in elements)
        {
            parent[element] = element;
            rank[element] = 0;
        }
    }

    public long Find(long element)
    {
        if (parent[element] != element)
        {
            parent[element] = Find(parent[element]);
        }
        return parent[element];
    }

    public void Union(long element1, long element2)
    {
        long root1 = Find(element1);
        long root2 = Find(element2);

        if (root1 != root2)
        {
            if (rank[root1] > rank[root2])
            {
                parent[root2] = root1;
            }
            else if (rank[root1] < rank[root2])
            {
                parent[root1] = root2;
            }
            else
            {
                parent[root2] = root1;
                rank[root1]++;
            }
        }
    }
}
