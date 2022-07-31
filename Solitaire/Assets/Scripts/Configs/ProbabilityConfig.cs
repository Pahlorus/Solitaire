using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ProbabilityConfig), menuName = "ScriptableObjects/GameEntity/" + nameof(ProbabilityConfig))]
public class ProbabilityConfig : ScriptableObject
{
    public float ascendingProbabilty;
    public float changingProbabilty;
}
