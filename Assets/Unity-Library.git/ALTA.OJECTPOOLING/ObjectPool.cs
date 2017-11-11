using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Alta.OJECTPOOLING
{
    public class ObjectPool : MonoBehaviour
    {
        /// <summary>
        /// The minimum item in pool.
        /// </summary>
        /// 
        [HideInInspector]
        public int minItemInPool = 4;

        /// <summary>
        /// The prefab.
        /// </summary>
        public RecycleGameObject prefab;

        /// <summary>
        /// The pool instances.
        /// </summary>
        private List<RecycleGameObject> poolInstances;// = new List<RecycleGameObject>();

        public int MaxItem = 8;

        void Awake()
        {
            //DontDestroyOnLoad(transform.gameObject);
            poolInstances = new List<RecycleGameObject>();
        }


        /// <summary>
        /// Creates the instance from object pool.
        /// </summary>
        /// <returns>The instance.</returns>
        /// <param name="position">Position.</param>
        private RecycleGameObject CreateInstance(Vector2 position)
        {
            // create a clone
            var clone = GameObject.Instantiate(prefab);
            clone.transform.SetParent(transform);
            clone.transform.localScale = Vector3.one;
            clone.GetComponent<RectTransform>().anchoredPosition = position;
            poolInstances.Add(clone);
            return clone;
        }

        /// <summary>
        /// Nexts the object.
        /// </summary>
        /// <returns>The object.</returns>
        public RecycleGameObject NextObject(Vector2 anchoredPosition)
        {
            RecycleGameObject instance = null;

            if (poolInstances.Count < minItemInPool)
            {
                instance = this.CreateInstance(anchoredPosition);
                instance.RestartObject(); // reactivating object
            }
            else if(poolInstances.Count< MaxItem)
            {
                // reactivate object from the bool
              
                foreach (var obj in poolInstances)
                {
                    if (obj.gameObject.activeSelf != true)
                    {
                        instance = obj;
                        instance.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
                    }
                }

                if (instance == null)
                {
                    instance = this.CreateInstance(anchoredPosition);
                }
                instance.RestartObject(); // reactivating object
            }
            
            return instance;

        }
    }
}
