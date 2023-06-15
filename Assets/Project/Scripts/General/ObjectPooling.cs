using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Scripts.General
{
    public abstract class ObjectPooling<T> : Singleton<T> where T:MonoBehaviour
    {
        [SerializeField] protected GameObject objectToPool;
        private List<GameObject> objectPool;
        private int totalObjectCount;
        
        protected virtual void Start()
        {
            objectPool = new List<GameObject>() ;
        }

        public virtual void AddObjectToPool(GameObject obj)
        {
            if (objectPool.Contains(obj)) return;
            AddObjectExtraCommands(obj);
            objectPool.Add(obj);
            UpdateName();
            obj.SetActive(false);
        }

        protected virtual void AddObjectExtraCommands(GameObject obj)
        {
            obj.transform.position = transform.position;
            obj.transform.SetParent(transform);
        }

        public virtual GameObject GetObjectFromPool()
        {
            GameObject returnObj = null;
            if (!objectPool.Any())
            {
                returnObj = Instantiate(objectToPool, transform.position, Quaternion.identity);
                returnObj.gameObject.name = $"{objectToPool.name}({totalObjectCount})";
                totalObjectCount++;
            }
            else
            {
                returnObj = objectPool.First();
                objectPool.Remove(returnObj);
                GetObjectExtraCommands(returnObj);
                returnObj.SetActive(true);
            }
            
            UpdateName();
            return returnObj;
        }
        
        protected virtual void GetObjectExtraCommands(GameObject obj)
        {
            obj.transform.SetParent(null);
        }


        private void UpdateName()
        {
            gameObject.name = $"{objectToPool.name}Pool({transform.childCount})";
        }
    }
}
