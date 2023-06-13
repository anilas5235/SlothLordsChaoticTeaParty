using Project.Scripts.General;

namespace Project.Scripts.UIScripts
{
    public class SingleWindow<T> : UIMenuWindowHandler,IShouldForceAwake where T : UIMenuWindowHandler
    {
        public static T Instance { get; private set; }

        private bool woken;

        protected virtual void Awake()
        {
            if(woken)return;
            if (!Instance) Instance = gameObject.GetComponent<T>();
            else if(Instance.GetInstanceID() != GetInstanceID())
            {
                Destroy(gameObject);
            }

            woken = true;
        }

        public void ForceAwake()
        {
            if(woken)return;
            Awake();
        }
    }
}
