
using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    public int ID { get; }
    public int StackIndex { get; }
    public CardValue Value { get; }
}

public interface ICardStack
{
    public int StackIndex { get; }
    public ICard[] Cards { get; }
}
