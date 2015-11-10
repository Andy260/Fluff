using System;
using System.Linq;
using UnityEngine;

namespace Sheeplosion.Extensions
{
    public static class GameObjectExtensions
    {
        public static T GetInterface<T>(this GameObject a_gameObject)
        {
            // Ensure given type, is an interface
            if (!typeof(T).IsInterface)
            {
                throw new SystemException("Requested type must be an interface");
            }

            // Return first object found by GetInterfaces()
            return a_gameObject.GetInterfaces<T>().FirstOrDefault();
        }

        public static T[] GetInterfaces<T>(this GameObject a_gameObject)
        {
            // Ensure given type, is an interface
            if (!typeof(T).IsInterface)
            {
                throw new SystemException("Requested type must be an interface");
            }

            // Get all scripts attached to this game object
            MonoBehaviour[] monoBehaviours = a_gameObject.GetComponents<MonoBehaviour>();

            // Return array using Linq
            return (from a in monoBehaviours where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
        }

        public static T GetInterfaceInChildren<T>(this GameObject a_gameObject)
        {
            // Ensure given type, is an interface
            if (!typeof(T).IsInterface)
            {
                throw new SystemException("Requested type must be an interface");
            }

            // Return first found object using GetInterfacesInChildren()
            return a_gameObject.GetInterfacesInChildren<T>().FirstOrDefault();
        }

        public static T[] GetInterfacesInChildren<T>(this GameObject a_gameObject)
        {
            // Ensure given type, is an interface
            if (!typeof(T).IsInterface)
            {
                throw new SystemException("Requested type must be an interface");
            }

            // Get all scripts attached to given game object
            MonoBehaviour[] monoBehaviours = a_gameObject.GetComponentsInChildren<MonoBehaviour>();

            // Return array using Linq
            return (from a in monoBehaviours where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
        }

        public static T FindInterface<T>(this GameObject a_gameObject)
        {
            // Ensure given type, is an interface
            if (!typeof(T).IsInterface)
            {
                throw new SystemException("Requested type must be an interface");
            }

            // Return first object found by FindInterfaces()
            return a_gameObject.FindInterfaces<T>().FirstOrDefault();
        }

        public static T[] FindInterfaces<T>(this GameObject a_gameObject)
        {
            // Ensure given type, is an interface
            if (!typeof(T).IsInterface)
            {
                throw new SystemException("Requested type must be an interface");
            }

            // Get all scripts globally
            MonoBehaviour[] monoBehaviours = GameObject.FindObjectsOfType<MonoBehaviour>();

            // Return array using Linq
            return (from a in monoBehaviours where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
        }
    }
}
