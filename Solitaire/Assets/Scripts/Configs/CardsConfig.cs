using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(CardsConfig), menuName = "ScriptableObjects/GameEntity/" + nameof(CardsConfig))]
public class CardsConfig : ScriptableObject
{
    [Header("CommonSettings")]
    public Sprite reversCardSprite;
    public List<CardVisual> cardVisualList;
    public List<CardRaiting> cardRaiting;
    public Dictionary<CardSuits, CardVisual> spriteDic = new Dictionary<CardSuits, CardVisual>();
    public Dictionary<int, CardRaiting> raitingDic = new Dictionary<int, CardRaiting>();
    [Header("AnimSettings")]
    public float cardMoveDuration;
    public float cardDealDuration;
    public Ease moveEase;
    public Ease dealEase;

    [Header("CalculateSettings")]
    public int minRaiting;
    public int maxRaiting;

    private void OnEnable()
    {
        FillCardSpriteDictionary();
        FillCardRaitingDictionary();
        try
        {
            maxRaiting = cardRaiting[cardRaiting.Count - 1].raiting;
            minRaiting = cardRaiting[0].raiting;

        }
        catch (Exception e)
        {
            Debug.LogError($"[CardsConfig][OnEnable] failed to calculate card parameter : {e.Message}");
        }
    }

    public void FillCardSpriteDictionary()
    {
        foreach (var item in cardVisualList)
        {
            spriteDic.Add(item.suit, item);
        }
    }

    public void FillCardRaitingDictionary()
    {
        foreach (var item in cardRaiting)
        {
            raitingDic.Add(item.raiting, item);
        }
    }
}
