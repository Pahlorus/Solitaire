using UnityEngine;


[CreateAssetMenu(fileName = nameof(PrefConfig), menuName = "ScriptableObjects/GameEntity/" + nameof(PrefConfig))]
public class PrefConfig : ScriptableObject
{
    public CardView cardViewPref;
    public CardContainerView cardStackViewPref;
}
