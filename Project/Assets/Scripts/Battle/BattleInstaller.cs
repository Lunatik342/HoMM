using System;
using System.Collections.Generic;
using Battle.BattleArena;
using Battle.BattleArena.StaticData;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Battle
{
    public class BattleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var testBattleStartParameters = new BattleStartParameters(BattleArenaId.Blank, new ObstacleGenerationParameters()
            {
                IsRandom = true,
                RandomSeed = Random.Range(0, Int32.MaxValue),
                DeterminedObstacleParameters = new List<DeterminedObstacleParameters>()
                {
                    new(ObstacleId.Blank1, ObstaclesSpawner.ObstacleRotationAngle.Degrees0, new Vector2Int(5, 5))
                }
            });
            
            Container.Bind<BattleStartParameters>().FromInstance(testBattleStartParameters).AsSingle();
            Container.BindInterfacesAndSelfTo<BattleStarter>().AsSingle().NonLazy();
        }
    }
}
