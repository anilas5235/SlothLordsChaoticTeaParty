using System;
using System.Collections.Generic;
using Project.Scripts.General;
using UnityEngine;

namespace Project.Scripts.Tiles
{
    public class Tile : MonoBehaviour, ITile
    {
        private SpriteRenderer myItem,myBackground;

        public TileType myTileType;
        [SerializeField] private Vector2Int positionInGrid;
        [SerializeField] private Vector3 positionInScene, dragVector;
        
        
        private Tile PreviewDragedTile;
        private Camera _camera;
        private TileManager myTileManager;

        private bool currentlyDraged = false;
        
        private const float DragRange = 2.2f, MoveThreshold = .5f;

        public enum TileType
        {
            Type0 = 0,
            Type1 = 1,
            Type2 = 2,
            Type3 = 3,
            Type4 = 4,
            Type5 = 5,
        }

        private void Awake()
        {
            myBackground = transform.GetChild(0).GetComponent<SpriteRenderer>();
            myItem = transform.GetChild(1).GetComponent<SpriteRenderer>();
            _camera = Camera.main;
            myTileManager = TileManager.instance;
        }

        public TileType GetTileType()
        {
            return myTileType;
        }

        public Tile[] GetNeighbours()
        {
            List<Tile> neighbours = new List<Tile>();
            int ownX = positionInGrid.x, ownY = positionInGrid.y;
            neighbours.Add(myTileManager.GetTile(new Vector2Int(ownX + 1, ownY)));
            neighbours.Add(myTileManager.GetTile(new Vector2Int(ownX - 1, ownY)));
            neighbours.Add(myTileManager.GetTile(new Vector2Int(ownX, ownY + 1)));
            neighbours.Add(myTileManager.GetTile(new Vector2Int(ownX, ownY - 1)));

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
            return myTileManager.GetTile(positionInGrid + offset);
        }

        public Vector2Int GetTilePosition()
        {
            return positionInGrid;
        }

        private void ChangeTileType(TileType newTileType)
        {
            myTileType = newTileType;
            myItem.sprite = TileRecourseKeeper.instance.tileSprites[(int)myTileType];
        }

        private void OnMouseDown()
        {
            if (myTileManager.EditMode)
            {
                int id = (int)myTileType + 1;
                if (id > 5) id = 0;
                ChangeTileType((TileType) id);
            }
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
            if(myTileManager.EditMode) return;
            if (!myTileManager.interactable)return;
            if (!currentlyDraged)DragStateChanged();
            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            dragVector = mousePosition - positionInScene;
            
            if (dragVector.magnitude > DragRange) dragVector = dragVector.normalized * DragRange;
            if (Mathf.Abs(dragVector.x) > Mathf.Abs(dragVector.y)) dragVector.y = 0;
            else dragVector.x = 0;
            
            transform.localPosition = positionInScene + dragVector;
            MovePreview();
        }

        private void MovePreview()
        {
            if (Mathf.Abs(dragVector.x) < MoveThreshold && Mathf.Abs(dragVector.y) < MoveThreshold)
            {
                if (PreviewDragedTile) PreviewDragedTile.transform.localPosition = PreviewDragedTile.positionInScene;
                PreviewDragedTile = null;
                return;
            }
            Vector2Int tileOffset = Vector2Int.zero;
            if (Mathf.Abs( dragVector.x) > Mathf.Abs(dragVector.y)) tileOffset.x = dragVector.x > 0 ? 1 : -1;
            else tileOffset.y = dragVector.y > 0 ? -1 : 1;

            if (!myTileManager.IsPositionInGrid(positionInGrid + tileOffset)) return;

            Tile currentTile = myTileManager.GetTile(positionInGrid + tileOffset);
            if (PreviewDragedTile != currentTile)
            {
                if (PreviewDragedTile) PreviewDragedTile.transform.localPosition = PreviewDragedTile.positionInScene;
                PreviewDragedTile = currentTile;
            }
            PreviewDragedTile.transform.localPosition = positionInScene;
        }

        private void OnMouseUp()
        {
            if(myTileManager.EditMode) return;
            if (currentlyDraged)
            {
                Move();
                DragStateChanged();
            }
        }

        private void DragStateChanged()
        {
            currentlyDraged = !currentlyDraged;
            CursorManager.instance.ChangeCursor(currentlyDraged
                ? CursorManager.Cursors.ClosedHand
                : CursorManager.Cursors.OpenHand);
            myBackground.sortingOrder = currentlyDraged ? 2 : 0;
            myItem.sortingOrder = currentlyDraged ? 3 : 1;
        }

        public void SetNewPosition(Vector2Int newPos)
        {
            positionInGrid = newPos;
            positionInScene = myTileManager.fieldGridPositions[positionInGrid.x][positionInGrid.y];
            transform.localPosition = positionInScene;
        }

        private void Move()
        {
            if (Mathf.Abs(dragVector.x) < MoveThreshold && Mathf.Abs(dragVector.y) < MoveThreshold)
            {
                transform.localPosition = positionInScene;
                return;
            }
            Vector2Int tileOffset = Vector2Int.zero;
            if (Mathf.Abs( dragVector.x) > Mathf.Abs(dragVector.y)) tileOffset.x = dragVector.x > 0 ? 1 : -1;
            else tileOffset.y = dragVector.y > 0 ? -1 : 1;

            if (!myTileManager.IsPositionInGrid(positionInGrid+tileOffset))
            {
                transform.localPosition = positionInScene;
                return;
            }
            
            myTileManager.SwitchTiles(positionInGrid,positionInGrid+tileOffset);
            myTileManager.Turns--;
        }
    }
}
