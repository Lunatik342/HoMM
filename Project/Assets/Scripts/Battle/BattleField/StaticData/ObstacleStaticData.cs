using System;
using Array2DEditor;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "ObstacleStaticData", menuName = "StaticData/Obstacle")]
public class ObstacleStaticData : SerializedScriptableObject
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private Array2DBool _figure;
    
    public AssetReferenceGameObject ViewPrefabReference;

    public Vector2Int Size => _figure.GridSize;

    public bool[,] GetObstacleFigure()
    {
        return FlipArray(_figure.GetCells());
    }

    //Need to y flip array because Array2DBool [0,0] is in the top left corner, game grid array [0,0] is in the bottom left corner
    //So it would look the same in the editor and in the game
    private bool[,] FlipArray(bool[,] arrayToFlip)
    {
        int rows = arrayToFlip.GetLength(0);
        int columns = arrayToFlip.GetLength(1);
        bool[,] flippedArray = new bool[rows, columns];

        for (int i = 0 ; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                flippedArray[i, j] = arrayToFlip[(rows -1) - i, j];
            }
        }
        return flippedArray;
    }
}
