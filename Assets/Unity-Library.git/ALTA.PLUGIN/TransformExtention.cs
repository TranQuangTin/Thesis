using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Alta.Plugin
{
    public static class TranformExtention
    {
        /// <summary>
        /// detach children of game object
        /// </summary>
        /// <param name="tr">tranform of game object</param>
        public static void ClearChildren(this Transform tr)
        {
            foreach (Transform child in tr)
            {
                GameObject.Destroy(child.gameObject);
            }
            tr.DetachChildren();
        }

        public static void Reset(this Transform tr)
        {
            tr.localPosition = Vector3.zero;
            tr.localRotation = Quaternion.identity;
            tr.localScale = Vector3.one;
        }
        /// <summary>
        /// create child of game object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">game object parent</param>
        /// <param name="child">Componect chid</param>
        /// <param name="activeSelf"></param>
        /// <returns>component</returns>
        public static T CreateChild<T>(this Transform parent, T child, bool activeSelf = true) where T: Component
        {
            T go = GameObject.Instantiate(child);           
            go.transform.SetParent(parent);
            go.transform.Reset();
            go.gameObject.SetActive(activeSelf);
            return go;
        }
        public static GameObject CreateChild(this Transform parent, GameObject child)
        {
            GameObject go = GameObject.Instantiate(child);
            go.transform.SetParent(parent);
            go.transform.Reset();
            return go;
        }

    }
}
