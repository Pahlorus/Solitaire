using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(CardStackConfig), menuName = "ScriptableObjects/GameEntity/" + nameof(CardStackConfig))]
public class CardStackConfig : ScriptableObject
{
    public List<CardStackVisual> cardStackVisualList;
}
