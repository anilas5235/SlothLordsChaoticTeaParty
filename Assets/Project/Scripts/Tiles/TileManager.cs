using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.General;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Tiles
{
    public class TileManager : Singleton<TileManager>
    {
        [SerializeField] private GameObject tilePreFap;
        public int fieldSizeX = 10,fieldSizeY = 7;
        [SerializeField] private float tileSize =2f, tileSpacing = 0.2f;
        public Vector3[][] fieldGridPositions;
        public Tile[][] fieldGridTiles;

        private int score;

        public int Score
        {
            get => score;
            set
            {
                score = value;
                UIManager.instance.UpdateScore();
            }
        }

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
                    NewTile(new Vector2Int(j,i), GetTileType(counter));
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

        private Tile.TileType GetTileType(int i)
        {
            switch (i)
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
            List<Tile> ComboTiles = new List<Tile>();
            Comp(origin);
            
            if(ComboTiles.Count < 3) return;
            foreach (var t in ComboTiles)
            {
                Vector2Int pos = t.GetTilePosition();
                fieldGridTiles[pos.x][pos.y] = null;
                Score += (int) Mathf.Pow(2f, ComboTiles.Count);
                Destroy(t.gameObject);
            }

            void Comp(Vector2Int localOrigin)
            {
                foreach (Tile neighbour in GetTile(localOrigin).GetNeighbours())
                {
                    if (neighbour.GetTileType() == tileType)
                    {
                        if(ComboTiles.Contains(neighbour)) continue;
                        ComboTiles.Add(neighbour);
                        Comp(neighbour.GetTilePosition());
                    }
                }
            }
        }
        
        private void  CheckForComboAfterFall()
        {
            List<Tile> checkedTiles = new List<Tile>();

            for (int i = 0; i < fieldSizeY - 1; i++)
            {
                for (int j = 0; j < fieldSizeX; j++)
                {
                    if (!checkedTiles.Contains(fieldGridTiles[j][i]))
                    {
                        Comp(fieldGridTiles[j][i].GetTilePosition(),checkedTiles,fieldGridTiles[j][i].GetTileType());
                    }
                }
            }

            foreach (var t in checkedTiles)
            {
                Vector2Int pos = t.GetTilePosition();
                fieldGridTiles[pos.x][pos.y] = null;
                Score += (int) Mathf.Pow(2f, checkedTiles.Count);
                Destroy(t.gameObject);
            }


            void Comp(Vector2Int localOrigin,List<Tile> checkedTiles, Tile.TileType tileType)
            {
                foreach (Tile neighbour in GetTile(localOrigin).GetNeighbours())
                {
                    if (neighbour.GetTileType() == tileType)
                    {
                        if(checkedTiles.Contains(neighbour)) continue;
                        checkedTiles.Add(neighbour);
                        Comp(neighbour.GetTilePosition(),checkedTiles,tileType);
                    }
                }
            }
        }

        private bool CheckForFall()
        {
            bool doneFalling = true;
            for (int i = 0; i < fieldSizeY-1; i++)
            {
                for (int j = 0; j < fieldSizeX; j++)
                {
                    if(i==0 && fieldGridTiles[j][i] == null)
                    {
                        NewTile(new Vector2Int(j,i));
                        doneFalling = false;
                    }
                    if (fieldGridTiles[j][i] != null && fieldGridTiles[j][i+1] == null)
                    {
                        Tile currentTile = fieldGridTiles[j][i];
                        currentTile.SetNewPosition(new Vector2Int(j,i+1));
                        fieldGridTiles[j][i + 1] = currentTile;
                        fieldGridTiles[j][i] = null;
                        doneFalling = false;
                    }
                }
            }
            return doneFalling;
            CheckForComboAfterFall();
        }

        private IEnumerator Falling()
        {
            interactable = false;
            do
            {
                yield return new WaitForSeconds(.5f);
            } while (!CheckForFall());

            interactable = true;
        }

        private void NewTile(Vector2Int posInGrid)
        {
            Vector3 currentPosition = fieldGridPositions[posInGrid.x][posInGrid.y];
            Tile tile = Instantiate(tilePreFap, currentPosition, quaternion.identity).GetComponent<Tile>();
            tile.gameObject.transform.SetParent(transform);
            tile.InitializeTile(GetTileType(Random.Range(0,4)),posInGrid,currentPosition);
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
    }
}
