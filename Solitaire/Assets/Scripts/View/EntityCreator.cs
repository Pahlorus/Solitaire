using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCreator : MonoBehaviour
{
    [SerializeField] private PrefConfig _prefConfig;

    private T GeneralBaseCreate<T>(Transform parent, T pref) where T : MonoBehaviour
    {
        return Instantiate(pref, parent);
    }

    public CardView CreateCardView(Transform parent)
    {
        var entity = GeneralBaseCreate(parent, _prefConfig.cardViewPref);
        return entity;
    }

    public CardContainerView CreateCardCantainerView(Transform parent)
    {
        var entity = GeneralBaseCreate(parent, _prefConfig.cardStackViewPref);
        return entity;
    }
}
