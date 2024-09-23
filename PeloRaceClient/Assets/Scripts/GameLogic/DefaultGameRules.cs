using System.Linq;
using Data;
using UnityEngine;

namespace GameLogic
{
    public class DefaultGameRules : IGameRules
    {
        /// <summary>
        /// Checks if the player's speed is close enough to the target speed to earn points.
        /// </summary>
        /// <param name="config">Race configuration data.</param>
        /// <param name="state">Current game state.</param>
        /// <param name="player">The player's simulated progress data.</param>
        /// <returns>True if the player's speed is within the score distance threshold of the target speed, otherwise false.</returns>
        public bool IsInScoreDistance(RaceConfig config, GameRunner.GameState state, GameRunner.RowerData player)
        {
            return Mathf.Abs(player.Speed - state.TargetSpeed) <= config.ScoreSpeedDistanceThresh;
        }

        /// <summary>
        /// Determines if any of the end game conditions have been met.
        /// </summary>
        /// <param name="config">Race configuration to use.</param>
        /// <param name="state">Current game state.</param>
        /// <returns>True if the race has ended, otherwise false.</returns>
        public bool IsEndConditionMet(RaceConfig config, GameRunner.GameState state)
        {
            if (state.RaceEnded || state.RowerDatas.All(rowerEntry =>  FinishedRace(config, rowerEntry.Value)))
            {
                state.RaceEnded = true;
            }

            return state.RaceEnded;
        }


        /// <summary>
        /// Runs all logic for a single simulation tick of the game.
        /// </summary>
        /// <param name="config">Race configuration to use.</param>
        /// <param name="state">Current game state.</param>
        /// <param name="deltaTime">Time elapsed since the last tick.</param>
        /// <returns>True if the simulation should continue, otherwise false.</returns>
        public bool SimulationTick(RaceConfig config, GameRunner.GameState state, float deltaTime)
        {
            state.RaceTimeElapsed += deltaTime;

            SetNewTargetSpeed(config, state);
            AdjustRowerSpeeds(state, deltaTime);
            AdjustRowerPositions(state, deltaTime);
            AdjustScores(config, state, deltaTime);
            
            return !IsEndConditionMet(config, state);
        }

        /// <summary>
        /// Sets the acceleration direction for a given rower.
        /// </summary>
        /// <param name="config">Race configuration to use.</param>
        /// <param name="state">Current game state.</param>
        /// <param name="id">ID of the rower to adjust.</param>
        /// <param name="up">If true, speed will increase; if false, speed will decrease.</param>
        public void AdjustRowerAcceleration(RaceConfig config, GameRunner.GameState state, GameRunner.RowerId id, bool up)
        {
            state.RowerDatas[id].SpeedUp = up;
        }

        /// <summary>
        /// Checks if the rower has finished the race based on their simulated position.
        /// </summary>
        /// <param name="config">Race configuration to use.</param>
        /// <param name="rower">Rower data to check.</param>
        /// <returns>True if the rower has completed the race distance, otherwise false.</returns>
        public bool FinishedRace(RaceConfig config, GameRunner.RowerData rower)
        {
            return rower.SimPosition > config.RaceDistance;
        } 

        /// <summary>
        /// Adjusts the scoretime for each rower based on their proximity to the target speed.
        /// </summary>
        /// <param name="config">Race configuration to use.</param>
        /// <param name="state">Current game state.</param>
        public void AdjustScores(RaceConfig config, GameRunner.GameState state, float deltaTime)
        {
            foreach (var rower in state.RowerDatas)
            {
                if (IsInScoreDistance(config, state, rower.Value) && !FinishedRace(config, rower.Value))
                {
                    rower.Value.ScoreTime += deltaTime;
                }
            }
        }

        /// <summary>
        /// Adjusts the speeds of all rowers based on their acceleration state.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <param name="deltaTime">Time elapsed since the last speed adjustment.</param>
        public void AdjustRowerSpeeds(GameRunner.GameState state, float deltaTime)
        {
            foreach (var rower in state.RowerDatas)
            {
                rower.Value.Speed += deltaTime * rower.Value.SpeedChangePerSecond * 
                                     (rower.Value.SpeedUp ? 1 : -1);
                
                rower.Value.Speed = Mathf.Clamp(rower.Value.Speed, rower.Value.MinSpeed, rower.Value.MaxSpeed);
            }
        }
        
        /// <summary>
        /// Adjusts the positions of all rowers based on their current speed.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <param name="deltaTime">Time elapsed since the last position adjustment.</param>
        public void AdjustRowerPositions(GameRunner.GameState state, float deltaTime)
        {
            foreach (var rower in state.RowerDatas)
            {
                rower.Value.SimPosition += deltaTime * rower.Value.Speed;
            }
        }

        /// <summary>
        /// Sets a new target speed for the rowers at defined intervals.
        /// </summary>
        /// <param name="config">Race configuration to use.</param>
        /// <param name="state">Current game state.</param>
        public void SetNewTargetSpeed(RaceConfig config, GameRunner.GameState state)
        {
            // Change target speed on first pass or after every time interval
            if (state.RaceTimeElapsed - state.LastTargetSpeedChange >= config.TargetSpeedChangeInterval || state.LastTargetSpeedChange == 0)
            {
                state.TargetSpeed = Random.Range(config.MinTargetSpeed, config.MaxTargetSpeed);
                state.LastTargetSpeedChange = state.RaceTimeElapsed;
            }
        }
    }
}