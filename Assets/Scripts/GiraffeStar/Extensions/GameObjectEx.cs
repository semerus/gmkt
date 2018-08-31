using UnityEngine;

namespace GiraffeStar
{
    public static class GameObjectEx
    {
        public static GameObject FindChildByName(this GameObject go, string name)
        {
            var children = go.GetComponentsInChildren<Transform>(true);
            foreach (var child in children)
            {
                if(child.name.Equals(name))
                {
                    return child.gameObject;
                }
            }

            return null;
        }

        public static T GetOrAddComponent<T>(this GameObject go)
            where T: Component
        {
            var comp = go.GetComponent<T>();
            if(comp == null)
            {
                comp = go.AddComponent<T>();
            }

            return comp;
        }
    }
}


