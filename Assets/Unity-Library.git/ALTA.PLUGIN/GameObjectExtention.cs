using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alta.Plugin
{
    public static class GameObjectHelper
    {
        /// <summary>
        /// copy component form a game object to another game object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destination"></param>
        /// <param name="original"></param>
        /// <returns></returns>
        public static GameObject CopyComponent<T>(this GameObject destination, GameObject original) where T : Component
        {
            System.Type type = original.GetComponent<T>().GetType();
            Component copy = destination.AddComponent(type);
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                Debug.Log(field.ToString());
                field.SetValue(copy, field.GetValue(original));
            }
            return destination;
        }

    } 
}
