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
                Resources.Load<Sprite>("ArtWork/Tiles/cookie"),
                Resources.Load<Sprite>("ArtWork/Tiles/eukalyptus"),
                Resources.Load<Sprite>("ArtWork/Tiles/maus"),
                Resources.Load<Sprite>("ArtWork/Tiles/mooncake"),
                Resources.Load<Sprite>("ArtWork/Tiles/strawberry"),
                Resources.Load<Sprite>("ArtWork/Tiles/IconButton_Small_Purpel_Circle")
            };
        }
    }
}
