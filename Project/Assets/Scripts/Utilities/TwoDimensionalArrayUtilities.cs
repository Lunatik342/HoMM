using Battle.BattleArena;
using UnityEngine;

namespace Utilities
{
    public static class TwoDimensionalArrayUtilities 
    {
        public static Vector3 GetWorldPositionFor(Vector2 gridPosition, int xSize, int ySize)
        {
            var centerPosition = new Vector2(
                (2 * gridPosition.x + xSize - 1) / 2f,
                (2 * gridPosition.y + ySize - 1) / 2f);

            return centerPosition.ToBattleArenaWorldPosition();
        }
        
        public static T[,] TurnBy90Degrees<T>(this T[,] source)
        {
            return source.ReverseEachRow().Transpose();
        }

        //Not optimal but good enough
        public static T[,] TurnBy180Degrees<T>(this T[,] source)
        {
            return source.TurnBy90Degrees().TurnBy90Degrees();
        }

        public static T[,] TurnBy270Degrees<T>(this T[,] source)
        {
            return source.Transpose().ReverseEachRow();
        }

        private static T[,] Transpose<T>(this T[,] source)
        {
            var xLenght = source.GetLength(0);
            var yLenght = source.GetLength(1);
            
            var newArray = new T[yLenght, xLenght];

            for (int i = 0; i < xLenght; i++)
            {
                for (int j = 0; j < yLenght; j++)
                {
                    newArray[j, i] = source[i, j];
                }
            }

            return newArray;
        }

        //Not optimal but good enough
        private static T[,] ReverseEachRow<T>(this T[,] source)
        {
            var xLenght = source.GetLength(0);
            var yLenght = source.GetLength(1);
            
            var newArray = new T[xLenght, yLenght];

            for (int i = 0; i < xLenght; i++)
            {
                for (int j = 0; j < yLenght; j++)
                {
                    newArray[i, j] = source[i, (yLenght - 1) - j];
                }
            }

            return newArray;
        }
    }
}