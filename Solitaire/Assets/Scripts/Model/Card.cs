using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ICard
{
    private int _id;
    private int _stackIndex;
    public int ID => _id;
    private CardValue _value;
    public int StackIndex => _stackIndex;

    public CardValue Value => _value;

    public Card()
    {
        _id = CardUtils.GetId();
    }

    public void SetIndex(int stackIndex)
    {
        _stackIndex = stackIndex;
    }

    public void SetCard(CardValue value)
    {
        _value = value;
    }

    public int GetRaiting()
    {
        return Value.raiting.raiting;
    }
}
