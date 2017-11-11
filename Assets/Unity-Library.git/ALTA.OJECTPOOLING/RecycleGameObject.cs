using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Alta.OJECTPOOLING
{
    public class RecycleGameObject : MonoBehaviour
    {

        /// <summary>
        /// The recycle components.
        /// </summary>
        private List<IRecycle> recycleComponents;


        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            // check all recycle component in this gameobject
            var components = GetComponents<MonoBehaviour>();
            recycleComponents = new List<IRecycle>();
            foreach (var component in components)
            {
                if (component is IRecycle)
                {
                    recycleComponents.Add(component as IRecycle);
                }
            }

        }

        /// <summary>
        /// Restarts game object.
        /// </summary>
        public void RestartObject()
        {

            gameObject.SetActive(true);

            foreach (var component in recycleComponents)
            {
                component.RestartObject();
            }

        }

        /// <summary>
        /// Disables game object.
        /// </summary>
        public void DisableObject()
        {
            gameObject.SetActive(false);

            foreach (var component in recycleComponents)
            {
                component.DisableObject();
            }
        }
    }
}

