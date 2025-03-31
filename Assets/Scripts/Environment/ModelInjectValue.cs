using UnityEngine;

namespace CodeScripts.Environment
{
    public abstract class ModelInjectValue : ScriptableObject
    {
        public float timeActiveState;

        public abstract void SetValueMax<T>(T value);
    }
}