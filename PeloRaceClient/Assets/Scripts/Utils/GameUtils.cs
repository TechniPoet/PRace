using UnityEngine;

namespace Utils
{
    public static class GameUtils
    {
        /// <summary>
        /// Calculates a views 3D position based on the simulations 1D value
        /// </summary>
        /// <param name="simPosition">progress value from simulation</param>
        /// <param name="viewStart">3D view start position where simPosition 0 would be</param>
        /// <param name="dir">Direction in which the viewStart should be translated from</param>
        /// <returns></returns>
        public static Vector3 SimToViewPosition(float simPosition, Vector3 viewStart, Vector3 dir)
        {
            return viewStart + (simPosition * dir);
        }
    }
}