using UnityEngine;

namespace CodeScripts.PlayerInteraction
{
    [CreateAssetMenu(fileName = "DragDropConfig", menuName = "DragDropConfig", order = 0)]
    public class DragDropConfig : ScriptableObject
    {
        public float MaxDist;
        public float Speed;
        public float Gravity;
    }
}