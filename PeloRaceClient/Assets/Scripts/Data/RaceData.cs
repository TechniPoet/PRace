using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "NewRaceData", menuName = "PeloRace/RaceData", order = 1)]
    public class RaceData : ScriptableObject
    {
        public string DisplayName;
        public float RaceDistance;
        public float ScoreDistanceThresh;
        public float ScorePerTime;
        public float ScoreTimeDenominator;
    }
}