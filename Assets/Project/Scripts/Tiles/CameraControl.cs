using System;
using Project.Scripts.General;
using UnityEngine;

namespace Project.Scripts.Tiles
{
    [RequireComponent(typeof(Camera))]
    public class CameraControl : Singleton<CameraControl>
    {
        public Camera gameCam { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            gameCam = GetComponent<Camera>();
        }
        
        private void Update()
        {
            if (!TileFieldManager.instance.editMode) return;
            transform.position += (Vector3) new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * (Time.deltaTime * 5);
        }
        
        /// <summary>
        ///   <para>Sets the camera size so that you can see the howl grid</para>   
        /// </summary>

        public void PlayFieldSizeChanged()
        {
            TileFieldManager.instance.GetPLayFieldAspects(out Vector2Int size,out float tileSize,out float spacing);

            float tileCombined = tileSize + spacing;

            float width = (size.x * 1.3f * tileCombined) / gameCam.aspect *.5f;
            float height = (size.y * 1.3f) * tileCombined * .5f;

            gameCam.orthographicSize = width > height ? width:height;
        }
    }
}
