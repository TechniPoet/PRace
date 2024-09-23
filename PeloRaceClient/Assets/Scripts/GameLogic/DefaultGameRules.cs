using System.Linq;
using Data;
using UnityEngine;

namespace GameLogic
{
    public class DefaultGameRules : IGameRules
    {
        /// <summary>
        /// Is players speed close enough to target speed to earn points
        /// </summary>
        /// <param name="config">Race Data Used</param>
        /// <param name="player">players simulated progress</param>
        /// <returns></returns>
        public bool IsInScoreDistance(RaceConfig config, GameRunner.GameState state, GameRunner.RowerData player)
        {
            return Mathf.Abs(player.Speed - state.TargetSpeed) <= config.ScoreSpeedDistanceThresh;
        }

        /// <summary>
        /// Have any of the end game conditions been met?
        /// </summary>
        /// <param name="config">Race configuration to use</param>
        /// <param name="state">Current Game state</param>
        /// <returns></returns>
        public bool IsEndConditionMet(RaceConfig config, GameRunner.GameState state)
        {
            if (state.RaceEnded || state.RowerDatas.All(rowerEntry =>  FinishedRace(config, rowerEntry.Value)))
            {
                state.RaceEnded = true;
            }

            return state.RaceEnded;
        }


        /// <summary>
        /// Runs all logic for game simulation
        /// </summary>
        /// <returns>True if the simulation should continue</returns>
        public bool SimulationTick(RaceConfig config, GameRunner.GameState state, float deltaTime)
        {
            state.RaceTimeElapsed += deltaTime;

            SetNewTargetSpeed(config, state);
            AdjustRowerSpeeds(state, deltaTime);
            AdjustRowerPositions(state, deltaTime);
            AdjustScores(config, state, deltaTime);
            
            return !IsEndConditionMet(config, state);
        }

        public void AdjustRowerAcceleration(RaceConfig config, GameRunner.GameState state, GameRunner.RowerId id, bool up)
        {
            state.RowerDatas[id].SpeedUp = up;
        }

        public bool FinishedRace(RaceConfig config, GameRunner.RowerData rower)
        {
            return rower.SimPosition > config.RaceDistance;
        } 

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

        public void AdjustRowerSpeeds(GameRunner.GameState state, float deltaTime)
        {
            foreach (var rower in state.RowerDatas)
            {
                rower.Value.Speed += deltaTime * rower.Value.SpeedChangePerSecond * 
                                     (rower.Value.SpeedUp ? 1 : -1);
                Debug.Log($"{rower.Key} {rower.Value.Speed} {rower.Value.SpeedUp} {rower.Value.SpeedChangePerSecond}");
                rower.Value.Speed = Mathf.Clamp(rower.Value.Speed, rower.Value.MinSpeed, rower.Value.MaxSpeed);
            }
        }
        
        public void AdjustRowerPositions(GameRunner.GameState state, float deltaTime)
        {
            foreach (var rower in state.RowerDatas)
            {
                rower.Value.SimPosition += deltaTime * rower.Value.Speed;
            }
        }

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