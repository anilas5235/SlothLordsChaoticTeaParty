using Project.Scripts.General;
using Unity.Mathematics;
using UnityEngine;

namespace Project.Scripts.Tiles
{
    public class TileManager : Singleton<TileManager>
    {
        [SerializeField] private GameObject tilePreFap;
        public int fieldSizeX = 10,fieldSizeY = 7;
        [SerializeField] private float tileSize =2f, tileSpacing = 0.2f;
        public Vector3[][] fieldGridPositions;
        public Tile[][] fieldGridTiles;

        protected override void Awake()
        {
            base.Awake();
            InitializeGrid(fieldSizeX,fieldSizeY);
        }

        private void InitializeGrid(int tileCountX, int tileCountY)
        {
            fieldGridPositions = new Vector3[tileCountY][];
            fieldGridTiles = new Tile[tileCountY][];

            float x = -1* GetStartingValue(tileCountX, tileSize, tileSpacing);
            float y = GetStartingValue(tileCountY,tileSize,tileSpacing);

            Vector3 xAdd = new Vector3(tileSize + tileSpacing, 0, 0);
            Vector3 yAdd = new Vector3(0, tileSize + tileSpacing, 0);

            for (int i = 0; i < tileCountY; i++)
            {
                Vector3 currentPosition = new Vector3(x, y, 0) + -1* i * yAdd;
                fieldGridPositions[i] = new Vector3[tileCountX];
                fieldGridTiles[i] = new Tile[tileCountX];
                for (int j = 0; j < tileCountX; j++)
                {
                    fieldGridPositions[i][j] = currentPosition;
                    Tile tile = Instantiate(tilePreFap, currentPosition, quaternion.identity).GetComponent<Tile>();
                    tile.InitializeTile(Tile.TileType.Type0,new Vector2Int(i,j));
                    tile.gameObject.transform.SetParent(transform);
                    tile.gameObject.transform.localPosition = currentPosition;
                    fieldGridTiles[i][j] = tile;
                    currentPosition += xAdd;
                }
            }

            float GetStartingValue(int number,float size , float spacing )
            {
                return (number % 2 ==0) ? (number/2f - 0.5f)* (size+ spacing):
                    Mathf.FloorToInt(number / 2f)* (size + spacing);
            }
        }
        
        public Tile GetTile(int x, int y)
        {
            if (x >=0 && x < fieldSizeX && y >= 0 && y < fieldSizeY) return fieldGridTiles[x][y];
            return null;
        }
    }
}
