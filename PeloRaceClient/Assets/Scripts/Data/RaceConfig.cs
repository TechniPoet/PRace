using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "NewRaceData", menuName = "PeloRace/RaceData", order = 1)]
    public class RaceConfig : ScriptableObject
    {
        public string DisplayName;
        public float RaceDistance;
        public float ScoreSpeedDistanceThresh;
        public float ScorePerSecond;

        public float TargetSpeedChangeInterval;
        public float MinTargetSpeed;
        public float MaxTargetSpeed;

        public PlayerConfig ControlledPlayer;
        public PlayerConfig BotPlayer;
    }
}