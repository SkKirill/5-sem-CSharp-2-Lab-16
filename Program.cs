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
        ITree<MyClass> tree = new ArrayTree<MyClass>();

        Tests(tree);

        // Тест LinkedTree
        tree = new LinkedTree<MyClass>();
        Tests(tree);

        // Тест UnmutableTree
        tree = new UnmutableTree<MyClass>(tree);
        Tests(tree);
    }

    private static void Tests(ITree<MyClass> tree)
    {
        Console.WriteLine("Запуск тестов");

        // Добавление элементов
        try
        {
            tree.Add(new MyClass(1));
            tree.Add(new MyClass(5));
            tree.Add(new MyClass(13));
            tree.Add(new MyClass(-3));
            tree.Add(new MyClass(-7));
            tree.Add(new MyClass(-4));
            tree.Add(new MyClass(3));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine(tree);

        // Проверка Contains
        Console.WriteLine(tree.Contains(new MyClass(1)));
        Console.WriteLine(tree.Contains(new MyClass(7)));

        // Удаление элементов
        try
        {
            tree.Remove(new MyClass(1));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine(tree);

        try
        {
            tree.Remove(new MyClass(6));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine(tree);

        // Проверка утилит TreeUtils
        try
        {
            Console.WriteLine(TreeUtils<MyClass>.Exists(tree, x => x.CompareTo(new MyClass(3)) == 1));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        try
        {
            Console.WriteLine(TreeUtils<MyClass>.FindAll(tree, x => x.CompareTo(new MyClass(3)) == 1));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        try
        {
            Console.WriteLine(TreeUtils<MyClass>.CheckForAll(tree, x => x.CompareTo(new MyClass(3)) == 1));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        Console.Write($"Элементы: ");
        TreeUtils<MyClass>.ForEach(tree, x => Console.Write(x + " "));
        
        try
        {
            // Очистка дерева
            tree.Clear();
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private class MyClass(int value) : IComparable<MyClass>
    {
        private int Value { get; } = value;


        public int CompareTo(MyClass? other)
        {
            if (ReferenceEquals(this, other)) 
                return 0;

            return other is null ? 1 : Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}