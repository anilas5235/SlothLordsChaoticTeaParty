using System;
using UnityEngine;

namespace Project.Scripts.Tiles
{
    [CreateAssetMenu]
    public class Level : ScriptableObject
    {
        public void LevelDataSet(int[][] tileStartingGrid, float[] spawnProbabilities,int allowedTurns, Tile.TileType likedTileType, Tile.TileType dislikedTileType)
        {
            fieldSize = new Vector2Int(tileStartingGrid.Length, tileStartingGrid[0].Length);
            startingGrid = new Colum[tileStartingGrid.Length];

            for (int i = 0; i < tileStartingGrid.Length; i++)
            {
                startingGrid[i] = new Colum
                {
                    data = new int[fieldSize.y]
                };
            }

            for (int i = 0; i < fieldSize.y; i++)
            {
                for (int j = 0; j < fieldSize.x; j++)
                {
                    startingGrid[j].data[i] = tileStartingGrid[j][i];
                }
            }

            probabilities = spawnProbabilities;
            turns = allowedTurns;
            preferredTile = likedTileType;
            dislikedTile = dislikedTileType;
        }

        public void LevelDataSet(Vector2Int tileFieldSize, float[] spawnProbabilities,int allowedTurns, Tile.TileType likedTileType, Tile.TileType dislikedTileType)
        {
            probabilities = spawnProbabilities;
            fieldSize = tileFieldSize;
            random = true;
            turns = allowedTurns;
            preferredTile = likedTileType;
            dislikedTile = dislikedTileType;
        }

        [SerializeField] private Colum[] startingGrid; 
        [SerializeField] private Vector2Int fieldSize;
        [SerializeField] private Tile.TileType preferredTile, dislikedTile;
        [SerializeField] private float[] probabilities;
        [SerializeField] private int turns;
        [SerializeField] private bool random = false;
        
        public Colum[] StartingGrid { get => startingGrid; }
        public Vector2Int FieldSize{ get => fieldSize; }
        public Tile.TileType PreferredTile{ get => preferredTile; }
        public Tile.TileType DislikedTile{ get => dislikedTile; }
        public float[] Probabilities { get => probabilities; }
        public int Turns { get => turns; }
        public bool Random { get => random; }
    }

    [Serializable]
    public class Colum
    {
        public int[] data;
    }
}