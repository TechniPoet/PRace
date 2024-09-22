using Data;

namespace GameLogic
{
    public interface IGameRules
    {
        /// <summary>
        /// Are players close enough to earn points
        /// </summary>
        /// <returns></returns>
        public bool IsInScoreDistance(RaceConfig config, GameRunner.GameState state, GameRunner.RowerData player);

        /// <summary>
        /// Have any of the end game conditions been met?
        /// </summary>
        /// <param name="config">Race configuration to use</param>
        /// <param name="state">Current Game state</param>
        /// <returns></returns>
        public bool IsEndConditionMet(RaceConfig config, GameRunner.GameState state);

        public bool SimulationTick(RaceConfig config, GameRunner.GameState state, float deltaTime);

        public void AdjustRowerTargetSpeed(RaceConfig config, GameRunner.GameState state, GameRunner.RowerId id, bool up);
        public void SetNewTargetSpeed(RaceConfig config, GameRunner.GameState state);

        public void AdjustRowerSpeeds(GameRunner.GameState state);

    }
}