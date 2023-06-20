using Project.Scripts.General;
using UnityEngine;

namespace Project.Scripts.Tiles
{
    public class LevelDataLoader : Singleton<LevelDataLoader>
    {
        public Level GetLevelData(int levelID)
        {
            return levelID switch
            {
                0 => Resources.LoadAll<Level>("LevelData/Tutorial/")[0],
                1 => Resources.LoadAll<Level>($"LevelData/Level {levelID}/")[0],
                2 => Resources.LoadAll<Level>($"LevelData/Level {levelID}/")[0],
                3 => Resources.LoadAll<Level>($"LevelData/Level {levelID}/")[0],
                _ => null
            };
        }
    }
}
