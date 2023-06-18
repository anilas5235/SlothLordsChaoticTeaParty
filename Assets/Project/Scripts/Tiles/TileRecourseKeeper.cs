using Project.Scripts.General;
using UnityEngine;

namespace Project.Scripts.Tiles
{
    public class TileRecourseKeeper : Singleton<TileRecourseKeeper>
    {
        public Sprite[] tileSprites;
        public Sprite[] brokenTileSprites;
        public Color32[] tileBackgroundColors = new Color32[6];

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

            brokenTileSprites = new[]
            {
                Resources.Load<Sprite>("ArtWork/Tiles/BrokenEukalyptus"),
                Resources.Load<Sprite>("ArtWork/Tiles/BrokenTea"),
                Resources.Load<Sprite>("ArtWork/Tiles/BrokenMaus"),
                Resources.Load<Sprite>("ArtWork/Tiles/BrokenCookie"),
                Resources.Load<Sprite>("ArtWork/Tiles/BrokenStrawberry"),
                Resources.Load<Sprite>("ArtWork/Tiles/BrokenMooncake"),
            };

            tileBackgroundColors = new[]
            {
                new Color32(0xFF, 0x83, 0xB1, 0xFF),
                new Color32(0x20,0xFF,0xF5,0xFF),
                new Color32(0x97,0x47,0xF1,0xFF),
                new Color32(0x42,0x81,0xFF,0xFF),
                new Color32(0xB6,0xEE,0x37,0xFF),
                new Color32(0xA5,0xE4,0xFF,0xFF),
            };
        }
    }
}
