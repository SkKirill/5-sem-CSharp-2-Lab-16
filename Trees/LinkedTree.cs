using System.Collections;
using BalancedTree.Exceptions;

namespace BalancedTree.Trees;

public class LinkedTree<T> : ITree<T> where T : IComparable<T>
{
    private Node<T>? _root;
    public int Count { get; private set; }
    public bool IsEmpty => Count == 0;

    public IEnumerable<T> Nodes
    {
        get
        {
            if (_root != null)
            {
                foreach (var value in Traverse(_root))
                    yield return value;
            }
        }
    }

    public void Add(T item)
    {
        if (item == null)
            throw new TreeNullException();

        if (_root == null)
        {
            _root = new Node<T>(item);
            Count = 1;
            return;
        }

        if (NeedsRebalancing())
        {
            Rebalance();
        }

        AddRecursive(_root, item);
    }

    public void Clear()
    {
        _root = null;
        Count = 0;
    }

    public bool Contains(T item)
    {
        if (item == null)
            throw new TreeNullException();

        return ContainsRecursive(_root, item);
    }

    public bool Contains(ITree<T> tree)
    {
        if (tree == null)
            throw new TreeNullException();

        foreach (var node in tree.Nodes)
        {
            if (!Contains(node))
                return false;
        }

        return true;
    }

    public void Remove(T item)
    {
        if (item == null)
            throw new TreeNullException();

        (_root, var removed) = RemoveRecursive(_root, item);

        if (!removed)
            throw new TreeItemNotFoundException();

        Count--;
        if (NeedsRebalancing())
        {
            Rebalance();
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Nodes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        if (_root == null)
            return "[empty]";

        return BuildTreeString(_root, "", true);
    }

    private string BuildTreeString(Node<T>? node, string indent, bool isLast)
    {
        if (node == null)
            return "";

        string result = indent;

        if (isLast)
        {
            result += "└─";
            indent += "  ";
        }
        else
        {
            result += "├─";
            indent += "| ";
        }

        result += node.Value + "\n";

        bool hasLeft = node.Left != null;
        bool hasRight = node.Right != null;

        if (hasLeft)
            result += BuildTreeString(node.Left, indent, !hasRight);

        if (hasRight)
            result += BuildTreeString(node.Right, indent, true);

        return result;
    }

    private bool NeedsRebalancing()
    {
        if (_root == null) return false;

        int GetHeight(Node<T>? node)
        {
            if (node == null) return 0;
            return 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        }

        int leftHeight = GetHeight(_root.Left);
        int rightHeight = GetHeight(_root.Right);

        return Math.Abs(leftHeight - rightHeight) > 1;
    }

    private void Rebalance()
    {
        if (_root == null) 
            return;

        // собрать все значения в списке и отсортировать
        var nodes = Nodes.ToList();
        nodes.Sort();

        // строим идеально сбалансированное BST
        Node<T>? BuildBalanced(int left, int right)
        {
            if (left > right)
                return null;

            int mid = (left + right) / 2;
            var node = new Node<T>(nodes[mid])
            {
                Left = BuildBalanced(left, mid - 1),
                Right = BuildBalanced(mid + 1, right)
            };

            return node;
        }

        _root = BuildBalanced(0, nodes.Count - 1);
    }


    private void AddRecursive(Node<T> current, T item)
    {
        var cmp = item.CompareTo(current.Value);
        switch (cmp)
        {
            case 0:
                throw new TreeDuplicateValueException();
            case < 0 when current.Left == null:
                current.Left = new Node<T>(item);
                Count++;
                break;
            case < 0:
                AddRecursive(current.Left, item);
                break;
            default:
            {
                if (current.Right == null)
                {
                    current.Right = new Node<T>(item);
                    Count++;
                }
                else
                {
                    AddRecursive(current.Right, item);
                }

                break;
            }
        }
    }

    private (Node<T>?, bool) RemoveRecursive(Node<T>? node, T item)
    {
        if (node == null)
            return (null, false);

        var cmp = item.CompareTo(node.Value);
        switch (cmp)
        {
            case < 0:
            {
                (node.Left, var removed) = RemoveRecursive(node.Left, item);
                return (node, removed);
            }
            case > 0:
            {
                (node.Right, var removed) = RemoveRecursive(node.Right, item);
                return (node, removed);
            }
            default:
            {
                // Узел найден
                if (node.Left == null) return (node.Right, true);
                if (node.Right == null) return (node.Left, true);

                // Два потомка: замена на минимальный элемент в правом поддереве
                var minLargerNode = FindMin(node.Right);
                node.Value = minLargerNode.Value;
                (node.Right, _) = RemoveRecursive(node.Right, minLargerNode.Value);
                return (node, true);
            }
        }
    }

    private static bool ContainsRecursive(Node<T>? node, T item)
    {
        if (node == null)
            return false;

        var cmp = item.CompareTo(node.Value);
        if (cmp == 0) return true;
        return cmp < 0 ? ContainsRecursive(node.Left, item) : ContainsRecursive(node.Right, item);
    }

    private static IEnumerable<T> Traverse(Node<T> node)
    {
        yield return node.Value;
        if (node.Left != null)
        {
            foreach (var value in Traverse(node.Left))
                yield return value;
        }

        if (node.Right == null) yield break;

        foreach (var value in Traverse(node.Right))
            yield return value;
    }

    private static Node<T> FindMin(Node<T> node)
    {
        while (node.Left != null)
            node = node.Left;
        return node;
    }

    private class Node<TNode>(TNode value)
    {
        public TNode Value { get; set; } = value;
        public Node<TNode>? Left { get; set; }
        public Node<TNode>? Right { get; set; }
    }
}