using UnityEngine;

namespace Project.Scripts.General
{
    /// <summary>
    ///   <para>class driving form this class will act as Singletons</para>
    /// </summary>

    public abstract class Singleton<T> : MonoBehaviour where T :MonoBehaviour
    {
        public static T instance { get; private set; }

        protected virtual void Awake()
        {
            if (!instance) instance = gameObject.GetComponent<T>();
            else if(instance.GetInstanceID() != GetInstanceID())
            {
                Destroy(gameObject);
            }
        }
    }
    
    /// <summary>
    ///   <para>class driving form this class will act as Singletons and will be continued through scene loads</para>
    /// </summary>

    public abstract class PersistantSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }
    }
}

