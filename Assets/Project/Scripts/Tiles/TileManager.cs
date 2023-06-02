using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Project.Scripts.General;
using Project.Scripts.UIScripts;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Tiles
{
    public class TileManager : Singleton<TileManager>
    {
        private GameObject tilePreFap;
        [SerializeField] private Level levelData;
        
        public Vector2Int fieldSize = new Vector2Int( 10, 7);
        public Vector3[][] fieldGridPositions;
        private Tile[][] fieldGridTiles;
        public int score, comboRoll, turns = 20;
        public float[] probabilities = new[] { 100 / 6f, 100 / 6f, 100 / 6f, 100 / 6f, 100 / 6f, 100 / 6f };
        
        [Header("EditMode")]
        public bool EditMode = false; 

        private const float tileSize =2f, tileSpacing = 0.2f;
        private const int minComboSize = 3;
        private const float stepTime = .3f;

        #region Properties

        public int Score
        {
            get => score;
            private set
            {
                score = value;
                UIManager.instance.UpdateScore();
            }
        }

        public int ComboRoll
        {
            get => comboRoll;
            private set
            {
                comboRoll = value;
                UIManager.instance.UpdateComboRoll();
            }
        }

        public int Turns
        {
            get => turns;
            set
            {
                turns = value;
                UIManager.instance.UpdateTurn();
            }
        }

        #endregion

        public bool interactable = true;

        protected override void Awake()
        {
            base.Awake();
            tilePreFap =  Resources.Load<GameObject>("Prefaps/Tiles/Tile1");
            
            CreateGrid();
        }

        private void InitializeGridFromData( Level data)
        {
            fieldSize = data.FieldSize;
            probabilities = data.Probabilities;
            
            fieldGridPositions = new Vector3[fieldSize.x][];
            fieldGridTiles = new Tile[fieldSize.x][];
            
            float x = -1* GetStartingCoordinates(fieldSize.x, tileSize, tileSpacing);
            float y = GetStartingCoordinates(fieldSize.y,tileSize,tileSpacing);

            Vector3 xAdd = new Vector3(tileSize + tileSpacing, 0, 0);
            Vector3 yAdd = new Vector3(0, tileSize + tileSpacing, 0);
            
            for (int i = 0; i < fieldSize.x; i++)
            {
                fieldGridPositions[i] = new Vector3[fieldSize.y];
                fieldGridTiles[i] = new Tile[fieldSize.y];
            }
            
            for (int i = 0; i < fieldSize.y; i++)
            {
                Vector3 currentPosition = new Vector3(x, y, 0) + -1* i * yAdd;
                for (int j = 0; j < fieldSize.x; j++)
                {
                    fieldGridPositions[j][i] = currentPosition;
                    NewTile(new Vector2Int(j,i), GetTileTypeFormIndex(data.StartingGrid[j].data[i]),fieldGridTiles);
                    currentPosition += xAdd;
                }
            }
        }

        public void CreateGrid()
        {
            if (fieldGridTiles == null)
            {
                if (levelData) InitializeGridFromData(levelData);
                else InitializeGridRandom();
            }
            else
            {
                Tile[][] newTileField = new Tile[fieldSize.x][];
                fieldGridPositions = new Vector3[fieldSize.x][];

                for (int i = 0; i < fieldSize.x; i++)
                {
                    newTileField[i] = new Tile[fieldSize.y];
                    fieldGridPositions[i]  = new Vector3[fieldSize.y];
                }
                float x = -1* GetStartingCoordinates(fieldSize.x, tileSize, tileSpacing);
                float y = GetStartingCoordinates(fieldSize.y,tileSize,tileSpacing);
                 
                Vector3 xAdd = new Vector3(tileSize + tileSpacing, 0, 0);
                Vector3 yAdd = new Vector3(0, tileSize + tileSpacing, 0);

                for (int i = 0; i < fieldSize.y; i++)
                {
                    Vector3 currentPosition = new Vector3(x, y, 0) + -1 * i * yAdd;
                    for (int j = 0; j < fieldSize.x; j++)
                    {
                        fieldGridPositions[j][i] = currentPosition;
                        
                        if (j < fieldGridTiles.Length)
                        {
                            if (i < fieldGridTiles[i].Length)
                            {
                                Tile current =  newTileField[j][i] = fieldGridTiles[j][i];
                                current.SetNewPosition(new Vector2Int(j,i));
                            }
                            else NewTile(new Vector2Int(j,i), Tile.TileType.Type0,newTileField);
                        }
                        else NewTile(new Vector2Int(j,i), Tile.TileType.Type0,newTileField);
                        
                        currentPosition += xAdd;
                    }
                }

                for (int i = 0; i < fieldGridTiles[0].Length; i++)
                {
                    for (int j = 0; j < fieldGridTiles.Length; j++)
                    {
                        if (i >= fieldSize.y || j >= fieldSize.x)
                        {
                            Destroy(fieldGridTiles[j][i].gameObject);
                        }
                    }
                }

                fieldGridTiles = newTileField;
            }
        }

        private void InitializeGridRandom()
        {
            int tileCountX = fieldSize.x, tileCountY = fieldSize.y;
            fieldGridPositions = new Vector3[tileCountX][];
            fieldGridTiles = new Tile[tileCountX][];

            float x = -1* GetStartingCoordinates(tileCountX, tileSize, tileSpacing);
            float y = GetStartingCoordinates(tileCountY,tileSize,tileSpacing);

            Vector3 xAdd = new Vector3(tileSize + tileSpacing, 0, 0);
            Vector3 yAdd = new Vector3(0, tileSize + tileSpacing, 0);

            int counter = 0;

            for (int i = 0; i < tileCountX; i++)
            {
                fieldGridPositions[i] = new Vector3[tileCountY];
                fieldGridTiles[i] = new Tile[tileCountY];
            }

            for (int i = 0; i < tileCountY; i++)
            {
                Vector3 currentPosition = new Vector3(x, y, 0) + -1* i * yAdd;
                for (int j = 0; j < tileCountX; j++)
                {
                    fieldGridPositions[j][i] = currentPosition;
                    NewTile(new Vector2Int(j,i), GetTileTypeFormIndex(counter),fieldGridTiles);
                    currentPosition += xAdd;
                    counter++;
                    if (counter > 4) counter = 0;
                }
            }
        }
        private float GetStartingCoordinates(int number,float size , float spacing )
        {
            return (number % 2 ==0) ? (number/2f - 0.5f)* (size+ spacing):
                Mathf.FloorToInt(number / 2f)* (size + spacing);
        }
        
        public Tile GetTile(Vector2Int tilePosInGrid)
        {
            if (IsPositionInGrid(tilePosInGrid)) return fieldGridTiles[tilePosInGrid.x][tilePosInGrid.y];
            return null;
        }

        public void SwitchTiles(Vector2Int tile1, Vector2Int tile2)
        {
            if(!IsPositionInGrid(tile1)|| !IsPositionInGrid(tile2)) return;
            GetTile(tile1).SetNewPosition(tile2);
            GetTile(tile2).SetNewPosition(tile1);
            (fieldGridTiles[tile1.x][tile1.y], fieldGridTiles[tile2.x][tile2.y]) =
                (fieldGridTiles[tile2.x][tile2.y], fieldGridTiles[tile1.x][tile1.y]);
            CheckForCombo(tile2,GetTile(tile2).GetTileType());
            CheckForCombo(tile1,GetTile(tile1).GetTileType());
            StartCoroutine(Falling());
        }
        
        public bool IsPositionInGrid(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < fieldSize.x && pos.y >= 0 && pos.y < fieldSize.y;
        }

        private Tile.TileType GetTileTypeFormIndex(int index)
        {
            switch (index)
            {
                case 0 : return Tile.TileType.Type0;
                case 1 : return Tile.TileType.Type1;
                case 2 : return Tile.TileType.Type2;
                case 3 : return Tile.TileType.Type3;
                case 4 : return Tile.TileType.Type4;
                case 5 : return Tile.TileType.Type5;
            }

            return Tile.TileType.Type0;
        }

        private void CheckForCombo(Vector2Int origin, Tile.TileType tileType)
        {
            List<Tile> comboTiles = new List<Tile> { fieldGridTiles[origin.x][origin.y] };
            Comp(origin);
            
            if(comboTiles.Count < minComboSize) return;
            ComboRoll++;
            foreach (var t in comboTiles)
            {
                Vector2Int pos = t.GetTilePosition();
                fieldGridTiles[pos.x][pos.y] = null;
                Destroy(t.gameObject);
            }
            ScoreCalculator( comboTiles.Count);

            void Comp(Vector2Int localOrigin)
            {
                foreach (Tile neighbour in GetTile(localOrigin).GetNeighbours())
                {
                    if (neighbour.GetTileType() == tileType)
                    {
                        if(comboTiles.Contains(neighbour)) continue;
                        comboTiles.Add(neighbour);
                        Comp(neighbour.GetTilePosition());
                    }
                }
            }
        }
        
        private void  CheckForAllCombos()
        {
            List<Tile> checkedTiles = new List<Tile>();
            List<Tile> toBeDeleteTiles = new List<Tile>();

            for (int i = 0; i < fieldSize.y - 1; i++)
            {
                for (int j = 0; j < fieldSize.x; j++)
                {
                    if (!checkedTiles.Contains(fieldGridTiles[j][i]))
                    {
                        List<Tile> combTiles = Comp(fieldGridTiles[j][i].GetTilePosition(), fieldGridTiles[j][i].GetTileType());
                        Debug.Log($"Combo Of Size {combTiles.Count} found");
                        if (combTiles.Count < 3) continue;
                        toBeDeleteTiles.AddRange(combTiles);
                        ComboRoll++;
                        ScoreCalculator(combTiles.Count);
                    }
                }
            }

            foreach (var t in toBeDeleteTiles)
            {
                Vector2Int pos = t.GetTilePosition();
                fieldGridTiles[pos.x][pos.y] = null;
                Destroy(t.gameObject);
            }

            if (toBeDeleteTiles.Count > 0) StartCoroutine(Falling());
            else
            {
                interactable = true;
                ComboRoll = 0;
            }

            List<Tile> Comp(Vector2Int localOrigin, Tile.TileType tileType)
            {
                List<Tile> currentCombo = new List<Tile>();
                if (!checkedTiles.Contains(fieldGridTiles[localOrigin.x][localOrigin.y]))
                {
                    currentCombo.Add(fieldGridTiles[localOrigin.x][localOrigin.y]);
                    checkedTiles.Add(fieldGridTiles[localOrigin.x][localOrigin.y]);
                }
                foreach (Tile neighbour in GetTile(localOrigin).GetNeighbours())
                {
                    if (neighbour.GetTileType() != tileType) continue;
                    if(checkedTiles.Contains(neighbour) || currentCombo.Contains(neighbour)) continue;
                    currentCombo.Add(neighbour);
                    checkedTiles.Add(neighbour);
                    currentCombo.AddRange( Comp(neighbour.GetTilePosition(),tileType));
                }
                return currentCombo;
            }
        }

        private bool CheckForEmptyTiles()
        {
            for (int i = 0; i < fieldSize.y; i++)
            {
                for (int j = 0; j < fieldSize.x; j++)
                {
                    if (fieldGridTiles[j][i] == null) return true;
                }
            }

            return false;
        }

        private bool CheckAndFall()
        {
            bool doneFalling = true;
            List<Vector2Int> toSkip = new List<Vector2Int>();
            for (int i = 0; i < fieldSize.y; i++)
            {
                for (int j = 0; j < fieldSize.x; j++)
                {
                    Tile currentTile = fieldGridTiles[j][i];

                    if (currentTile == null)
                    {
                        if (i == 0)
                        {
                            NewTile(new Vector2Int(j, i));
                        }
                        else if (fieldGridTiles[j][i - 1] != null && !toSkip.Contains(new Vector2Int(j, i)))
                        {
                            Tile aboveTile = fieldGridTiles[j][i - 1];
                            if (aboveTile.GetTileType() == Tile.TileType.Clear)
                            {
                                NewTile(new Vector2Int(j, i));
                            }
                            else
                            {
                                aboveTile.SetNewPosition(new Vector2Int(j, i));
                                fieldGridTiles[j][i] = aboveTile;
                                fieldGridTiles[j][i - 1] = null;
                                toSkip.Add(new Vector2Int(j, i +1));
                            }
                        }
                        doneFalling = false;
                    }
                }
            }
            return doneFalling;
        }

        private IEnumerator Falling()
        {
            interactable = false;
            do
            {
                yield return new WaitForSeconds(stepTime);
            } while (!CheckAndFall());

            interactable = true;
            CheckForAllCombos();
        }

        private Tile.TileType GenerateTileType()
        {
            float randomNumber = Random.Range(0f, 100f);
            float prob = probabilities[0];
            int step = 0;

            while (randomNumber > prob && step < probabilities.Length-1)
            {
                step++;
                prob += probabilities[step];
            }
            
            return (Tile.TileType)step;
        }

        private void NewTile(Vector2Int posInGrid)
        {
            Vector3 currentPosition = fieldGridPositions[posInGrid.x][posInGrid.y];
            Tile tile = Instantiate(tilePreFap, currentPosition, quaternion.identity).GetComponent<Tile>();
            tile.gameObject.transform.SetParent(transform);
            tile.InitializeTile( GenerateTileType(),posInGrid,currentPosition);
            Debug.Log($"{tile.GetTileType()}");
            fieldGridTiles[posInGrid.x][posInGrid.y] = tile;
        }
        
        private void NewTile(Vector2Int posInGrid, Tile.TileType tileType, Tile[][] tileGrid)
        {
            Vector3 currentPosition = fieldGridPositions[posInGrid.x][posInGrid.y];
            Tile tile = Instantiate(tilePreFap, currentPosition, quaternion.identity).GetComponent<Tile>();
            tile.gameObject.transform.SetParent(transform);
            tile.InitializeTile(tileType,posInGrid,currentPosition);
            tileGrid[posInGrid.x][posInGrid.y] = tile;
        }

        private void ScoreCalculator(int comboSize)
        {
            if (comboSize < minComboSize) return;
            Score += (int) Mathf.Pow(2, comboSize) * comboRoll; 
        }

        public void SaveCurrentGrid()
        {
            int[][] data = new int[fieldSize.x][];
            for (int i = 0; i < fieldSize.x; i++)
            {
                data[i] = new int[fieldSize.y];
            }

            for (int i = 0; i < fieldSize.y; i++)
            {
                for (int j = 0; j < fieldSize.x; j++)
                {
                    data[j][i] = (int)fieldGridTiles[j][i].GetTileType();
                }
            }

            float[] probs = probabilities;

            Level level = ScriptableObject.CreateInstance<Level>();
            level.LevelDataSet(data,probs);
#if UNITY_EDITOR
            int id = 0;
            string path; 
            do
            {
                path = $"Assets/Project/Resources/LevelData/NewLevel{id}.asset";
                id++;
            } while (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(),path)));

            UnityEditor.AssetDatabase.CreateAsset(level,path);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}
