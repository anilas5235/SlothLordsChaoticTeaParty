using Project.Scripts.General;
using UnityEngine;

namespace Project.Scripts.UIScripts.Effects
{
    public class ScorePopUpPool : ObjectPooling<ScorePopUpPool>
    {
        protected override void Start()
        {
            base.Start();
            objectToPool = Resources.Load<GameObject>("Prefaps/Effects/FloatingText");
        }
    }
}
