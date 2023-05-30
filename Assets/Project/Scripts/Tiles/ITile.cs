using UnityEngine;

namespace Project.Scripts.Tiles
{
    public interface ITile
    {
        public Tile.TileType GetTileType();
        public Tile[] GetNeighbours();
        public Tile GetNeighbour(Vector2Int offset);
        public Vector2Int GetTilePosition();
    }
}
