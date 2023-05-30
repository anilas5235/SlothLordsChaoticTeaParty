using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Tiles
{
    public class Tile : MonoBehaviour, ITile
    {
        private SpriteRenderer myItem,myBackground;
        private BoxCollider2D _myBoxCollider2D;

        private TileType myTileType;
        [SerializeField] private Vector2Int positionInGrid;
        [SerializeField] private Vector3 positionInScene, dragVector;
        private Camera _camera;

        private bool currentlyDraged = false;
        
        private const float DragRange = 1.5f;

        public enum TileType
        {
            Type0 = 0,
            Type1 = 1,
            Type2 = 2,
            Type3 = 3,
            Type4 = 4,
        }

        private void Awake()
        {
            _myBoxCollider2D = GetComponentInChildren<BoxCollider2D>();
            myBackground = transform.GetChild(0).GetComponent<SpriteRenderer>();
            myItem = transform.GetChild(1).GetComponent<SpriteRenderer>();
            _camera = Camera.main;
        }

        public TileType GetTileType()
        {
            return myTileType;
        }

        public Tile[] GetNeighbours()
        {
            List<Tile> neighbours = new List<Tile>();
            int ownX = positionInGrid.x, ownY = positionInGrid.y;
            neighbours.Add(TileManager.instance.GetTile(new Vector2Int(ownX + 1, ownY)));
            neighbours.Add(TileManager.instance.GetTile(new Vector2Int(ownX - 1, ownY)));
            neighbours.Add(TileManager.instance.GetTile(new Vector2Int(ownX, ownY + 1)));
            neighbours.Add(TileManager.instance.GetTile(new Vector2Int(ownX, ownY - 1)));

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

        public Tile GetNeighbour(Vector2Int offset)
        {
            return TileManager.instance.GetTile(positionInGrid + offset);
        }

        public Vector2Int GetTilePosition()
        {
            return positionInGrid;
        }

        public void InitializeTile(TileType type, Vector2Int positionInGird, Vector3 localPos)
        {
            myTileType = type;
            positionInGrid = positionInGird;
            myItem.sprite = TileRecourseKeeper.instance.tileSprites[(int)myTileType];
            positionInScene = localPos;
            transform.localPosition = positionInScene;
        }

        private void OnMouseDrag()
        {
            if (!TileManager.instance.interactable)return;
            if (!currentlyDraged)DragStateChanged();
            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            if (Vector3.Distance(positionInScene, mousePosition) < DragRange) transform.position = mousePosition;
            else transform.localPosition = positionInScene + (mousePosition - positionInScene).normalized;
            dragVector = transform.position - positionInScene;
        }

        private void OnMouseUp()
        {
            if (currentlyDraged)
            {
                Move();
                DragStateChanged();
            }
        }

        private void DragStateChanged()
        {
            currentlyDraged = !currentlyDraged;
            myBackground.sortingOrder = currentlyDraged ? 2 : 0;
            myItem.sortingOrder = currentlyDraged ? 3 : 1;
        }

        public void SetNewPosition(Vector2Int newPos)
        {
            positionInGrid = newPos;
            positionInScene = TileManager.instance.fieldGridPositions[positionInGrid.x][positionInGrid.y];
            transform.localPosition = positionInScene;
        }

        private void Move()
        {
            if (Mathf.Abs(dragVector.x) < 0.2f&&Mathf.Abs(dragVector.y) < 0.2f)
            {
                transform.position = positionInScene;
                return;
            }
            Vector2Int tileOffset = Vector2Int.zero;
            if (Mathf.Abs( dragVector.x) > Mathf.Abs(dragVector.y)) tileOffset.x = dragVector.x > 0 ? 1 : -1;
            else tileOffset.y = dragVector.y > 0 ? -1 : 1;
            
            TileManager.instance.SwitchTiles(positionInGrid,positionInGrid+tileOffset);
        }
    }
}
