using UnityEngine;

namespace Project.Scripts.Tiles
{
    public interface ITile
    {
        public Tile.TileType GetTileType();
        public Tile[] GetNeighbours();
        public Vector2 GetTilePosition();
    }
}
