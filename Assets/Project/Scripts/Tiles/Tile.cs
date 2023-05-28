using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Tiles
{
    public class Tile : MonoBehaviour, ITile
    {
        private SpriteRenderer myItem;
        private BoxCollider2D _myBoxCollider2D;

        private TileType myTileType;
        private Vector2Int positionInGrid;

        public enum TileType
        {
            Type0 = 0,
            Type1 = 1,
            Type2 = 2,
            Type3 = 3,
            Type4 = 4,
            Type5 = 5,
            Type6 = 6,
        }

        private void Awake()
        {
            _myBoxCollider2D = GetComponentInChildren<BoxCollider2D>();
            myItem = transform.GetChild(1).GetComponent<SpriteRenderer>();
        }

        public TileType GetTileType()
        {
            return myTileType;
        }

        public Tile[] GetNeighbours()
        {
            List<Tile> neighbours = new List<Tile>();
            int ownX = positionInGrid.x, ownY = positionInGrid.y;
            neighbours.Add( TileManager.instance.GetTile( ownX+1,ownY));
            neighbours.Add( TileManager.instance.GetTile( ownX-1,ownY));
            neighbours.Add( TileManager.instance.GetTile( ownX,ownY+1));
            neighbours.Add( TileManager.instance.GetTile( ownX,ownY-1));

            for (int i = 0; i < neighbours.Count; i++)
            {
                if (!neighbours[i])
                {
                    neighbours.RemoveAt(i);
                    i--;
                }
            }

            return neighbours.ToArray();
        }

        public Vector2 GetTilePosition()
        {
            return positionInGrid;
        }

        public void InitializeTile(TileType type, Vector2Int positionInGird)
        {
            myTileType = type;
            positionInGrid = positionInGird;
            myItem.sprite = TileRecourseKeeper.instance.tileSprites[(int)myTileType];
        }
    }
}
