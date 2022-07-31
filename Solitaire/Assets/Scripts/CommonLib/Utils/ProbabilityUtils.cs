
using UnityEngine;

namespace CommonLib
{
    public static class ProbabilityUtils
    {
        public static bool GetRandomBool()
        {
            return Random.Range(0, 2) > 0;
        }
        public static int GetRandomIntByRange(int min, int maxIncl)
        {
            return Random.Range(min, maxIncl + 1);
        }

        public static bool GetRandomBoolByProbability(float probability)
        {
            var rnd = Random.value;
            return rnd <= probability;
        }
    }
}
