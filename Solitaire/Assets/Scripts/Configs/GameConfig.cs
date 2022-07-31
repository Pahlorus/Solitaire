using System;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameConfig), menuName = "ScriptableObjects/GameEntity/" + nameof(GameConfig))]
public class GameConfig : ScriptableObject
{
    [Header("Anim")]
    public int timeDelayAfterAction; // ms

    [Header("Settings")]

    [Tooltip("To display debug information")]
    public bool isDebug;

    [Tooltip("Number of cards in hand")]
    public int baseDeckCardCount; 
    [Tooltip("Number of cards stacks in game field")]
    public int stackCardCount;
    public int minCombCardCount;
    public int maxCombCardCount;

    public CardsConfig cardConfig;

    public int cardDifferenceRaitingMin;

    [Header("CalculateSettings")]
    public int cardDifferenceRaitingMax;


    public void OnEnable()
    {
#if !UNITY_EDITOR
        isDebug = false;
#endif
        try
        {
            var raitings = cardConfig.cardRaiting;
            cardDifferenceRaitingMax =  Mathf.Abs(raitings[raitings.Count - 1].raiting - raitings[0].raiting);
        }
        catch (Exception e) 
        { 
            Debug.LogError($"[GameConfig][OnEnable] failed to calculate game parameter : {e.Message}"); 
        }

    }
}
