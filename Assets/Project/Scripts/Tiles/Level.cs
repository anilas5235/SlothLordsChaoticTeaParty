using System;
using Project.Scripts.DialogScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scripts.Tiles
{
    [CreateAssetMenu]
    public class Level : ScriptableObject
    {
        public void LevelDataSet(int[][] tileStartingGrid, float[] spawnProbabilities, int allowedTurns,
            Tile.TileType likedTileType, Tile.TileType dislikedTileType, int perfectScore, CharacterAnimator.Characters levelCharacter)
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
            prefectScore = perfectScore;
            character = levelCharacter;
        }

        [SerializeField] private Colum[] startingGrid; 
        [SerializeField] private Vector2Int fieldSize;
        [SerializeField] private Tile.TileType preferredTile, dislikedTile;
        [SerializeField] private float[] probabilities;
        [SerializeField] private int turns;
        [SerializeField] private bool random = false;
        [SerializeField] private int prefectScore;
        [SerializeField] private CharacterAnimator.Characters character;
        
        public Colum[] StartingGrid { get => startingGrid; }
        public Vector2Int FieldSize{ get => fieldSize; }
        public Tile.TileType PreferredTile{ get => preferredTile; }
        public Tile.TileType DislikedTile{ get => dislikedTile; }
        public float[] Probabilities { get => probabilities; }
        public int Turns { get => turns; }
        public bool Random { get => random; }
        public int PerfectScore { get => prefectScore; }
        public CharacterAnimator.Characters Character { get => character; }
    }

    [Serializable]
    public class Colum
    {
        public int[] data;
    }
}