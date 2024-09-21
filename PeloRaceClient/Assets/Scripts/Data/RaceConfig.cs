using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "NewRaceData", menuName = "PeloRace/RaceData", order = 1)]
    public class RaceConfig : ScriptableObject
    {
        public string DisplayName;
        public float RaceDistance;
        public float ScoreDistanceThresh;
        public float ScorePerSecond;

        public PlayerConfig ControlledPlayer;
        public PlayerConfig BotPlayer;
    }
}