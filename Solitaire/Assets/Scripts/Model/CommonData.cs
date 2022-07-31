using System;
using UnityEngine;

public enum GameState { Ready, Game, Win, Defeat}
public enum CombTypes { Ascending, Descending}
public enum CardSuits { Clubs, Spade, Diamonds, Hearts }
public enum CardNames
{
    Ace, Deuce, Three, Four, Five, Six, Seven,
    Eight, Nine, Ten, Jack, Queen, King
}
[Serializable]

public struct CardRaiting
{
    public int raiting;
    public string shortName;
    public CardNames name;
}

public struct CardValue
{
    public CardSuits suit;
    public CardRaiting raiting;
}

[Serializable]
public struct CardVisual
{
    public Color cardColor;
    public CardSuits suit;
    public Sprite frontSprite;
}

public struct CardStackVisual
{
    public float cardShift;
    public Vector3 stackStartPos;
}

[Serializable]
public class CombsData
{
    public bool isChanging;
    public int count;
    public int startRaiting;
    public CombTypes type;
}