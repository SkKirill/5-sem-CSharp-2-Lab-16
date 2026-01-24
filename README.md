### Скоморохов Кирилл | 8 (900) 988-75-37 | Т-Банк | tg = @sk_kiriII | vk = sk_kirill | Rider | C#  

**Сбалансированное дерево**  
> Разработать обобщенный класс `Tree<T>` - класс для описания сбалансированного дерева.
> `ITree<T> : IEnumerable<T>` – базовый интерфейс для всех сбалансированных деревьев;

**Методы:**
- `void Add(T node);`
- `void Clear();`
- `bool Contains(T node);`
- `void Remove(T node);`

**Свойства:**
- `int Count;`
- `bool IsEmpty;`
- `IEnumerable<T> nodes;`

`TreeException` – класс, описывающий исключения, которые могут происходить в ходе работы с сбалансированного дерева (также можно
написать ряд наследников от `TreeException`);  

`ArrayTree<T>: ITree<T>` – класс сбалансированного дерева на основе массива;  
`LinkedTree<T>: ITree<T>` – класс сбалансированного дерева на основе связного списка;  
`UnmutableTree<T>: ITree<T>` – класс неизменяющегося сбалансированного дерева, является оберткой над любым существующим сбалансированным деревом (должен кидаться исключениями на вызов любого метода, изменяющего дерево);  

`TreeUtils` – класс различных операций над сбалансированным деревом;   

**Методы:**
- `static bool Exists<T>(ITree<T> tree, CheckDelegate<T> check);`
- `static ITree<T> FindAll<T>(ITree<T> tree, CheckDelegate<T> check;`
- `static void ForEach<T>(ITree<T> tree, ActionDelegate<T> action;`
- `static bool CheckForAll<T>(ITree<T> tree, ActionDelegate<T> action;`

**Cвойства:**
- `static readonly TreeConstructorDelegate<T> ArrayTreeConstructor;`
- `static readonly TreeConstructorDelegate<T> LinkedTreeConstructor;`

# 🌳 Что такое сбалансированное дерево?

Бинарное дерево поиска (BST) — это структура вида:
```
      (root)
     /      \
   left     right
```

Где:
* левый потомок (`left`) < родителя (`root`)
* правый потомок (`right`) > родителя (`root`)
* нет ограничений на высоту

> Бинарное дерево — это где слева меньше, справа больше

# Что такое сбалансированное дерево

> *Сбалансированное дерево* — это бинарное дерево (обычно бинарное дерево поиска), в котором поддерживается ограничение на высоту.
То есть дерево автоматически перестраивается так, чтобы высота была минимальной или около минимальной.

> Но любое сбалансированное дерево меняет структуру (повороты).
Например:
```
        10            8
       /       →     / \
      8             5   10
     / 
    5  
```

# Структура проекта

```
Partially-Ordered-List/
├── Trees/
│   ├── ArrayTree.cs
│   ├── LinkedTree.cs
│   ├── UnmuntadleTree.cs
│   └── ITree.cs
├── Exceptions/
│   └── TreeException.cs
│       ├── TreeException
│       ├── TreeNullException
│       ├── TreeDelegateNullException
│       ├── TreeItemNotFoundException
│       └── TreeInvalidOperationException
├── Utilites/
│   └── TreeUtils.cs
└── Program.cs
```

# public class ArrayTree<T> : ITree<T>

> Через массив будет реализовано способом расширения массива в 2 раза каждый раз когда в нем заканчивается место для элементов

Используется подход:
Корень — индекс 0:
- Левый потомок i → 2*i + 1
- Правый потомок i → 2*i + 2
