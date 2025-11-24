using BalancedTree.Exceptions;
using BalancedTree.Trees;

namespace BalancedTree.Utilities;

public delegate bool CheckDelegate<in T>(T item);
public delegate void ActionDelegate<in T>(T item);
public delegate ITree<T> TreeConstructorDelegate<T>() where T : IComparable<T>;

public static class TreeUtils<T> where T : IComparable<T>
{
    /// <summary>
    /// Конструктор для массива-дерева.
    /// </summary>
    private static readonly TreeConstructorDelegate<T> ArrayTreeConstructor = () => new ArrayTree<T>();

    /// <summary>
    /// Конструктор для связанного дерева.
    /// </summary>
    private static readonly TreeConstructorDelegate<T> LinkedTreeConstructor = () => new LinkedTree<T>();


    /// <summary>
    /// Проверяет, существует ли в дереве элемент,
    /// удовлетворяющий проверке check.
    /// </summary>
    public static bool Exists(ITree<T>? tree, CheckDelegate<T> check)
    {
        return tree?.Any(item => check(item)) ?? throw new TreeNullException("Передано null вместо дерева.");
    }

    /// <summary>
    /// Возвращает новое дерево, содержащее только те элементы,
    /// которые удовлетворяют проверке check.
    /// </summary>
    public static ITree<T> FindAll(ITree<T> tree, CheckDelegate<T> check)
    {
        if (tree == null)
            throw new TreeNullException("Передано null вместо дерева.");

        // Получаем нужный конструктор и создаем дерево.
        var filteredTree = tree switch
        {
            ArrayTree<T> => ArrayTreeConstructor.Invoke(),
            LinkedTree<T> => LinkedTreeConstructor.Invoke(),
            _ => throw new TreeTypeUnsupportedException(
                $"Тип дерева '{tree.GetType().Name}' не поддерживается.")
        };

        // Заполняем коллекцию
        foreach (var item in filteredTree)
        {
            if (check(item))
                filteredTree.Add(item);
        }

        return filteredTree;
    }

    /// <summary>
    /// Выполняет действие для каждого элемента дерева.
    /// </summary>
    public static void ForEach(ITree<T> tree, ActionDelegate<T> action)
    {
        if (tree == null)
            throw new TreeNullException("Передано null вместо дерева.");

        foreach (var item in tree)
            action(item);
    }

    /// <summary>
    /// Проверяет, что ВСЕ элементы дерева удовлетворяют check.
    /// </summary>
    public static bool CheckForAll(ITree<T>? tree, CheckDelegate<T> check)
    {
        return tree?.All(item => check(item)) ?? throw new TreeNullException("Передано null вместо дерева.");
    }
}