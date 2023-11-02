using System;
using System.Collections.Generic;
using Battle.Arena.Obstacles;
using Battle.Arena.StaticData;
using Battle.BattleFlow;
using Battle.BattleFlow.Phases;
using Battle.UnitCommands.Providers;
using Battle.Units.StaticData;
using Infrastructure.SimpleStateMachine;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Battle
{
    public class BattleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var testBattleStartParameters = CreateBattleStartParametersTEMPORARY();

            Container.Bind<BattleStartParameters>().FromInstance(testBattleStartParameters).AsSingle().WhenInjectedInto<BattleBootstrapper>();
            Container.BindInterfacesAndSelfTo<BattleBootstrapper>().AsSingle().NonLazy();
            Container.Bind<BattlePhasesStateMachine>().AsSingle();
            Container.Bind<StatesFactory>().AsSingle();
        }

        private static BattleStartParameters CreateBattleStartParametersTEMPORARY()
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
                { Team.TeamLeft, CommandProviderType.PlayerControlled },
                { Team.TeamRight, CommandProviderType.PlayerControlled },
            });
            return testBattleStartParameters;
        }
    }
}
