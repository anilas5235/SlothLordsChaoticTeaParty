using System;
using UnityEngine;

namespace Project.Scripts.General
{
    public class CursorManager : Singleton<CursorManager>
    {
        private Texture2D[] cursorCollection;

        private Cursors currentCursor;

        public enum Cursors
        {
            OpenHand,
            ClosedHand,
        }
        protected override void Awake()
        {
            base.Awake();
            cursorCollection = new[]
            {
                Resources.Load<Texture2D>("ArtWork/Cursor/Cursor_grab"),
                Resources.Load<Texture2D>("ArtWork/Cursor/Cursor_grabbing")
            };
        }

        public void ChangeCursor(Cursors newCursor)
        {
            if (currentCursor == newCursor)return;
            currentCursor = newCursor;
            switch (currentCursor)
            {
                case Cursors.OpenHand: Cursor.SetCursor(cursorCollection[0],new Vector2(11,14), CursorMode.Auto);
                    break;
                case Cursors.ClosedHand: Cursor.SetCursor(cursorCollection[1],new Vector2(11,14), CursorMode.Auto);
                    break;
            }
        }
    }
}
