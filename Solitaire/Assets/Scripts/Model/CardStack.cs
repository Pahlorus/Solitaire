
using System.Collections.Generic;

public class CardStack: ICardStack
{
    private int _stackIndex;
    private Stack<Card> _stack;

    public CardStack(int stackIndex=-1)
    {
        _stackIndex = stackIndex;
        _stack = new Stack<Card>();
    }

    public int Count => _stack.Count;
    public Stack<Card> Stack => _stack;

    public int StackIndex => _stackIndex;

    public ICard[] Cards => _stack.ToArray();

    public void Push(Card card)
    {
        card.SetIndex(_stackIndex);
        _stack.Push(card);
    }

    public Card Peek()
    {
        return _stack.Peek();
    }

    public Card Get()
    {
        return _stack.Pop();
    }

    public void Clear()
    {
        _stack.Clear();
    }

}
