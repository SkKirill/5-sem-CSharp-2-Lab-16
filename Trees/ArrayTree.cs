using System.Collections;
using BalancedTree.Exceptions;

namespace BalancedTree.Trees;

// Бинарное дерево поиска, реализованное с использованием массива (как структура кучи)
// Правила:
// • корень = индекс 0
// • левый потомок = 2*i + 1
// • правый потомок = 2*i + 2
// • левый < корень < правый
// Размер массива увеличивается автоматически путём удвоения
public class ArrayTree<T> : ITree<T> where T : IComparable<T>
{
    private T?[] _array = new T?[4];
    public int Count { get; private set; }
    public bool IsEmpty => Count == 0;

    public IEnumerable<T> Nodes
    {
        get
        {
            var list = new List<T>();
            foreach (var node in _array)
            {
                if (node is not null)
                {
                    list.Add(node);
                }
            }

            return list;
        }
    }

    public void Add(T item)
    {
        if (item == null)
            throw new TreeNullException();

        // Если нет корня
        if (IsEmpty)
        {
            _array[0] = item;
            Count = 1;
            return;
        }

        if (NeedsRebalancing())
        {
            Rebalance();
        }

        var index = 0;
        while (true)
        {
            EnsureCapacity(index);
            var cmp = item.CompareTo(_array[index]);
            switch (cmp)
            {
                case < 0:
                {
                    var left = 2 * index + 1;
                    EnsureCapacity(left);
                    if (Equals(_array[left], null))
                    {
                        _array[left] = item;
                        Count++;
                        return;
                    }

                    index = left;
                    break;
                }
                case > 0:
                {
                    var right = 2 * index + 2;
                    EnsureCapacity(right);
                    if (Equals(_array[right], null))
                    {
                        _array[right] = item;
                        Count++;
                        return;
                    }

                    index = right;
                    break;
                }
                default:
                    throw new TreeDuplicateValueException();
            }
        }
    }

    public void Clear()
    {
        _array = new T[4];
        Count = 0;
    }

    public bool Contains(T item)
    {
        if (item == null)
            throw new TreeNullException();

        var index = 0;
        while (index < _array.Length)
        {
            var cmp = item.CompareTo(_array[index]);
            if (Equals(_array[index], null))
                return false;

            switch (cmp)
            {
                case 0:
                    return true;
                case < 0:
                    index = 2 * index + 1;
                    break;
                default:
                    index = 2 * index + 2;
                    break;
            }
        }

        return false;
    }

    public bool Contains(ITree<T> tree)
    {
        if (tree == null)
            throw new TreeNullException();


        foreach (var node in tree.Nodes)
        {
            if (!Equals(node, null) && !Contains(node))
                return false;
        }

        return true;
    }

    public void Remove(T item)
    {
        if (item == null)
            throw new TreeNullException();


        var index = 0;
        while (index < _array.Length)
        {
            var cmp = item.CompareTo(_array[index]);


            switch (cmp)
            {
                case 0:
                    _array[index] = default;
                    Count--;
                    var rebase = new List<T>();
                    CutBranch(ref rebase, index);
                    foreach (var node in rebase)
                    {
                        Add(node);
                    }

                    if (NeedsRebalancing())
                    {
                        Rebalance();
                    }

                    return;
                case < 0:
                    index = 2 * index + 1;
                    break;
                default:
                    index = 2 * index + 2;
                    break;
            }
        }

        throw new TreeItemNotFoundException();
    }

    public override string ToString()
    {
        return IsEmpty ? "[empty]" : BuildTreeString(0, "", true);
    }

    private string BuildTreeString(int index, string indent, bool isLast)
    {
        if (index >= _array.Length || _array[index] == null)
            return "";

        var result = indent;

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

        result += _array[index] + "\n";

        var left = 2 * index + 1;
        var right = 2 * index + 2;

        var hasLeft = left < _array.Length && _array[left] != null;
        var hasRight = right < _array.Length && _array[right] != null;

        if (hasLeft)
            result += BuildTreeString(left, indent, !hasRight);

        if (hasRight)
            result += BuildTreeString(right, indent, true);

        return result;
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var t in _array)
        {
            if (!Equals(t, null))
                continue;

            if (t != null)
                yield return t;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private bool NeedsRebalancing()
    {
        if (IsEmpty) return false;

        int GetHeight(int index)
        {
            if (index >= _array.Length || _array[index] == null)
                return 0;

            int left = 2 * index + 1;
            int right = 2 * index + 2;

            return 1 + Math.Max(GetHeight(left), GetHeight(right));
        }

        int leftHeight = GetHeight(1);
        int rightHeight = GetHeight(2);

        return Math.Abs(leftHeight - rightHeight) > 1;
    }

    private void Rebalance()
    {
        if (IsEmpty) return;

        var nodes = Nodes.ToList();
        nodes.Sort();

        Clear();

        // построить сбалансированное дерево
        void BuildBalanced(int left, int right, int index)
        {
            if (left > right)
                return;

            int mid = (left + right) / 2;

            EnsureCapacity(index);
            _array[index] = nodes[mid];
            Count++;

            BuildBalanced(left, mid - 1, 2 * index + 1);
            BuildBalanced(mid + 1, right, 2 * index + 2);
        }

        BuildBalanced(0, nodes.Count - 1, 0);
    }


    private void CutBranch(ref List<T> list, int index)
    {
        if (2 * index + 1 <= _array.Length && _array[2 * index + 1] is not null)
        {
            list.Add(_array[2 * index + 1]!);
            _array[2 * index + 1] = default;
            Count--;
            CutBranch(ref list, 2 * index + 1);
        }

        if (2 * index + 2 <= _array.Length && _array[2 * index + 2] is not null)
        {
            list.Add(_array[2 * index + 2]!);
            _array[2 * index + 2] = default;
            Count--;
            CutBranch(ref list, 2 * index + 2);
        }
    }

    private void EnsureCapacity(int index)
    {
        if (index < _array.Length)
            return;

        var newSize = _array.Length;
        while (newSize <= index)
            newSize *= 2;

        Array.Resize(ref _array, newSize);
    }
}