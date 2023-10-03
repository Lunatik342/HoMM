using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.BattleArena.StaticData
{
    [CreateAssetMenu(fileName = "ObstaclesGenerationRules", menuName = "StaticData/BattleArena/ObstaclesGenerationRules")]
    public class ObstaclesGenerationStaticData: ScriptableObject
    {
        [field: InfoBox("Weights for determining the random amount of obstacles on the battle arena.")] 
        [field: SerializeField] public IntIntSerializableDictionary ObstaclesCountWeights { get; private set; }
        
        [field: InfoBox("Weights for determining the random size of an obstacle on the battle arena.")]
        [field: SerializeField] public IntIntSerializableDictionary ObstaclesSizeWeights { get; private set; }
        [field: SerializeField] public int MaximumOccupiedSpaceByObstacles { get; private set; }
        [field: SerializeField] public int MaxTriesToPlaceObstacleBeforeGivingUp { get; private set; }
    }
    
    [Serializable]
    public class IntIntSerializableDictionary: SerializableDictionary<int, int> { }
}