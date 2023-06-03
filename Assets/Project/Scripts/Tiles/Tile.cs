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
        private TileFieldManager myTileFieldManager;

        private bool currentlyDraged = false;
        
        private const float DragRange = 2.2f, MoveThreshold = .5f;

        public enum TileType
        {
            Clear = -1,
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
            myTileFieldManager = TileFieldManager.instance;
        }

        #region TileInfoFunctions

        public TileType GetTileType()
        {
            return myTileType;
        }

        public Tile[] GetNeighbours()
        {
            List<Tile> neighbours = new List<Tile>();
            int ownX = positionInGrid.x, ownY = positionInGrid.y;
            neighbours.Add(myTileFieldManager.GetTile(new Vector2Int(ownX + 1, ownY)));
            neighbours.Add(myTileFieldManager.GetTile(new Vector2Int(ownX - 1, ownY)));
            neighbours.Add(myTileFieldManager.GetTile(new Vector2Int(ownX, ownY + 1)));
            neighbours.Add(myTileFieldManager.GetTile(new Vector2Int(ownX, ownY - 1)));

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
            return myTileFieldManager.GetTile(positionInGrid + offset);
        }

        public Vector2Int GetTilePosition()
        {
            return positionInGrid;
        }
        
        #endregion

        #region TileChangeFunctions

        private void ChangeTileType(TileType newTileType)
        {
            myTileType = newTileType;
            if (newTileType == TileType.Clear)
            {
                myItem.enabled = false;
            }
            else
            {
                myItem.sprite = TileRecourseKeeper.instance.tileSprites[(int)myTileType];
            }
        }

        public void InitializeTile(TileType type, Vector2Int positionInGird, Vector3 localPos)
        {
            myTileType = type;
            positionInGrid = positionInGird;
            if (myTileType == TileType.Clear) myItem.enabled = false;
            else
            {
                myItem.enabled = true;
                myItem.sprite = TileRecourseKeeper.instance.tileSprites[(int)myTileType];
            }

            positionInScene = localPos;
            transform.localPosition = positionInScene;
        }

        public void SetNewPosition(Vector2Int newPos)
        {
            positionInGrid = newPos;
            positionInScene = myTileFieldManager.GetScenePosition(positionInGrid);
            transform.localPosition = positionInScene;
        }

        #endregion

        #region OnMouseFunctions/Interactions

        private void OnMouseDown()
        {
            if (myTileFieldManager.editMode)
            {
                int id = (int)myTileType + 1;
                if (id > 5) id = -1;
                ChangeTileType((TileType) id);
            }
        }

        private void OnMouseDrag()
        {
            if(myTileFieldManager.editMode) return;
            if (!myTileFieldManager.interactable || myTileType == TileType.Clear)return;
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
                if (PreviewDragedTile)
                {
                    if (PreviewDragedTile.GetTileType() == TileType.Clear) return;
                    PreviewDragedTile.transform.localPosition = PreviewDragedTile.positionInScene;
                }
                PreviewDragedTile = null;
                return;
            }
            Vector2Int tileOffset = Vector2Int.zero;
            if (Mathf.Abs( dragVector.x) > Mathf.Abs(dragVector.y)) tileOffset.x = dragVector.x > 0 ? 1 : -1;
            else tileOffset.y = dragVector.y > 0 ? -1 : 1;

            if (!myTileFieldManager.IsPositionInGrid(positionInGrid + tileOffset) || myTileFieldManager.GetTile(positionInGrid+tileOffset).GetTileType() == TileType.Clear)
            {
                if (PreviewDragedTile) PreviewDragedTile.transform.localPosition = PreviewDragedTile.positionInScene;
                return;
            }

            Tile currentTile = myTileFieldManager.GetTile(positionInGrid + tileOffset);
            if (PreviewDragedTile != currentTile)
            {
                if (PreviewDragedTile) PreviewDragedTile.transform.localPosition = PreviewDragedTile.positionInScene;
                PreviewDragedTile = currentTile;
            }
            PreviewDragedTile.transform.localPosition = positionInScene;
        }

        private void OnMouseUp()
        {
            if(myTileFieldManager.editMode) return;
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

            if (!myTileFieldManager.IsPositionInGrid(positionInGrid+tileOffset) || 
                myTileFieldManager.GetTile(positionInGrid+tileOffset).GetTileType() == TileType.Clear)
            {
                transform.localPosition = positionInScene;
                return;
            }
            
            myTileFieldManager.SwitchTiles(positionInGrid,positionInGrid+tileOffset);
            myTileFieldManager.Turns--;
        }
        #endregion
    }
}
