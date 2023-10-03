using Array2DEditor;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Battle.BattleArena.StaticData
{
    [CreateAssetMenu(fileName = "Obstacle", menuName = "StaticData/BattleArena/Obstacle")]
    public class ObstacleStaticData : SerializedScriptableObject
    {
        [field: SerializeField] public ObstacleId Id { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject ViewPrefabReference { get; private set; }
        
        [field: Space(20), InfoBox("Checked cells are occupied by obstacle")] 
        [field: SerializeField] private Array2DBool Figure { get; set; }

        public bool[,] GetLayout() => Figure.GetCells();

        public int GetOccupiedCellsCount()
        {
            int result = 0;
            var layout = GetLayout();
            
            for (int i = 0; i < layout.GetLength(0); i++)
            {
                for (int j = 0; j < layout.GetLength(1); j++)
                {
                    if (layout[i, j])
                    {
                        result++;
                    }
                }
            }

            return result;
        }
    }
}
