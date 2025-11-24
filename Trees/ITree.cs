namespace BalancedTree.Trees;

public interface ITree<T> : IEnumerable<T> where T : IComparable<T>
{
    int Count { get; }
    bool IsEmpty { get; }
    IEnumerable<T> Nodes { get; }

    void Add(T item);
    void Clear();
    bool Contains(T item);
    bool Contains(ITree<T> tree);
    void Remove(T item);
}