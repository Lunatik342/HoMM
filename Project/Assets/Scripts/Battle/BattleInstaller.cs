using System;
using System.Collections.Generic;
using Battle.BattleArena;
using Battle.BattleArena.Pathfinding.StaticData;
using Battle.BattleArena.StaticData;
using Battle.BattleFlow;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Battle
{
    public class BattleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var unitsToSpawn = new Dictionary<Team, List<UnitCreationParameter>>()
            {
                {
                    Team.TeamLeft, new List<UnitCreationParameter>
                    {
                        new UnitCreationParameter(new Vector2Int(0, 0), UnitId.Blank, 5),
                        new UnitCreationParameter(new Vector2Int(1, 4), UnitId.Blank, 15),
                        new UnitCreationParameter(new Vector2Int(1, 9), UnitId.Blank, 10),
                    }
                },
                {
                    Team.TeamRight, new List<UnitCreationParameter>
                    {
                        new UnitCreationParameter(new Vector2Int(10, 2), UnitId.Blank, 33),
                        new UnitCreationParameter(new Vector2Int(11, 6), UnitId.Blank, 62),
                        new UnitCreationParameter(new Vector2Int(10, 8), UnitId.Blank, 12),
                    }
                },
            };
            
            var testBattleStartParameters = new BattleStartParameters(BattleArenaId.Blank, new ObstacleGenerationParameters()
            {
                IsRandom = true,
                RandomSeed = Random.Range(0, Int32.MaxValue),
                DeterminedObstacleParameters = new List<ObstacleOnGridParameters>()
                {
                    new(ObstacleId.Blank1, ObstaclesSpawner.ObstacleRotationAngle.Degrees0, new Vector2Int(5, 5))
                },
                
            }, unitsToSpawn, new Dictionary<Team, CommandProviderType>
            {
                { Team.TeamLeft , CommandProviderType.PlayerControlled },
                { Team.TeamRight , CommandProviderType.AIControlled },
            });
            
            Container.Bind<BattleStartParameters>().FromInstance(testBattleStartParameters).AsSingle();
            Container.BindInterfacesAndSelfTo<BattleStarter>().AsSingle().NonLazy();
        }
    }
}
