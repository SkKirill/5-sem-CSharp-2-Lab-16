using System.Collections;
using BalancedTree.Exceptions;

namespace BalancedTree.Trees;

public class UnmutableTree<T>(ITree<T> tree) : ITree<T>
    where T : IComparable<T>
{
    public int Count => tree.Count;
    public bool IsEmpty => tree.IsEmpty;
    public IEnumerable<T> Nodes => tree.Nodes;

    public void Add(T node)
    {
        throw new TreeUnmutableException();
    }

    public void Clear()
    {
        throw new TreeUnmutableException();
    }

    public bool Contains(T node)
    {
        return tree.Contains(node);
    }

    public bool Contains(ITree<T> tree1)
    {
        return tree.Contains(tree1);
    }

    public void Remove(T node)
    {
        throw new TreeUnmutableException();
    }

    public override string ToString()
    {
        return tree.ToString()!;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return tree.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}