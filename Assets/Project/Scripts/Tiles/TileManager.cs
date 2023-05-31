using System.Collections;
using System.Collections.Generic;
using Project.Scripts.General;
using Project.Scripts.UIScripts;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Tiles
{
    public class TileManager : Singleton<TileManager>
    {
        [SerializeField] private GameObject tilePreFap;
        
        public int fieldSizeX = 10,fieldSizeY = 7;
        public Vector3[][] fieldGridPositions;
        public Tile[][] fieldGridTiles;
        public int score, comboRoll, turns = 20;

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
            InitializeGrid(fieldSizeX,fieldSizeY);
        }

        private void InitializeGrid(int tileCountX, int tileCountY)
        {
            fieldGridPositions = new Vector3[tileCountX][];
            fieldGridTiles = new Tile[tileCountX][];

            float x = -1* GetStartingValue(tileCountX, tileSize, tileSpacing);
            float y = GetStartingValue(tileCountY,tileSize,tileSpacing);

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
                    NewTile(new Vector2Int(j,i), GetTileTypeFormIndex(counter));
                    currentPosition += xAdd;
                    counter++;
                    if (counter > 4) counter = 0;
                }
            }

            float GetStartingValue(int number,float size , float spacing )
            {
                return (number % 2 ==0) ? (number/2f - 0.5f)* (size+ spacing):
                    Mathf.FloorToInt(number / 2f)* (size + spacing);
            }
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
            return pos.x >= 0 && pos.x < fieldSizeX && pos.y >= 0 && pos.y < fieldSizeY;
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
            }

            return Tile.TileType.Type0;
        }

        private void CheckForCombo(Vector2Int origin, Tile.TileType tileType)
        {
            List<Tile> comboTiles = new List<Tile>();
            comboTiles.Add(fieldGridTiles[origin.x][origin.y]);
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

            for (int i = 0; i < fieldSizeY - 1; i++)
            {
                for (int j = 0; j < fieldSizeX; j++)
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
            for (int i = 0; i < fieldSizeY; i++)
            {
                for (int j = 0; j < fieldSizeX; j++)
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
            for (int i = 0; i < fieldSizeY-1; i++)
            {
                for (int j = 0; j < fieldSizeX; j++)
                {
                    if(i==0 && fieldGridTiles[j][i] == null)
                    {
                        NewTile(new Vector2Int(j,i));
                        doneFalling = false;
                        continue;
                    }
                    if (fieldGridTiles[j][i] != null && fieldGridTiles[j][i+1] == null && !toSkip.Contains(new Vector2Int(j,i)))
                    {
                        Tile currentTile = fieldGridTiles[j][i];
                        currentTile.SetNewPosition(new Vector2Int(j,i+1));
                        fieldGridTiles[j][i + 1] = currentTile;
                        fieldGridTiles[j][i] = null;
                        toSkip.Add(new Vector2Int(j,i+1));
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

        private void NewTile(Vector2Int posInGrid)
        {
            Vector3 currentPosition = fieldGridPositions[posInGrid.x][posInGrid.y];
            Tile tile = Instantiate(tilePreFap, currentPosition, quaternion.identity).GetComponent<Tile>();
            tile.gameObject.transform.SetParent(transform);
            tile.InitializeTile(GetTileTypeFormIndex(Random.Range(0,4)),posInGrid,currentPosition);
            fieldGridTiles[posInGrid.x][posInGrid.y] = tile;
        }
        
        private void NewTile(Vector2Int posInGrid, Tile.TileType tileType)
        {
            Vector3 currentPosition = fieldGridPositions[posInGrid.x][posInGrid.y];
            Tile tile = Instantiate(tilePreFap, currentPosition, quaternion.identity).GetComponent<Tile>();
            tile.gameObject.transform.SetParent(transform);
            tile.InitializeTile(tileType,posInGrid,currentPosition);
            fieldGridTiles[posInGrid.x][posInGrid.y] = tile;
        }

        private void ScoreCalculator(int comboSize)
        {
            if (comboSize < minComboSize) return;
            Score += (int) Mathf.Pow(2, comboSize) * comboRoll; 
        }
    }
}
