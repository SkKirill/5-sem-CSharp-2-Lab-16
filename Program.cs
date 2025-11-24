using System.Text;
using BalancedTree.Trees;
using BalancedTree.Utilities;

namespace BalancedTree;

internal static class Program
{
    public static void Main()
    {
        Console.InputEncoding = new UTF8Encoding();
        Console.OutputEncoding = new UTF8Encoding();

        // Тест ArrayTree
        ITree<int> tree = new ArrayTree<int>();

        Tests(tree);

        // Тест LinkedTree
        tree = new LinkedTree<int>();
        Tests(tree);

        // Тест UnmutableTree
        tree = new UnmutableTree<int>(tree);
        Tests(tree);
    }

    private static void Tests(ITree<int> tree)
    {
        Console.WriteLine(new string('=', 60));
        Console.Write($"Запуск тестов для дерева: ");
        Console.WriteLine($"{tree.GetType().Name}");
        Console.WriteLine(new string('=', 60));
        Console.WriteLine();

        // Добавление элементов
        tree.Add(1);
        tree.Add(5);
        tree.Add(13);
        tree.Add(17);
        
        PrintTree(tree);

        // Проверка Contains
        Console.WriteLine(tree.Contains(1));
        Console.WriteLine(tree.Contains(7));

        // Удаление элементов
        tree.Remove(1);
        PrintTree(tree);

        try
        {
            tree.Remove(6);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        PrintTree(tree);

        // Проверка утилит TreeUtils
        Console.WriteLine(TreeUtils<int>.Exists(tree, x => x > 3));
        Console.WriteLine(TreeUtils<int>.FindAll(tree, x => x > 3));
        Console.WriteLine(TreeUtils<int>.CheckForAll(tree, x => x > 3));
        

        // 5. Очистка дерева
        tree.Clear();
        PrintTree(tree);
    }

    private static void PrintTree(ITree<int> tree)
    {
        if (tree.IsEmpty)
            Console.WriteLine("Дерево пустое");
        
        Console.Write($"Элементы: ");
        TreeUtils<int>.ForEach(tree, x => Console.Write(x + " "));
        Console.WriteLine();
    }
}