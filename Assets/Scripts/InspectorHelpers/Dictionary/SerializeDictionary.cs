using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeScripts.InspectorHelpers.Dictionary
{
    [Serializable]
    public class SerializeDictionary<TKey, TValue>
    {
        public Dictionary<TKey, TValue> Dictionary = new();

        [Serializable]
        private struct KeyValuePair<TKey, TValue>
        {
            [SerializeField] public TKey Key;
            [SerializeField] public TValue Value;
        }

        [SerializeField] private List<KeyValuePair<TKey, TValue>> _dictionary = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private void SetDictionary()
        {
            foreach (KeyValuePair<TKey, TValue> kvp in _dictionary)
                Dictionary.Add(kvp.Key, kvp.Value);
            Debug.Log(Dictionary.Count);
        }
    }
}
