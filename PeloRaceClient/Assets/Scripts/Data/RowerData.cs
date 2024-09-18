using UnityEngine;

namespace PeloRace.Scripts.Data
{
    [CreateAssetMenu(fileName = "NewPlayerControlData", menuName = "PeloRace/PlayerControlData", order = 0)]
    public class RowerData : ScriptableObject
    {
        public float MaxSpeed;
        public float MinSpeed;
        public float ChangeInterval;
        public float StartSpeed;
    }
}