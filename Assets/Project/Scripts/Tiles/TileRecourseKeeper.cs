using Project.Scripts.General;
using UnityEngine;

namespace Project.Scripts.Tiles
{
    public class TileRecourseKeeper : Singleton<TileRecourseKeeper>
    {
        public Sprite[] tileSprites;

        protected override void Awake()
        {
            base.Awake();

            tileSprites = new[]
            {
                Resources.Load<Sprite>("ArtWork/Tiles/IconButton_Small_Blue_Circle"),
                Resources.Load<Sprite>("ArtWork/Tiles/IconButton_Small_Circle"),
                Resources.Load<Sprite>("ArtWork/Tiles/IconButton_Small_Green_Circle"),
                Resources.Load<Sprite>("ArtWork/Tiles/IconButton_Small_Orange_Circle"),
                Resources.Load<Sprite>("ArtWork/Tiles/IconButton_Small_Red_Circle")
            };
        }
    }
}
