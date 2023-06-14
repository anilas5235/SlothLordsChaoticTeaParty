using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.General;
using Project.Scripts.UIScripts;
using Project.Scripts.UIScripts.Menu;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Tiles
{
    /// <summary>
    ///   <para>This component managers the tile play field and its behaviours</para>
    /// </summary>
    public class TileFieldManager : Singleton<TileFieldManager>
    {
        [Header("Data")]
        [SerializeField] private Level levelData;
        
        public Vector2Int fieldSize = new Vector2Int( 10, 7);
        public int score, comboRoll, turns = 20;
        public float[] probabilities = { 100 / 6f, 100 / 6f, 100 / 6f, 100 / 6f, 100 / 6f, 100 / 6f };
        public Tile.TileType preferredTile, dislikedTile;
        public int goodScore, perfectScore;
        
        private Vector3[][] fieldGridPositions;
        private Tile[][] fieldGridTiles;
        private GameObject tilePreFap;

        [Header("EditMode")]
        public bool editMode = false;
        public bool brushTool = false;
        public Tile.TileType brushTileType;

        private int fallingCount;

        private const float TileSize =2f, TileSpacing = 0.2f;
        private const int MinComboSize = 3;
        private const float StepTime = .2f;

        #region Properties

        public int Score
        {
            get => score;
            private set
            {
                score = value;
                StatsUIManager.Instance.UpdateScore();
            }
        }

        public int ComboRoll
        {
            get => comboRoll;
            private set
            {
                comboRoll = value;
                StatsUIManager.Instance.UpdateComboRoll();
            }
        }

        public int Turns
        {
            get => turns;
            set
            {
                turns = value;
                StatsUIManager.Instance.UpdateTurn();
            }
        }

        #endregion

        public bool interactable = true;

        protected override void Awake()
        {
            base.Awake();
            tilePreFap =  Resources.Load<GameObject>("Prefaps/Tiles/Tile1");
        }

        private void Start()
        {
            int levelID = PlayerPrefs.GetInt("levelID", 0);

            levelData = LevelDataLoader.Instance.GetLevelData(levelID);
            MenuWindowsMaster.Instance.OnMenuActiveChange += MenuChange;

            CreateGrid();
        }

        #region GridMoveFunctions

        /// <summary>
        ///   <para>Switches two tile at given positions</para>
        ///   <para>Will aboard if any of the positions are outside the current grid</para>   
        /// </summary>
        public void SwitchTiles(Vector2Int tile1, Vector2Int tile2)
        {
            if (!IsPositionInGrid(tile1) || !IsPositionInGrid(tile2)) return;
            GetTile(tile1).SetNewPosition(tile2);
            GetTile(tile2).SetNewPosition(tile1);
            (fieldGridTiles[tile1.x][tile1.y], fieldGridTiles[tile2.x][tile2.y]) =
                (fieldGridTiles[tile2.x][tile2.y], fieldGridTiles[tile1.x][tile1.y]);
            CheckForCombo(tile2);
            CheckForCombo(tile1);
            StartCoroutine(Falling());
        }
        
        /// <summary>
        ///   <para>Moves a tile from its position to another</para>
        ///   <para>Will aboard if any of the positions are outside the current grid or if there is already a tile at the destination</para>   
        /// </summary>

        public void MoveTile(Vector2Int tilePosition, Vector2Int tileDestination)
        {
            if (!IsPositionInGrid(tilePosition)||!IsPositionInGrid(tileDestination))return;
            if (GetTile(tileDestination))
            {
                Debug.Log("try override Tile ");
                return;
            }

            Tile moveTile = GetTile(tilePosition);
            moveTile.SetNewPosition(new Vector2Int(tileDestination.x, tileDestination.y));
            fieldGridTiles[tileDestination.x][tileDestination.y] = moveTile;
            fieldGridTiles[tilePosition.x][tilePosition.y] = null;
        }

        #endregion

        #region GridCreation

        /// <summary>
        ///   <para>Generates a hole play field based on the provided data</para>   
        /// </summary>
        private void InitializeGridFromData( Level data)
        {
            fieldSize = data.FieldSize;
            probabilities = data.Probabilities;
            turns = data.Turns;
            preferredTile = data.PreferredTile;
            dislikedTile = data.DislikedTile;
            goodScore = data.LevelCompleteScore;
            perfectScore = data.LevelSuccessScore;
            
            fieldGridPositions = new Vector3[fieldSize.x][];
            fieldGridTiles = new Tile[fieldSize.x][];
            
            float x = -1* GetStartingCoordinates(fieldSize.x, TileSize, TileSpacing);
            float y = GetStartingCoordinates(fieldSize.y,TileSize,TileSpacing);

            Vector3 xAdd = new Vector3(TileSize + TileSpacing, 0, 0);
            Vector3 yAdd = new Vector3(0, TileSize + TileSpacing, 0);
            
            for (int i = 0; i < fieldSize.x; i++)
            {
                fieldGridPositions[i] = new Vector3[fieldSize.y];
                fieldGridTiles[i] = new Tile[fieldSize.y];
            }
            
            for (int i = 0; i < fieldSize.y; i++)
            {
                Vector3 currentPosition = new Vector3(x, y, 0) + -1* i * yAdd;
                for (int j = 0; j < fieldSize.x; j++)
                {
                    fieldGridPositions[j][i] = currentPosition;
                    NewTile(new Vector2Int(j,i),(Tile.TileType) data.StartingGrid[j].data[i],fieldGridTiles);
                    currentPosition += xAdd;
                }
            }
        }
        
        /// <summary>
        ///   <para>Generates a hole play field corresponding to the current circumstances</para>  
        /// </summary>

        public void CreateGrid()
        {
            if (fieldGridTiles == null)
            {
                if (levelData) InitializeGridFromData(levelData);
                else InitializeGridRandom();
            }
            else
            {
                Tile[][] newTileField = new Tile[fieldSize.x][];
                fieldGridPositions = new Vector3[fieldSize.x][];

                for (int i = 0; i < fieldSize.x; i++)
                {
                    newTileField[i] = new Tile[fieldSize.y];
                    fieldGridPositions[i]  = new Vector3[fieldSize.y];
                }
                float x = -1* GetStartingCoordinates(fieldSize.x, TileSize, TileSpacing);
                float y = GetStartingCoordinates(fieldSize.y,TileSize,TileSpacing);
                 
                Vector3 xAdd = new Vector3(TileSize + TileSpacing, 0, 0);
                Vector3 yAdd = new Vector3(0, TileSize + TileSpacing, 0);

                for (int i = 0; i < fieldSize.y; i++)
                {
                    Vector3 currentPosition = new Vector3(x, y, 0) + -1 * i * yAdd;
                    for (int j = 0; j < fieldSize.x; j++)
                    {
                        fieldGridPositions[j][i] = currentPosition;
                        
                        if (j < fieldGridTiles.Length)
                        {
                            if (i < fieldGridTiles[j].Length)
                            {
                                Tile current =  newTileField[j][i] = fieldGridTiles[j][i];
                                current.SetNewPosition(new Vector2Int(j,i));
                            }
                            else NewTile(new Vector2Int(j,i), Tile.TileType.EucalyptusTea,newTileField);
                        }
                        else NewTile(new Vector2Int(j,i), Tile.TileType.EucalyptusTea,newTileField);
                        
                        currentPosition += xAdd;
                    }
                }

                for (int i = 0; i < fieldGridTiles[0].Length; i++)
                {
                    for (int j = 0; j < fieldGridTiles.Length; j++)
                    {
                        if (i >= fieldSize.y || j >= fieldSize.x)
                        {
                            Destroy(fieldGridTiles[j][i].gameObject);
                        }
                    }
                }

                fieldGridTiles = newTileField;
            }
            CameraControl.Instance.PlayFieldSizeChanged();
        }
        
        /// <summary>
        ///   <para>Generates a hole play field Random based of the fieldSize variable</para>
        ///   <para>The tile-types are determent by the current probabilities</para>
        /// </summary>

        private void InitializeGridRandom()
        {
            int tileCountX = fieldSize.x, tileCountY = fieldSize.y;
            fieldGridPositions = new Vector3[tileCountX][];
            fieldGridTiles = new Tile[tileCountX][];

            float x = -1* GetStartingCoordinates(tileCountX, TileSize, TileSpacing);
            float y = GetStartingCoordinates(tileCountY,TileSize,TileSpacing);

            Vector3 xAdd = new Vector3(TileSize + TileSpacing, 0, 0);
            Vector3 yAdd = new Vector3(0, TileSize + TileSpacing, 0);


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
                    NewTile(new Vector2Int(j,i));
                    currentPosition += xAdd;
                }
            }
        }
        private float GetStartingCoordinates(int number,float size , float spacing )
        {
            return (number % 2 ==0) ? (number/2f - 0.5f)* (size+ spacing):
                Mathf.FloorToInt(number / 2f)* (size + spacing);
        }
        
        /// <summary>   
        ///   <para>Generates a tile-type determent by the current probabilities</para>
        /// </summary>
        
        private Tile.TileType GenerateTileType()
        {
            float randomNumber = Random.Range(0f, 100f);
            float prob = probabilities[0];
            int step = 0;

            while (randomNumber > prob && step < probabilities.Length-1)
            {
                step++;
                prob += probabilities[step];
            }
            
            return (Tile.TileType)step;
        }
        
        /// <summary>
        ///   <para>Generates a tile at the position if the position is part of the current grid</para>
        ///   <para>The tile´s type is determent by the current probabilities</para>
        /// </summary>

        private void NewTile(Vector2Int posInGrid)
        {
            if(!IsPositionInGrid(posInGrid)) return;
            Vector3 currentPosition = fieldGridPositions[posInGrid.x][posInGrid.y];
            Tile tile = Instantiate(tilePreFap, currentPosition, quaternion.identity).GetComponent<Tile>();
            tile.gameObject.transform.SetParent(transform);
            tile.InitializeTile( GenerateTileType(),posInGrid,currentPosition);
            fieldGridTiles[posInGrid.x][posInGrid.y] = tile;
        }
        
        /// <summary>
        ///   <para>Generates a specific tile at the position in the named tile grid</para>  
        /// </summary>
        
        private void NewTile(Vector2Int posInGrid, Tile.TileType tileType, Tile[][] tileGrid)
        {
            if(!IsPositionInGrid(posInGrid)) return;
            Vector3 currentPosition = fieldGridPositions[posInGrid.x][posInGrid.y];
            Tile tile = Instantiate(tilePreFap, currentPosition, quaternion.identity).GetComponent<Tile>();
            tile.gameObject.transform.SetParent(transform);
            tile.InitializeTile(tileType,posInGrid,currentPosition);
            tileGrid[posInGrid.x][posInGrid.y] = tile;
        }
        
        #endregion

        #region InfoFunctions
        
        /// <summary>
        ///   <para>Gets the tile at the position if position exists in the current context</para>
        /// </summary>
        public Tile GetTile(Vector2Int position)
        {
            return IsPositionInGrid(position) ? fieldGridTiles[position.x][position.y] : null;
        }
        
        /// <summary>
        ///   <para>Checks if position is part of the current grid</para>
        /// </summary>
        public bool IsPositionInGrid(Vector2Int position)
        {
            return position.x >= 0 && position.x < fieldSize.x && position.y >= 0 && position.y < fieldSize.y;
        }

        public Vector3 GetScenePosition(Vector2Int positionInGrid)
        {
            return !IsPositionInGrid(positionInGrid) ? Vector3.zero : fieldGridPositions[positionInGrid.x][positionInGrid.y];
        }

        private bool CheckForEmptyTiles()
        {
            for (int i = 0; i < fieldSize.y; i++)
            {
                for (int j = 0; j < fieldSize.x; j++)
                {
                    if (fieldGridTiles[j][i] == null) return true;
                }
            }

            return false;
        }

        public void GetPLayFieldAspects(out Vector2Int tileFieldSize, out float tileSize, out float tileSpacing)
        {
            tileFieldSize = fieldSize;
            tileSize = TileSize;
            tileSpacing = TileSpacing;
        }

        public void MenuChange(bool state)
        {
            interactable = !state;
        }

        #endregion

        #region ComboChecks
        
        /// <summary>
        ///   <para>Checks if a tile is part of valid combo</para>
        /// </summary>
        private void CheckForCombo(Vector2Int tilePosition)
        {
            if(!IsPositionInGrid(tilePosition)) return;
            if(!GetTile(tilePosition)) return;
            Tile.TileType tileType = GetTile(tilePosition).GetTileType();
            List<Tile> comboTiles = new List<Tile> { fieldGridTiles[tilePosition.x][tilePosition.y] };
            Comp(tilePosition);
            
            if(comboTiles.Count < MinComboSize) return;
            if (tileType == Tile.TileType.Clear) return;
            ComboRoll++;
            foreach (var t in comboTiles)
            {
                Vector2Int pos = t.GetTilePosition();
                fieldGridTiles[pos.x][pos.y] = null;
                Destroy(t.gameObject);
            }
            ScoreCalculator( comboTiles.Count, tileType);

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
        
        /// <summary>
        ///   <para>Checks if there are any valid combos in the grid´s current state</para>
        /// </summary>
        private void  CheckForAllCombos()
        {
            List<Tile> checkedTiles = new List<Tile>();
            List<Tile> toBeDeleteTiles = new List<Tile>();

            for (int i = 0; i < fieldSize.y; i++)
            {
                for (int j = 0; j < fieldSize.x; j++)
                {
                    if (!checkedTiles.Contains(fieldGridTiles[j][i]))
                    {
                        List<Tile> combTiles = Comp(fieldGridTiles[j][i].GetTilePosition(), fieldGridTiles[j][i].GetTileType());
                        Tile.TileType tileType = fieldGridTiles[j][i].GetTileType();
                        if (combTiles.Count < 3 || tileType == Tile.TileType.Clear) continue;
                        toBeDeleteTiles.AddRange(combTiles);
                        ComboRoll++;
                        ScoreCalculator(combTiles.Count,tileType);
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
                //Falling done
                interactable = true;
                ComboRoll = 0;
                fallingCount = 0;
                if (Turns < 1)
                {
                    int levelID =PlayerPrefs.GetInt("levelID", 0);
                    PlayPreviewWindow.Instance.levelID = levelID;
                    SaveData saveData = SaveSystem.Instance.GetActiveSave();
                    if (score > saveData.highScoresForLevels[levelID])
                    {
                        saveData.highScoresForLevels[levelID] = score;
                        if (saveData.levelsUnlocked.Length - 1 > levelID)
                        {
                            if (!saveData.levelsUnlocked[levelID + 1] && score >= levelData.LevelCompleteScore)
                                saveData.levelsUnlocked[levelID + 1] = true;
                        }

                        SaveSystem.Instance.Save();
                    }
                    MenuWindowsMaster.Instance.OpenWindow(PlayPreviewWindow.Instance);
                }
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
        
        #endregion

        #region FallingBehaviour

        /// <summary>
        ///   <para>Checks if there are any empty tiles in the grid and performs one fall step if needed</para>
        /// </summary>
        private bool CheckAndFall()
        {
            bool doneFalling = true;
            List<Vector2Int> toSkip = new List<Vector2Int>();
            for (int i = fieldSize.y - 1; i >= 0; i--)
            {
                for (int j = 0; j < fieldSize.x; j++)
                {
                    if (fieldGridTiles[j][i]) continue;
                    EmptyTile( new Vector2Int(j, i));
                    doneFalling = false;
                }
            }

            return doneFalling;

            void EmptyTile(Vector2Int thisPosition)
            {
                if (thisPosition.y == 0) {NewTile(thisPosition); return;}

                Tile aboveTile = GetTile(thisPosition + Vector2Int.down);
                if (aboveTile && !toSkip.Contains(thisPosition))
                {
                    if (aboveTile.GetTileType() == Tile.TileType.Clear)
                    {
                        if (ClearTileCase(out Vector2Int position))
                        {
                            if (toSkip.Contains(position)) return;
                            if (GetTile(position))
                            {
                                MoveTile(position, thisPosition);
                                toSkip.Add(thisPosition);
                            }
                        }
                        else NewTile(thisPosition);
                    }
                    else
                    {
                        MoveTile(aboveTile.GetTilePosition(), thisPosition);
                        toSkip.Add(thisPosition + Vector2Int.down);
                    }
                }
                
                //tries to find the right direction and tile to fill up empty spaces at are under clear tiles
                bool ClearTileCase( out Vector2Int switchPosition)
                {
                    switchPosition = Vector2Int.zero;
                    
                    Vector2Int leftInlet = thisPosition;
                    bool left = false;
                    for (int i = thisPosition.x-1; i >= 0; i--)
                    {
                        leftInlet = new Vector2Int(i, thisPosition.y-1);
                        if (!IsPositionInGrid(leftInlet)) { break;}
                        Tile tileToCheck = GetTile(leftInlet);
                        if (!tileToCheck)
                        {
                            left = true;
                            break;
                        }
                        if(tileToCheck.GetTileType() != Tile.TileType.Clear) {
                            left = true;
                            break;
                        }
                    }
                    
                    Vector2Int rightInlet = thisPosition;
                    bool right = false;
                    for (int i = thisPosition.x+1; i < fieldSize.x; i++)
                    {
                        rightInlet = new Vector2Int(i,thisPosition.y-1);
                        if (!IsPositionInGrid(rightInlet)) break;
                        Tile tileToCheck = GetTile(rightInlet);
                        if (!tileToCheck) {
                            right = true;
                            break;
                        }
                        if(tileToCheck.GetTileType() != Tile.TileType.Clear)  {
                            right = true;
                            break;
                        }
                    }

                    
                    switch (left, right)
                    {
                        case (false, false):
                            if (CheckUp(out Vector2Int switchPos))
                            {switchPosition = switchPos; return true;}
                            else return false;
                        
                        case (true, true): switchPosition =
                                Mathf.Abs(thisPosition.x - rightInlet.x) <= Mathf.Abs(thisPosition.x - leftInlet.x)
                                    ? thisPosition + Vector2Int.right : thisPosition + Vector2Int.left;
                            return true;
                        case (true, false): switchPosition = thisPosition + Vector2Int.left; return true;
                        case (false, true): switchPosition = thisPosition + Vector2Int.right; return true;
                    }

                    bool CheckUp(out Vector2Int upPosition)
                    {
                        bool up = false;
                        upPosition = thisPosition;
                        
                        for (int i = thisPosition.y-1; i >= 0; i--)
                        {
                            upPosition = new Vector2Int(i,thisPosition.y-1);
                            if (!IsPositionInGrid(upPosition)) break;
                            Tile tileToCheck = GetTile(upPosition);
                            if (!tileToCheck) {
                                up = true;
                                break;
                            }
                            if(tileToCheck.GetTileType() != Tile.TileType.Clear)  {
                                up = true;
                                break;
                            }
                        }
                        return up;
                    }

                }
            }
        }
        
        /// <summary>
        ///   <para>Falling Routine that runs until nothing can fall anymore</para>
        /// </summary>
        private IEnumerator Falling()
        {
            interactable = false;
            do
            {
                yield return new WaitForSeconds(StepTime * Mathf.Pow(0.95f,fallingCount) +.05f);
                fallingCount++;
            } while (!CheckAndFall());

            interactable = true;
            CheckForAllCombos();
        }

        private void ScoreCalculator(int comboSize, Tile.TileType comboTileType)
        {
            if (comboSize < MinComboSize) return;
            if (comboTileType == Tile.TileType.Clear)return;

            int scoreToAdd = comboSize * comboRoll;
            switch ((comboTileType == preferredTile, comboTileType == dislikedTile))
            {
                case (true,false): scoreToAdd *= 3; break;
                case (false,true): scoreToAdd *= 0; break;
                default: scoreToAdd *= 1; break; 
            }
            Score += scoreToAdd;
        }
        
        #endregion

        #region EditorFunctions
        
        /// <summary>
        ///   <para> This Editor-function saves the current state of the grid as an scriptableObject at the location: Assets/Project/Resources/LevelData/ </para>
        /// </summary>
        public void SaveCurrentGrid()
        {
#if UNITY_EDITOR
            int[][] data = new int[fieldSize.x][];
            for (int i = 0; i < fieldSize.x; i++)
            {
                data[i] = new int[fieldSize.y];
            }

            for (int i = 0; i < fieldSize.y; i++)
            {
                for (int j = 0; j < fieldSize.x; j++) data[j][i] = (int)fieldGridTiles[j][i].GetTileType();
            }

            float[] probes = probabilities;

            Level level = ScriptableObject.CreateInstance<Level>();
            level.LevelDataSet(data,probes, turns, preferredTile,dislikedTile,goodScore,perfectScore);

            int id = 0;
            string path; 
            do
            {
                path = $"Assets/Project/Resources/LevelData/NewLevel{id}.asset";
                id++;
            } while (System.IO.File.Exists(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),path)));

            UnityEditor.AssetDatabase.CreateAsset(level,path);
            UnityEditor.AssetDatabase.SaveAssets();
#endif        
        }
        #endregion
    }
}
