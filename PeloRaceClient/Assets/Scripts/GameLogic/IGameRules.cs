using Data;

namespace GameLogic
{
    public interface IGameRules
    {
        /// <summary>
        /// Checks if the player's speed is close enough to the target speed to earn points.
        /// </summary>
        /// <param name="config">Race configuration data.</param>
        /// <param name="state">Current game state.</param>
        /// <param name="player">The player's simulated progress data.</param>
        /// <returns>True if the player's speed is within the score distance threshold of the target speed, otherwise false.</returns>
        public bool IsInScoreDistance(RaceConfig config, GameRunner.GameState state, GameRunner.RowerData player);

        /// <summary>
        /// Determines if any of the end game conditions have been met.
        /// </summary>
        /// <param name="config">Race configuration to use.</param>
        /// <param name="state">Current game state.</param>
        /// <returns>True if the race has ended, otherwise false.</returns>
        public bool IsEndConditionMet(RaceConfig config, GameRunner.GameState state);


        /// <summary>
        /// Runs all logic for a single simulation tick of the game.
        /// </summary>
        /// <param name="config">Race configuration to use.</param>
        /// <param name="state">Current game state.</param>
        /// <param name="deltaTime">Time elapsed since the last tick.</param>
        /// <returns>True if the simulation should continue, otherwise false.</returns>
        public bool SimulationTick(RaceConfig config, GameRunner.GameState state, float deltaTime);

        /// <summary>
        /// Sets the acceleration direction for a given rower.
        /// </summary>
        /// <param name="config">Race configuration to use.</param>
        /// <param name="state">Current game state.</param>
        /// <param name="id">ID of the rower to adjust.</param>
        /// <param name="up">If true, speed will increase; if false, speed will decrease.</param>
        public void AdjustRowerAcceleration(RaceConfig config, GameRunner.GameState state, GameRunner.RowerId id,
            bool up);

        /// <summary>
        /// Checks if the rower has finished the race based on their simulated position.
        /// </summary>
        /// <param name="config">Race configuration to use.</param>
        /// <param name="rower">Rower data to check.</param>
        /// <returns>True if the rower has completed the race distance, otherwise false.</returns>
        public bool FinishedRace(RaceConfig config, GameRunner.RowerData rower);

        /// <summary>
        /// Adjusts the scoretime for each rower based on their proximity to the target speed.
        /// </summary>
        /// <param name="config">Race configuration to use.</param>
        /// <param name="state">Current game state.</param>
        public void AdjustScores(RaceConfig config, GameRunner.GameState state, float deltaTime);

        /// <summary>
        /// Adjusts the speeds of all rowers based on their acceleration state.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <param name="deltaTime">Time elapsed since the last speed adjustment.</param>
        public void AdjustRowerSpeeds(GameRunner.GameState state, float deltaTime);

        /// <summary>
        /// Adjusts the positions of all rowers based on their current speed.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <param name="deltaTime">Time elapsed since the last position adjustment.</param>
        public void AdjustRowerPositions(GameRunner.GameState state, float deltaTime);

        /// <summary>
        /// Sets a new target speed for the rowers at defined intervals.
        /// </summary>
        /// <param name="config">Race configuration to use.</param>
        /// <param name="state">Current game state.</param>
        public void SetNewTargetSpeed(RaceConfig config, GameRunner.GameState state);

    }
}