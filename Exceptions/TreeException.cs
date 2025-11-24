namespace BalancedTree.Exceptions;

public class TreeException : Exception
{
    protected TreeException(string message = "Произошла ошибка при работе с деревом.") : base(message)
    {
    }
}

public class TreeNullException(string message = "Значение не может быть null.") : TreeException(message);

public class TreeTypeUnsupportedException(string message = "Данный тип дерева не поддерживается.") : TreeException(message);

public class TreeItemNotFoundException(string message = "Указанный элемент не найден в сбалансированном дереве.")
    : TreeException(message);

public class TreeDuplicateValueException(string message = "Нельзя вставить данное значение")
    : TreeException(message);

public class TreeUnmutableException(string message = "Невозможная операция.") : TreeException(message);