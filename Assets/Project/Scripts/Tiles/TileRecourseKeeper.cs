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
                Resources.Load<Sprite>("ArtWork/Tiles/eukalyptus"),
                Resources.Load<Sprite>("ArtWork/Tiles/tea"),
                Resources.Load<Sprite>("ArtWork/Tiles/maus2"),
                Resources.Load<Sprite>("ArtWork/Tiles/cookie"),
                Resources.Load<Sprite>("ArtWork/Tiles/strawberry"),
                Resources.Load<Sprite>("ArtWork/Tiles/mooncake"),
            };
        }
    }
}
