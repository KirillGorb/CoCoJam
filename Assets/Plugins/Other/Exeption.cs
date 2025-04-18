using UnityEngine;

namespace Plugins.Other
{
    public static class Exeption
    {
        public static void ClearChild(this Transform transform)
        {
            foreach (Transform tr in transform)
                Object.Destroy(tr.gameObject);
        }
    }
}