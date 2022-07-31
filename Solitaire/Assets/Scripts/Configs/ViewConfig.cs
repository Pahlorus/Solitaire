using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ViewConfig), menuName = "ScriptableObjects/GameEntity/" + nameof(ViewConfig))]
public class ViewConfig : ScriptableObject
{
    [Header("Texts")]

    public string readyDescription;
    public string winDescription;
    public string defeatDescription;
}
