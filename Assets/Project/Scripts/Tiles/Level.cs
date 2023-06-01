using System;
using UnityEngine;

namespace Project.Scripts.Tiles
{
    [CreateAssetMenu]
    public class Level : ScriptableObject
    {
        public void LevelDataSet(int[][] startingGrid, float[] probabilities)
        {
            FieldSize = new Vector2Int(startingGrid.Length, startingGrid[0].Length);
            StartingGrid = new Colum[startingGrid.Length];

            for (int i = 0; i < startingGrid.Length; i++)
            {
                StartingGrid[i] = new Colum
                {
                    data = new int[FieldSize.y]
                };
            }

            for (int i = 0; i < FieldSize.y; i++)
            {
                for (int j = 0; j < FieldSize.x; j++)
                {
                    StartingGrid[j].data[i] = startingGrid[j][i];
                }
            }

            Probabilities = probabilities;
           
        }

        public void LevelDataSet(float[] probabilities, Vector2Int fieldSize)
        {
            Probabilities = probabilities;
            FieldSize = fieldSize;
            Random = true;
        }

        public Vector2Int FieldSize; //{ get; private set; }
        public Colum[] StartingGrid; //{ get; private set; }
        public float[] Probabilities; // { get; private set; }

        public bool Random = false; //{ get; private set; } = false;
    }

    [Serializable]
    public class Colum
    {
        public int[] data;
    }
}