using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "NewPlayerConfig", menuName = "PeloRace/PlayerConfig", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        public float MaxSpeed;
        public float MinSpeed;
        public float SpeedChangePerSecond;
        public float StartSpeed;
    }
}