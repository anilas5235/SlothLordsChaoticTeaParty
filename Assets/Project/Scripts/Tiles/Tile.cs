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
        
        
        private Tile previewDragedTile;
        private Camera _camera;
        private TileFieldManager myTileFieldManager;
        private LineRenderer myLineRenderer;
        private static Vector3[] LineRenderPoints = new []{new Vector3(1,1,0),new Vector3(-1,1,0),new Vector3(-1,-1,0),new Vector3(1,-1,0)};

        private bool currentlyDraged = false;
        private bool highLighted = false;
        
        private const float DragRange = 2.2f, MoveThreshold = .5f;

        public enum TileType
        {
            Clear = -1,
            EucalyptusTea = 0,
            AppleTea = 1,
            Mouse = 2,
            Cookie = 3,
            StrawberryCake = 4,
            MoonCake = 5,
        }

        private void Awake()
        {
            myBackground = transform.GetChild(0).GetComponent<SpriteRenderer>();
            myItem = transform.GetChild(1).GetComponent<SpriteRenderer>();
            _camera = Camera.main;
            myTileFieldManager = TileFieldManager.instance;
            myLineRenderer = GetComponent<LineRenderer>();
        }

        private void FixedUpdate()
        {
            if (highLighted)
            {
                HighLight();
            }
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
                myBackground.enabled = false;
            }
            else
            {
                myItem.enabled = true;
                myBackground.enabled = true;
                myItem.sprite = TileRecourseKeeper.instance.tileSprites[(int)myTileType];
            }
        }

        public void InitializeTile(TileType type, Vector2Int positionInGird, Vector3 localPos)
        {
            myTileType = type;
            positionInGrid = positionInGird;
            if (myTileType == TileType.Clear)
            {
                myItem.enabled = false;
                myBackground.enabled = false;
            }
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

        public void HighLight()
        {
            myLineRenderer.enabled = true;
            Vector3[] points = new Vector3[4];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = LineRenderPoints[i] + transform.position;
            }
            myLineRenderer.SetPositions(points);
        }

        #endregion

        #region OnMouseFunctions/Interactions

        private void OnMouseDown()
        {
            if (!myTileFieldManager.editMode) return;
            if (myTileFieldManager.brushTool)
            {
                ChangeTileType(myTileFieldManager.brushTileType);
            }
            else
            {
                int id = (int)myTileType + 1;
                if (id > 5) id = -1;
                ChangeTileType((TileType)id);
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
                if (previewDragedTile)
                {
                    if (previewDragedTile.GetTileType() == TileType.Clear) return;
                    previewDragedTile.transform.localPosition = previewDragedTile.positionInScene;
                }
                previewDragedTile = null;
                return;
            }
            Vector2Int tileOffset = Vector2Int.zero;
            if (Mathf.Abs( dragVector.x) > Mathf.Abs(dragVector.y)) tileOffset.x = dragVector.x > 0 ? 1 : -1;
            else tileOffset.y = dragVector.y > 0 ? -1 : 1;

            if (!myTileFieldManager.IsPositionInGrid(positionInGrid + tileOffset) || myTileFieldManager.GetTile(positionInGrid+tileOffset).GetTileType() == TileType.Clear)
            {
                if (previewDragedTile) previewDragedTile.transform.localPosition = previewDragedTile.positionInScene;
                return;
            }

            Tile currentTile = myTileFieldManager.GetTile(positionInGrid + tileOffset);
            if (previewDragedTile != currentTile)
            {
                if (previewDragedTile) previewDragedTile.transform.localPosition = previewDragedTile.positionInScene;
                previewDragedTile = currentTile;
            }
            previewDragedTile.transform.localPosition = positionInScene;
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
            highLighted = !highLighted;
            if (!highLighted) myLineRenderer.enabled = false;
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
